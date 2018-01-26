﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {
    public float speed = 3f;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKey("up"))
            transform.Translate(Vector3.forward * speed );
        if (Input.GetKey("down"))
            transform.Translate(Vector3.back * speed );
        if (Input.GetKey("left"))
            transform.Translate(Vector3.left * speed );
        if (Input.GetKey("right"))
            transform.Translate(Vector3.right * speed );

    }
}
