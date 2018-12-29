using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationInteraction : MonoBehaviour {

    public GameObject small;
    public GameObject big;
    public int scene_id = 1;
    public float offset = 20;
    private int old_id;

	// Use this for initialization
	void Start () {
        old_id = scene_id;
        change_scene();
    }
	
	// Update is called once per frame
	void Update () {
        if (old_id != scene_id)
        {
            change_scene();
        }

        switch (scene_id)
        {
            case 1:
                position();
                rotate_inv();
                break;
            case 2:
                position();
                rotate();
                break;
            case 3:
                rotate_inv();
                break;
            case 4:
                rotate();
                break;
            default:
                Debug.LogError("Invalid scene ID");
                break;
        }
            

         
    }
    void position()
    {
        big.transform.position = small.transform.position;
    }

    //inverse rotation
    void rotate_inv()
    {        
        big.transform.rotation = new Quaternion(small.transform.rotation.x,
            small.transform.rotation.y,
            small.transform.rotation.z * -1,
            small.transform.rotation.w * -1);
    }

    //normal rotation
    void rotate()
    {
        big.transform.rotation = small.transform.rotation;
    }

    //change materials and position when scene ID changes 
    void change_scene()
    {
        
        Material front = (Material)Resources.Load("frontCull_inv", typeof(Material));
        Material back = (Material)Resources.Load("backCull", typeof(Material));
        Vector3 pos = small.transform.position;
        pos.z += offset;
        switch (scene_id)
        {            
            case 1:
                small.GetComponent<Renderer>().material = back;
                big.GetComponent<Renderer>().material = front;
                break;
            case 2:
                small.GetComponent<Renderer>().material = front;
                big.GetComponent<Renderer>().material = front;
                break;
            case 3:
                small.GetComponent<Renderer>().material = front;
                big.GetComponent<Renderer>().material = back;
                big.transform.position = pos;
                break;
            case 4:
                small.GetComponent<Renderer>().material = back;
                big.GetComponent<Renderer>().material = back;
                big.transform.position = pos;
                break;
            default:
                break;
        }
        old_id = scene_id;
    }
}
