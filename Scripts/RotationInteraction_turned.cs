using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationInteraction_turned : MonoBehaviour {

    public GameObject small;
    public GameObject big;

	// Use this for initialization
	void Start () {
        
	}
	
	// Update is called once per frame
	void Update () {
        big.transform.position = small.transform.position;
        big.transform.rotation = new Quaternion(small.transform.rotation.x * -1,
            small.transform.rotation.y,
            small.transform.rotation.z,
            small.transform.rotation.w) * Quaternion.Euler(0, 180f, 0); 
    }}
