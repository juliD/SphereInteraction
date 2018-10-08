using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationInteraction_inv : MonoBehaviour {

    public GameObject small;
    public GameObject big;

	// Use this for initialization
	void Start () {
        
	}
	
	// Update is called once per frame
	void Update () {
        big.transform.position = small.transform.position;
        big.transform.rotation = new Quaternion(small.transform.rotation.x,
            small.transform.rotation.y,
            small.transform.rotation.z*-1,
            small.transform.rotation.w*-1); 
    }
}
