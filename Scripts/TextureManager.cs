using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextureManager : MonoBehaviour {

    private int old_id;
    private int scene_id;
    private float time;
    private bool texture_outlined;
	// Use this for initialization
	void Start () {
        scene_id = GameObject.Find("Interaction Handler").GetComponent<RotationInteraction>().scene_id;
        changeTexture("");
        old_id = scene_id;
        texture_outlined = false;
        time = 0f;
    }
	
	// Update is called once per frame
	void Update () {
        scene_id = GameObject.Find("Interaction Handler").GetComponent<RotationInteraction>().scene_id;

        if (old_id != scene_id)
        {
            changeTexture("");
            old_id = scene_id;
            texture_outlined = false;
            time = 0f;
        }
        if(!texture_outlined && time > 50)
        {
            texture_outlined = true;
            changeTexture("_umriss");
        }

        time += Time.deltaTime;
    }
    void changeTexture(string x)
    {
        GameObject.Find("Small Sphere").GetComponent<Renderer>().material.mainTexture = Resources.Load<Texture2D>("Welt_" + scene_id+x);
        GameObject.Find("Big Sphere").GetComponent<Renderer>().material.mainTexture = Resources.Load<Texture2D>("Welt_" + scene_id+x);
    }
}
