using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class finger_position_tracking : MonoBehaviour {
    private GameObject index;
    private Vector3 center;
    public bool big;
    public bool inv;

    // Use this for initialization
    void Start () {
        index = GameObject.Find("Human_RightHandIndex3");
    }

    // Update is called once per frame
    void Update() {
        Vector3 index_position = index.transform.position;
        if (big)
        {
            center = GameObject.Find("Big Sphere").transform.position;
        }else
        {
            center = GameObject.Find("Small Sphere").transform.position;
        }
        
        float distance = Vector3.Distance(index_position, center);
        Vector3 delta = index_position - center;
        float radius = 0;
        if (big)
        {
            radius = GameObject.Find("Big Sphere").GetComponent<SpiralSphere>().radius;
        }
        else
        {
            radius = GameObject.Find("Small Sphere").GetComponent<SpiralSphere>().radius;
        }
        if (inv)
        {
            radius -= 0.005f;
        }
        else
        {
            radius += 0.005f;
        }
        
        float rate = radius/distance;
        gameObject.transform.position = new Vector3(center.x+rate*delta.x, center.y + rate * delta.y, center.z + rate * delta.z);
        gameObject.transform.LookAt(center,Vector3.up);
    }
}
