using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Scroll : MonoBehaviour {

    public ScrollRect myScrollRect;
    public Scrollbar newScrollBar;    
    Vector3 oldRot;
    

    public void Start()
    {
        
    }

    // Update is called once per frame
    void Update () {
        float deltaRot = Vector3.SignedAngle(transform.forward, oldRot, transform.right);   
        
        if (deltaRot>0)
        {
            if (myScrollRect.verticalNormalizedPosition < 1)
            { 
                myScrollRect.verticalNormalizedPosition += deltaRot / 360;
            } 
        }
        else
        {
            if (myScrollRect.verticalNormalizedPosition > 0)
            {
                myScrollRect.verticalNormalizedPosition += deltaRot / 360;
            }           
            
        }
        Debug.Log(oldRot + " "+ transform.forward);   
        oldRot = transform.forward;
        
    }
}
