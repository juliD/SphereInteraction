using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

public class Marker_Generator : MonoBehaviour {
    public GameObject marker;
    private Vector2[] coords;
    private float radius;
    private Vector3 center;
    private int n = 0;
    private float Current_Time;
    private StreamWriter writer;
    private bool marker_generated;
    private string path;
    private int scene_id;
    private int old_id;

    public bool markers_generated()
    {
        return marker_generated;
    }
    public float TimeCount()
    {
        return Current_Time;
    }

    // Use this for initialization
    void Start () {
        marker_generated = false;
        path = "Assets/Log/logs.txt";
        scene_id = GameObject.Find("Interaction Handler").GetComponent<RotationInteraction>().scene_id;
        old_id = scene_id;
        Current_Time = 0f;
    }
	
	// Update is called once per frame
	void Update () {
        scene_id = GameObject.Find("Interaction Handler").GetComponent<RotationInteraction>().scene_id;
        if (scene_id != old_id)
        {
            for (int n = 0; n < coords.Length; n++)
            {
                Destroy(GameObject.Find((n).ToString()));
            }
            marker_generated = false;
            old_id = scene_id;
            Current_Time = 0f;
        }

        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("Creating markers now...");
            //Destroy all previous markers
            if (marker_generated)
            {
                for (int n = 0; n < coords.Length; n++)
                {
                    Destroy(GameObject.Find((n).ToString()));
                }
            }
            switch (scene_id)
            {
                case 1:
                    coords = new Vector2[] {
                        new Vector2(-60f, 50f),
                        new Vector2(-90f, 0f),
                        new Vector2(-25f, 0f),
                        new Vector2(-120f, -15f),
                        new Vector2(-130f, -50f),
                        new Vector2(-30f, 70f),
                        new Vector2(-210f, -30f),
                        new Vector2(-280f, 10f),
                        new Vector2(-255f, 60f),
                        new Vector2(-280f, 65f),
                        new Vector2(-310f, 60f),
                        new Vector2(35f, -20f)
                    };
                    break;
                case 2:
                    coords = new Vector2[] {
                        new Vector2(330f, 50f),
                        new Vector2(260f, 10f),
                        new Vector2(260f, 40f),
                        new Vector2(310f, -15f),
                        new Vector2(310f, 3f),
                        new Vector2(210f, 10f),
                        new Vector2(330f, -40f),
                        new Vector2(30f, -30f),
                        new Vector2(60f, 30f),
                        new Vector2(130f, 10f),
                        new Vector2(140f, 45f),
                        new Vector2(120f, -25f)
                    };
                    break;
                case 3:
                    coords = new Vector2[] {
                        new Vector2(50f, 45f),
                        new Vector2(50f, 20f),
                        new Vector2(65f, 0f),
                        new Vector2(125f, 0f),
                        new Vector2(200f, 15f),
                        new Vector2(230f, 0f),
                        new Vector2(220f, 35f),
                        new Vector2(210f, -30f),
                        new Vector2(240f, -40f),
                        new Vector2(260f, 10f),
                        new Vector2(260f, -20f),
                        new Vector2(300f, 50f)
                    };
                    break;
                case 4:
                    coords = new Vector2[] {
                        new Vector2(25f, 35f),
                        new Vector2(35f, 15f),
                        new Vector2(50f, 30f),
                        new Vector2(120f, 0f),
                        new Vector2(150f, -30f),
                        new Vector2(200f, 30f),
                        new Vector2(290f, 50f),
                        new Vector2(310f, 50f),
                        new Vector2(290f, -50f),
                        new Vector2(280f, -30f),
                        new Vector2(300f, -30f),
                        new Vector2(290f, 0f)
                    };
                    break;
            }
            GenerateMarkers(coords);
            marker_generated = true;
            Current_Time = 0f;
            writer = new StreamWriter(path, true);
            float current_time = GameObject.Find("Interaction Handler").GetComponent<RotationInteraction>().Time_Stamp();
            writer.WriteLine("-----------");
            writer.WriteLine(("Current Time:" + current_time + "; Task:" + scene_id + " begins"));
            writer.Close();
            Debug.Log("Created all Markers");

        }
        Current_Time += Time.deltaTime;//caculate time finishing a task

    }

    //tranform V2 array to V3 array
    public void GenerateMarkers(Vector2[] SphericalCoordinate)
    {
                
        radius = GameObject.Find("Big Sphere").GetComponent<SpiralSphere>().radius;
        if (scene_id < 3)
        {
            radius -= 0.09f;
        }
        else
        {
            radius += 0.03f;
        }

        for (int c = 0; c < SphericalCoordinate.Length; c++)
        {
            //calculate 3D position from 2D
            float longitude = SphericalCoordinate[c].x * Mathf.PI / 180;//fita
            float latitude = (90.0f - SphericalCoordinate[c].y) * Mathf.PI / 180;//theta
            float X = (float)radius * (float)Math.Sin((float)latitude) * (float)Math.Cos((float)longitude);
            float Y = (float)radius * (float)Math.Cos((float)latitude);
            float Z = (float)radius * (float)Math.Sin((float)latitude) * (float)Math.Sin((float)longitude);
            

            //Create Markers
            GameObject Current_Marker;
            center = GameObject.Find("Big Sphere").transform.position;
            Current_Marker = Instantiate(marker, GameObject.Find("Big Sphere").transform.rotation * new Vector3(X + center.x, Y + center.y, Z + center.z), Quaternion.identity) as GameObject;
            Current_Marker.transform.LookAt(center, Vector3.up);
            Current_Marker.transform.parent = gameObject.transform;
            Current_Marker.GetComponent<SpriteRenderer>().color = Color.red;
            Current_Marker.name = c.ToString();

            if (c == 0)
            {
                Current_Marker.tag = "Result";
            }
        }
        
    }

}
