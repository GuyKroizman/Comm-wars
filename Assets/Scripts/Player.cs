using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {
    public float speed = 3f;
    public KeyCode up;
    public KeyCode down;
    public KeyCode right;
    public KeyCode left;
    public GameObject transmissionBubble;
    
    void Update () {
        if (Input.GetKey(up))
            transform.Translate(Vector3.forward * speed );
        if (Input.GetKey(down))
            transform.Translate(Vector3.back * speed );
        if (Input.GetKey(left))
        {
            Vector3 temp = transform.rotation.eulerAngles;
            temp.y = temp.y - 2.0f;
            transform.rotation =  Quaternion.Euler(temp);
        }
        if (Input.GetKey(right))
        {
            Vector3 temp = transform.rotation.eulerAngles;
            temp.y = temp.y + 2.0f;
            transform.rotation = Quaternion.Euler(temp);
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Vector3 currentPos = this.transform.position;
            currentPos.y += 1f;
            var transmissionObjectInstance = Instantiate(transmissionBubble, currentPos, Quaternion.Euler(0f, 0f, 0f));
        }
    }
}
