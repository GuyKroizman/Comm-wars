
﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelMaster : MonoBehaviour {

    public GameObject radioPrefab;
    public GameObject mainRadio;
    
	// Use this for initialization
	void Start () {
        CreateMainRadio();
        for (int i =1; i<=20; i++)
        {
            float x = UnityEngine.Random.Range(4f, -80f);
            float z = UnityEngine.Random.Range(-77f, 16f);
            Vector3 v = new Vector3(x, 1, z);
            var newMinion = Instantiate(radioPrefab, v, Quaternion.Euler(0f, UnityEngine.Random.Range(0f, 360f), 0f));
            //float size = Random.Range(5f, 25f);
            float size = 10f;
            newMinion.transform.localScale = new Vector3(size, size, size);
        }
        
	}

    private void CreateMainRadio()
    {
        float x = UnityEngine.Random.Range(4f, -80f);
        float z = UnityEngine.Random.Range(-77f, 16f);
        Vector3 v = new Vector3(x, 1, z);
        var mainRadioInstance = Instantiate(mainRadio, v, Quaternion.Euler(0f, UnityEngine.Random.Range(0f, 360f), 0f));
        float size = 20f;
        mainRadioInstance.transform.localScale = new Vector3(size, size, size);
    }

    // Update is called once per frame
    void Update () {
		
	}
}
