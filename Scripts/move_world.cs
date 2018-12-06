using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class move_world : MonoBehaviour {

    public GameObject camera;
    Vector3 oldRot;
    public float speed;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        float deltaRot_Forward = Vector3.SignedAngle(transform.forward, oldRot, transform.right);
        camera.transform.position -= camera.transform.forward * deltaRot_Forward * speed; 
        
        Debug.Log(deltaRot_Forward);
        oldRot = transform.forward;
    }
}
