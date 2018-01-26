using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelMaster : MonoBehaviour {

    public GameObject radioPrefab;

	// Use this for initialization
	void Start () {

        for(int i =0; i<100; i++)
        {
            float x = Random.Range(4f, -80f);
            float z = Random.Range(-77f, 16f);
            Vector3 v = new Vector3(x, 1, z);
            var newMinion = Instantiate(radioPrefab, v, Quaternion.Euler(0f, Random.Range(0f, 360f), 0f));
            newMinion.transform.localScale = new Vector3(5, 5, 5);
        }
        
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
