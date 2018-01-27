
﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelMaster : MonoBehaviour {

    public GameObject radioPrefab;
    public GameObject mainRadio;
    public GameObject mainTv;
    public GameObject transBubble;

	// Use this for initialization
	void Start ()
    {
     
        CreateRadioMinions(1);

        CreateTvMinions(1);

    }

    private void CreateTvMinions(int count)
    {
        for (int i = 1; i <= count; i++)
        {
            float x = UnityEngine.Random.Range(4f, -80f);
            float z = UnityEngine.Random.Range(-77f, -46f);
            Vector3 v = new Vector3(x, 2.6f, z);

            var newMinion = Instantiate(mainTv, v, Quaternion.Euler(0f, UnityEngine.Random.Range(0f, 360f), 0f));            

            float size = 1.75f;
            newMinion.transform.localScale = new Vector3(size, size, size);

            newMinion.tag = "TvMinion";
        }
    }

    private void CreateRadioMinions(int count)
    {
        for (int i = 1; i <= count; i++)
        {
            float x = UnityEngine.Random.Range(4f, -80f);
            float z = UnityEngine.Random.Range(-1f, 6f);
            Vector3 v = new Vector3(x, 1, z);
            var newMinion = Instantiate(radioPrefab, v, Quaternion.Euler(0f, UnityEngine.Random.Range(0f, 360f), 0f));
            
            float size = 10f;
            newMinion.transform.localScale = new Vector3(size, size, size);

            newMinion.tag = "RadioMinion";
        }
    }

    // Update is called once per frame
    void Update () {
		
	}
}
