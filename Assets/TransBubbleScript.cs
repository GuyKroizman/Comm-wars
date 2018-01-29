using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class TransBubbleScript : MonoBehaviour {

    float transmissionMaxScale = 60f;

    public SphereCollider m_collider;
    // Use this for initialization
    void Start () {
        Vector3 initialScale = new Vector3(9f, 9f, 9f);
        transform.localScale = initialScale;
        
    }
	
	// Update is called once per frame
	void Update () {
        //enlarge
        float resizeFactor = 1.02f;

        transform.localScale = new Vector3(transform.localScale.x * resizeFactor, transform.localScale.y, transform.localScale.z * resizeFactor);
        //transform.localScale = transform.localScale * 1.02f;

        // check when to finish expanding bubble
        if (transform.localScale.x >= transmissionMaxScale)
            Destroy(gameObject);
    }

}
