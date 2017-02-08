using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class enemymode : MonoBehaviour {

    enum ENEMYSTATE
    {
        NONE = -1,
        IDLE = 0,
        MOVE,
        ATTACK,
        DAMAGE,
        DEAD
    }
    bool playerattack=true;
    ENEMYSTATE enemyState = ENEMYSTATE.IDLE;
    public Slider healthSlider;
    int healthPoint = 100;
    public int enemyhealth = 100;
    public int enemypower = 10;

    bool isDead=false;
    public bool animationeffect;
    float stateTime = 0.0f;
    float animtime = 0.0f;
    float deadtime = 0.0f;
    [Header("Idle")]
    public float idleStateMaxTime = 2.0f;

    Animator anim;
    Animation anima;
    Transform target = null;
    Transform enemycharacter = null;
    CharacterController characterController = null;

    [Header("move")]

    public float moveSpeed = 5.0f;//스크롤로 바뀐다
    public float rotationSpeed = 10.0f;
    public float attackRange = 5f;

    public float attackStateMaxTime = 2.0f;

    playermode playerstate = null;
    bool walk=false;
    Vector3 dir;
    void Awake()
    {
        InitDinoSaurs();
        target = GameObject.FindGameObjectWithTag("Player").transform;
        enemycharacter = transform;
        characterController = GetComponent<CharacterController>();
        if(animationeffect==false)
            anim = GetComponent<Animator>();
        else
            anima = GetComponent<Animation>();
        playerstate = target.GetComponent<playermode>();
    }

 //   void Start()
//    {
        
 //   }

       
    void OnEnable()
    {
        InitDinoSaurs();
    }

    void InitDinoSaurs()
    {
        healthPoint = enemyhealth;

        enemyState = ENEMYSTATE.IDLE;
       
    }

    public void AciveDamage()
    {
        enemyState = ENEMYSTATE.DAMAGE;
    }

    void OnCollisionEnter(Collision other)
    {
        Debug.Log(other.gameObject.name);
        if (other.gameObject.name.Contains("tiger_idle_2") == false)
            return;

        enemyState = ENEMYSTATE.DAMAGE;
    }

    
    // Update is called once per frame
    void Update()
    {
        Debug.Log(transform);
        if (target != null)
            dir = target.position - enemycharacter.position;
        else 
        {
             dir = Vector3.zero;
            
        }
           
        if (Input.GetKey(KeyCode.Z))
            playerattack = true;
        else if (Input.GetKey(KeyCode.X))
            playerattack = false;

        if (healthPoint <= 0)
            enemyState = ENEMYSTATE.DEAD;
        if(playerstate.playerhealth<=0)
            enemyState = ENEMYSTATE.DAMAGE;

        switch (enemyState)
        {
            case ENEMYSTATE.IDLE:
                {
                    if (animationeffect == false)
                        anim.SetBool("Move_bat", walk);
                    else
                        anima.CrossFade("Idle");
                    stateTime += Time.deltaTime;
                    if (stateTime > idleStateMaxTime)
                    {
                        stateTime = 0.0f;
                        enemyState = ENEMYSTATE.MOVE;
                        walk = true;
                    }
                }
                break;
            case ENEMYSTATE.MOVE:
                {
                    if (animationeffect == false)
                        anim.SetBool("Move_bat", walk);
                    else
                        anima.CrossFade("Walk");
                    float distance = 0f ;
                    if(enemyState==ENEMYSTATE.MOVE)
                        distance= (target.position - enemycharacter.position).magnitude;
                   
                    if (distance < attackRange)
                    {
                        enemyState = ENEMYSTATE.ATTACK;

                        stateTime = attackStateMaxTime;
                    }
                    else
                    {
                        

                        dir.y = 0.0f;
                        dir.Normalize();
                        characterController.SimpleMove(dir * moveSpeed);

                        
                    }
                    enemycharacter.rotation = Quaternion.Lerp(enemycharacter.rotation, Quaternion.LookRotation(dir), rotationSpeed * Time.deltaTime);

                }
                break;
            case ENEMYSTATE.ATTACK:
                {
                    dir.y = 0.0f;
                    dir.Normalize();
                    enemycharacter.rotation = Quaternion.Lerp(enemycharacter.rotation, Quaternion.LookRotation(dir), rotationSpeed * Time.deltaTime);
                    walk = false;
                    if (playerattack)
                    {
                        stateTime += Time.deltaTime;
                        if (stateTime > attackStateMaxTime)
                        {

                            if (animtime == 0)
                            {
                                if (animationeffect == false)
                                    anim.SetTrigger("Isattack_bat");
                                else
                                    anima.CrossFade("Attack1");
                            }
                            animtime += Time.deltaTime;
                            if (animtime > 0.4f)
                            {
                                playerstate.DamageByEnemy(enemypower);

                                animtime = 0.0f;
                                stateTime = 0.0f;
                            }
                        }
                    }

                    float distance = (target.position - enemycharacter.position).magnitude;
                    if (distance > attackRange)
                    {
                        enemyState = ENEMYSTATE.IDLE;
                        
                    }
                    else if(playerstate.playerhealth<=0)
                        enemyState = ENEMYSTATE.DAMAGE;
                }
                break;
            case ENEMYSTATE.DAMAGE:
                {
                    walk =false;
                    if (animationeffect == false)
                        anim.SetBool("Move_bat", walk);
                    else
                        anima.CrossFade("Idle");
                }
                break;
            case ENEMYSTATE.DEAD:
                {
                    //Destroy(gameObject);

                    if (animationeffect == false && deadtime == 0f)
                    {
                        anim.SetTrigger("Isdead_bat");
                        Debug.Log(deadtime);
                    }

                    else if (animationeffect == true && deadtime == 0f)
                        anima.CrossFade("Death");
                    deadtime += Time.deltaTime;
                    if (deadtime > 2f)
                        gameObject.SetActive(false);
                    

                }
                break;
        }

    }
    
    public void DamageByEnemy(int power)
    {
        if (isDead)
            return;

        healthPoint = healthPoint - power;
        healthSlider.value = healthPoint;
       // Debug.Log(healthPoint);

        //cameraShake.PlayCameraShake();

        if (healthPoint <= 0)
        {
            isDead = true;
        }
    }

    IEnumerator DeadProcess()
    {
        
       
            

        

        yield return new WaitForSeconds(1.0f);
        
        // Destroy(gameObject);
        gameObject.SetActive(false);
    }
    IEnumerator attackProcess()
    {
        
            yield return new WaitForSeconds(2.0f);
        
    }
}
