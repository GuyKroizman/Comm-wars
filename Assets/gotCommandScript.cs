using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gotCommandScript : MonoBehaviour {
    private float deltaTransparency = -0.02f;

    Component[] renderers;


    void Start () {
        renderers = this.GetComponentsInChildren(typeof(Renderer));
    }
	
	void Update () {
        
        foreach (Renderer curRenderer in renderers)
        {
            Color color;
            foreach (Material material in curRenderer.materials)
            {
                color = material.color;
                
                color.g += deltaTransparency;
                if (color.g < 0)
                {
                    color.g = 0;
                    deltaTransparency *= -1f;
                } 
                else if (color.g>1.0f)
                {
                    color.g = 1f;
                    deltaTransparency *= -1f;
                }
            
                material.color = color;
            }
        }
    }
}
