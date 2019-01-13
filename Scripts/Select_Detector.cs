using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
//this script is used to detect if a point is selected. Attach this script on the prefab point (red dot to select)

//Update 20.12.2018 by Mengyi
//1. Fixed the Bug during Accuracy measurement. When the finger is not found after 3 second, wait until finger is detected.
//2. reset the count time (threshold_Hold_Time_second) when finger is removed

public class Select_Detector : MonoBehaviour {
    private bool count;//IF finger on Point Last Frame, detected by  private bool Is_in_Touch()
    private bool Last_count;//IF finger on Point Last Frame
    private bool isComplete;//if the point is selected
    private bool isReported;//if the accuracy is reported
    public float threshold_Hold_Time_second;//recomend value:0.5 (better same as Task detector). 
    private float Current_Time;//time counter for selection
    private float Current_Fix_Time;// time counter for accuray measurement. 
    private bool Current_isOnSphere;//current finger status
    private bool Last_isOnSphere;//Finger Status Last Frame
    private GameObject finger_manager;
    public Material Mat;
    public float threshold_dis_finger_target;//recomend value: 0.015
    private string path;
    StreamWriter writer;
    //Out put
    private string path_ana;
    private float time_output;
    private float accuracy_output;
    private StreamWriter writer_ana;
    private int scene_id;

    // Use this for initialization
    void Start () {
        Last_isOnSphere = false;
        count = false;
        isComplete = false;
        isReported = false;
        Current_Time = 0f;
        Current_Fix_Time = 0f;
        finger_manager = GameObject.Find("TaskHandler");
        Mat.color = Color.white;
        path = "Assets/Log/logs.txt";//path for writing Logs
        path_ana = "Assets/Log/Analysis.txt";//path for writhing data for analysis
        scene_id = GameObject.Find("Interaction Handler").GetComponent<RotationInteraction>().scene_id;
        
    }
	
	// Update is called once per frame
	void Update () {

        Current_isOnSphere = finger_manager.GetComponent<Finger_Manager>().Is_in_interaction();
        if (Current_isOnSphere==false && Last_isOnSphere==true) {
            Last_isOnSphere = Current_isOnSphere;
            Current_Time = 0f;//Clear the timecount when finger removed
        }
        if (Current_isOnSphere==true) {
            Last_isOnSphere = Current_isOnSphere;
            Vector3 Current_Finger_Position = GameObject.FindWithTag("Finger").transform.position;
            Vector3 marker_Position = gameObject.transform.position;

            count = Is_in_Touch(Current_Finger_Position, marker_Position);

            if (count == false && Last_count == true)//Clear the timecount when finger removed
            {
                Current_Time = 0f;
            }
            if (count == true && isComplete == false)
            {
                Last_count = count;
                Current_Time += Time.deltaTime;
                if (Current_Time >= threshold_Hold_Time_second && isComplete == false)
                {
                    gameObject.GetComponent<SpriteRenderer>().color = Color.green;
                    isComplete = true;
                    //Get current time for this task from Marker_Generater
                    time_output = GameObject.FindWithTag("Time").GetComponent<Marker_Generater>().TimeCount();
                    Debug.Log("isComplete = true; Time: " + time_output + "second");
                    Debug.Log("Wait for 3 second to report accuracy");
                    writer = new StreamWriter(path, true);
                    writer.WriteLine("**Selection is done. Time is:" + time_output + " Seconds");
                    writer.Close();                  
                    return;

                }
                return;

            }

            if (isComplete == true && isReported == false)
            {
                
                Current_Fix_Time += Time.deltaTime;
          
                if (Current_Fix_Time > 3.0f && Current_isOnSphere==true)
                {
                    
                  //  Vector3 delta = Current_Finger_Position - marker_Position;
                    float dis = Vector3.Distance(Current_Finger_Position, marker_Position);
                    float r = GameObject.Find("Big Sphere").GetComponent<SpiralSphere>().radius;
                    if (scene_id < 3)
                    {
                        r -= 0.004f;
                    }
                    else
                    {
                        r += 0.004f;
                    }
                    accuracy_output = 2 * ((float)Math.Asin(dis / (2 * r)) * 180 / Mathf.PI);
                    isReported = true;
                    Mat.color = Color.gray;
                    
                    writer = new StreamWriter(path, true);
                    writer.WriteLine("**Fix is done. The distance is: " + accuracy_output + " degree");
                    writer.Close();
                    Debug.Log("Task completed. The distance is: " + accuracy_output + " degree");

                    writer_ana = new StreamWriter(path_ana, true);
                    int task_id = GameObject.Find("Points").GetComponent<Marker_Generater>().index(); ;
                    
                    writer_ana.WriteLine(scene_id +","+task_id+ ","+ time_output+ ","+ accuracy_output);
                    writer_ana.Close();
                }

            }
        }
        
        
    }

    private bool Is_in_Touch(Vector3 Finger_Position, Vector3 Marker_Position)
    {
        bool isTouch;
        float distance = Vector3.Distance(Finger_Position, Marker_Position);
        if (distance < threshold_dis_finger_target)
        {
            isTouch = true;
        }
        else
        {
            isTouch = false;
        }

        return isTouch;

    }
}
