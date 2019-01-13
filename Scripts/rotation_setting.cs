using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//for OpenLab day presentation
//make Europe data as start position
public class rotation_setting : MonoBehaviour {
    private GameObject GDPloader;
    private bool ischanged;
    private bool isloaded;
	// Use this for initialization
	void Start () {
        // isloaded = GameObject.Find("GDP").GetComponent<gdp_loader>().load();
        ischanged = false;
    }
	
	// Update is called once per frame
	void Update () {
        isloaded = GameObject.Find("GDP").GetComponent<gdp_loader>().load();
        if (isloaded==true && ischanged==false)
        {
            gameObject.transform.rotation = Quaternion.Euler(70f, 60f, 37f);
            ischanged = true;
        }
    }
}
