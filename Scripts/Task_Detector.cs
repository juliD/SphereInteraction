using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using System.IO;


//this script used to test alignment tasks. The marker and the target will turn blue if the task is complete
//Attach this script 

public class Task_Detector : MonoBehaviour {
    public float threshold_Hold_Time_second;//recomend value:0.5s
    private float Current_Time;
    private bool count;
    private bool isComplete;
    private bool isReported;
    public float Fix_Time;
    private float Current_Fix_Time;
    public Material Mat;
    private string path;

    //out put
    private string path_ana;
    private float time_output;
    private float accuracy_output;
    private StreamWriter writer_ana;
    private bool fix_grid;
    private int scene_id;

    // Use this for initialization
    void Start () {
        count = false;
        isComplete = false;
        Current_Time = 0f;
        isReported = false;
        Mat.color = Color.white;
        Current_Fix_Time = 0f;
        path = "Assets/Log/logs.txt";
        path_ana = "Assets/Log/Analysis.txt";
        scene_id = GameObject.Find("Interaction Handler").GetComponent<RotationInteraction>().scene_id;
        if (scene_id == 1 || scene_id == 3 || scene_id == 5 || scene_id == 7)
        {
            fix_grid = true;
        }
        else { fix_grid = false; }
    }
	
	// Update is called once per frame
	void Update () {

        if (count==true && isComplete==false)
        {
            
            Current_Time += Time.deltaTime;
            //Debug.Log(Current_Time);
            if (Current_Time>= threshold_Hold_Time_second && isComplete == false) {
                gameObject.GetComponent<SpriteRenderer>().color = Color.blue;
                GameObject current_marker = GameObject.FindWithTag("Marker");
                //offline test
                current_marker.GetComponent<SpriteRenderer>().color = Color.green;
                isComplete = true;
                time_output = GameObject.FindWithTag("Time").GetComponent<Marker_Generater>().TimeCount();//count total time used for this task
                Debug.Log("isComplete = true; Time is:"+ time_output + " Seconds");
                Debug.Log("Wait for 3 second to report accuracy");
                StreamWriter writer = new StreamWriter(path, true);
                writer.WriteLine("**Aligment is done. Time is:"+ time_output + " Seconds");
                writer.Close();
                return;

            }
            return;

        }

        if (isComplete == true && isReported == false)
        {
            Current_Fix_Time += Time.deltaTime;
           // Debug.Log(Current_Fix_Time);
            if (Current_Fix_Time >= Fix_Time)
            {
                Vector3 position_dot = GameObject.FindWithTag("Marker").transform.position;
                Vector3 position_circle = GameObject.FindWithTag("Target").transform.position;
                Vector3 delta = position_dot - position_circle;
                float distance = (float)Math.Sqrt(Math.Pow(delta.x, 2) + Math.Pow(delta.y, 2) + Math.Pow(delta.z, 2));
                float r = GameObject.Find("Globe_Tracked").GetComponent<SpiralSphere>().radius + 0.004f;
                accuracy_output = 2*((float)Math.Asin(distance/(2*r))*180/Mathf.PI);
                isReported = true;
                Mat.color = Color.gray;
                Debug.Log("Task completed. The distance is: "+ accuracy_output + " degree");
                StreamWriter writer = new StreamWriter(path, true);
                writer.WriteLine("**Fix is done. The distance is: " + accuracy_output + " degree");
                writer.Close();
                //output analysis
                writer_ana = new StreamWriter(path_ana, true);
                int task_id;
                if (fix_grid)
                {
                    task_id = GameObject.Find("Points").GetComponent<Marker_Generater>().index();

                }
                else
                {
                    task_id = GameObject.Find("Globe_Tracked").GetComponent<Marker_Generater>().index();
                }
                
                writer_ana.WriteLine(scene_id + "," + task_id + "," + time_output + "," + accuracy_output);
                writer_ana.Close();

                return;
            }
            return;
        }

    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag=="Marker") {
            count = true;
        }
    }


    void OnTriggerExit(Collider other)
    {
        //reset time counter
        if (other.gameObject.tag == "Marker")
        {
            count = false;
            Current_Time = 0.0f;

        }
    }




}
