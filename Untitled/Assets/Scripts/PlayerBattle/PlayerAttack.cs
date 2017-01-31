using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour {

    float timer = 0f;
    public float timeBetweenAttack = 1f;
    public float effectsDisplayTime = 0.25f;
    public Rigidbody m_fireBall;
    public Transform m_FireTransform;

    Animator anim;

    bool attacking = false;

    // Use this for initialization
    void Awake () {
        anim = GetComponent<Animator>();
    }
	
	// Update is called once per frame
	void FixedUpdate () {
        timer += Time.deltaTime;
        //Debug.Log("t = " + timer);

        if(Input.GetButton("Fire1") && timer >= timeBetweenAttack && Time.timeScale != 0)
        {
            Attack();
        }

        if(attacking && timer >= 0.5f)
        {
            attacking = false;
            Rigidbody fireBallInstance = Instantiate(m_fireBall, m_FireTransform.position, m_FireTransform.rotation) as Rigidbody;
        }
            
        //if (timer >= timeBetweenAttack * effectsDisplayTime)
        /*else
        {
            attacking = false;
            anim.SetBool("IsAttacking", attacking);
        }*/

    }

    void Attack()
    {
        timer = 0f;

        attacking = true;
        //anim.SetBool("IsAttacking", attacking);
        anim.SetTrigger("Attack");


    }
}
