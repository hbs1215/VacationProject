using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class slidermove : MonoBehaviour {
    Transform player,wheel;
    Vector3 movement;

	// Use this for initialization
	void Start () {
		player = GameObject.FindGameObjectWithTag("Player").transform;
       wheel = GameObject.FindGameObjectWithTag("Wheel").transform;
    }
	
	// Update is called once per frame
	void Update () {
        if (player != null)
        {
            movement = player.position;
            movement.y = 0.1f;
            //movement.Normalize();
            wheel.position = movement;
        }
        else
            gameObject.SetActive(false);
           

		
	}
}
