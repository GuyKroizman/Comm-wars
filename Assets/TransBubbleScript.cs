using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransBubbleScript : MonoBehaviour {

    float transmissionMaxScale = 60f;
    // Use this for initialization
    void Start () {
        Vector3 initialScale = new Vector3(9f, 9f, 9f);
        transform.localScale = initialScale;
    }
	
	// Update is called once per frame
	void Update () {
        //enlarge
        transform.localScale = transform.localScale * 1.02f; // * Time.deltaTime;

        // check when to finish expanding bubble
        if (transform.localScale.x >= transmissionMaxScale)
            Destroy(gameObject);
    }
}
