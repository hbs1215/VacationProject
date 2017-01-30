using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class playermode : MonoBehaviour {

    enum ENEMYSTATE
    {
        NONE = -1,
        IDLE = 0,
        MOVE,
        ATTACK,
        KILL,
        DEAD,
        ESCAPE
    }
  
    ENEMYSTATE enemyState = ENEMYSTATE.IDLE;
    public Slider healthSlider;
    int healthPoint = 2;
    public int playerhealth = 100;
    public int enemypower = 10;
    public int playerpower = 20;
    bool isDead=false;
    bool walk=false;
    float stateTime = 0.0f;
    float animationtime = 0.0f;
    [Header("Idle")]
    public float idleStateMaxTime = 2.0f;

    Animator anim;
    Transform target = null;
    Transform character = null;
    CharacterController characterController = null;

    [Header("move")]

    public float moveSpeed = 5.0f;//스크롤로 바뀐다
    public float rotationSpeed = 10.0f;
    public float attackRange = 5f;

    public float attackStateMaxTime = 3.0f;

    enemymode enemystate = null;

    Vector3 escape;
    void Awake()
    {
        InitDinoSaurs();
        target = GameObject.FindGameObjectWithTag("Enemy").transform;
        character = GameObject.FindGameObjectWithTag("character").transform;
        
        characterController = GetComponent<CharacterController>();
        anim = GetComponent<Animator>();
        enemystate = target.GetComponent<enemymode>();
    }

    //   void Start()
    //    {

    //   }



    void InitDinoSaurs()
    {
        healthPoint = playerhealth;

        enemyState = ENEMYSTATE.IDLE;
        //anim["slime_idle"].speed = 1.5f;
        //anim.CrossFade("slime_idle");
    }






    // Update is called once per frame
    void Update()
    {

        if (Input.GetKey(KeyCode.Z))
            enemyState = ENEMYSTATE.MOVE;
        else if (Input.GetKey(KeyCode.X))
            enemyState = ENEMYSTATE.ESCAPE;

        Vector3 dir = target.position - transform.position;
        if (GameObject.FindGameObjectWithTag("Enemy")!= null)
            target = GameObject.FindGameObjectWithTag("Enemy").transform;
        if(target!=null)
            enemystate = target.GetComponent<enemymode>();

        anim.SetBool("walking", walk);
        if (healthPoint <= 0)
            enemyState = ENEMYSTATE.DEAD;
        switch (enemyState)
        {
            case ENEMYSTATE.IDLE:
                {
                    stateTime += Time.deltaTime;
                    walk = false;
                    if (stateTime > idleStateMaxTime)
                    {
                        stateTime = 0.0f;
                        enemyState = ENEMYSTATE.MOVE;
  
                    }
                }
                break;
            case ENEMYSTATE.MOVE:
                {
                    walk = true;
                    float distance = (target.position - transform.position).magnitude;
                    Debug.Log(distance);
                    if (distance < attackRange)
                    {
                        enemyState = ENEMYSTATE.ATTACK;
                    }
                    else
                    {

                        dir = target.position - transform.position;

                        //Debug.Log(transform.position);
                        // Debug.Log(dir);

                        dir.y = 0.0f;
                        dir.Normalize();
                        characterController.SimpleMove(dir * moveSpeed);

                        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(dir), rotationSpeed * Time.deltaTime);

                    }
                }
                break;
            case ENEMYSTATE.ATTACK:
                {
                    stateTime += Time.deltaTime;
                    dir = target.position - transform.position;
                    transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(dir), rotationSpeed * Time.deltaTime);

                    if (stateTime > attackStateMaxTime)
                    {
                        if(animationtime == 0)
                            anim.SetTrigger("Isattack_tiger");
                        animationtime += Time.deltaTime;
                        if (animationtime>0.6f)
                        {
                           
                            enemystate.DamageByEnemy(playerpower);
                            animationtime = 0f;
                            stateTime = 0.0f;
                        }
                           
                        walk = false;
                    }
                   
                    float distance = (target.position - transform.position).magnitude;
                    if (distance > attackRange)
                    {
                        enemyState = ENEMYSTATE.IDLE;
                    }
                    
                }
                break;
            case ENEMYSTATE.KILL:
               
                break;
            case ENEMYSTATE.DEAD:
                {
                    //Destroy(gameObject);
                    anim.SetTrigger("Die");
                    StartCoroutine("DeadProcess");
                    enemyState = ENEMYSTATE.NONE;
                    
                }
                break;
            case ENEMYSTATE.ESCAPE:
                {
                    walk = true;
                    escape = character.position;
                    escape.z -= 2;

                     dir = escape - transform.position;
                    if (dir.magnitude > 2f)
                    {


                        dir.y = 0.0f;
                        dir.Normalize();
                        characterController.SimpleMove(dir * moveSpeed);

                        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(dir), rotationSpeed * Time.deltaTime);

                    }
                        
                    

                }
                break;

        }

    }



    public void DamageByEnemy(int number)
    {
        if (isDead)
            return;

        healthPoint = healthPoint - number;
        healthSlider.value = healthPoint;
        //cameraShake.PlayCameraShake();

        if (healthPoint <= 0)
        {
            isDead = true;
        }
    }

    IEnumerator DeadProcess()
    {
        //anim["slime_die"].speed = 2.0f;
        //anim.Play("slime_die");
        
        yield return new WaitForSeconds(2.0f);
        
        // Destroy(gameObject);
        gameObject.SetActive(false);
    }
    IEnumerator attackProcess()
    {
        //anim["slime_attack"].speed = 2.0f;
        //anim.Play("slime_attack");

       
            yield return new WaitForEndOfFrame();
        
    }
}
