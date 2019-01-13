using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using System.Linq;
//this script is used to detect if a point is selected. Attach this script on the prefab point (red dot to select)

//Update 20.12.2018 by Mengyi
//1. Fixed the Bug during Accuracy measurement. When the finger is not found after 3 second, wait until finger is detected.
//2. reset the count time (threshold_Hold_Time_second) when finger is removed

public class Selection_Detector : MonoBehaviour
{
    private bool count;//IF finger on Point Last Frame, detected by  private bool Is_in_Touch()
    private int Last_count;//IF finger on Point Last Frame
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
    private GameObject[] markers;
    private bool markers_existing;
    //Out put
    private string path_ana;
    private float time_output;
    private float accuracy_output;
    private int scene_id;

    // Use this for initialization
    void Start()
    {
        Last_count = -1;
        markers_existing = false;
        Last_isOnSphere = false;
        count = false;
        isComplete = false;
        isReported = false;
        Current_Time = 0f;
        Current_Fix_Time = 0f;
        finger_manager = GameObject.Find("TaskHandler");
        Mat.color = Color.white;
        path = "Assets/Log/logs.txt";//path for writing Logs
        scene_id = GameObject.Find("Interaction Handler").GetComponent<RotationInteraction>().scene_id;

    }

    // Update is called once per frame
    void Update()
    {
        scene_id = GameObject.Find("Interaction Handler").GetComponent<RotationInteraction>().scene_id;
        if(GameObject.Find("Big Sphere").GetComponent<Marker_Generator>().markers_generated())
        {
            if (!markers_existing)
            {
                markers = GameObject.FindGameObjectsWithTag("Result").Concat(GameObject.FindGameObjectsWithTag("Marker")).ToArray();
                markers_existing = true;
            }
            
            Current_isOnSphere = finger_manager.GetComponent<Finger_Manager>().Is_in_interaction();
            if (Current_isOnSphere == false && Last_isOnSphere == true)
            {
                Last_isOnSphere = Current_isOnSphere;
                Current_Time = 0f;//Clear the timecount when finger removed
            }
            if (Current_isOnSphere == true)
            {
                Last_isOnSphere = Current_isOnSphere;
                Vector3 Current_Finger_Position = GameObject.FindGameObjectsWithTag("Finger")[1].transform.position;

                if (isComplete == true && isReported == false)
                {

                    Current_Fix_Time += Time.deltaTime;

                    if (Current_Fix_Time > 3.0f && Current_isOnSphere == true)
                    {

                        float dis = Vector3.Distance(markers[0].transform.position, Current_Finger_Position);
                        float dis2 = Vector3.Distance(markers[Last_count].transform.position, Current_Finger_Position);
                        float r = GameObject.Find("Big Sphere").GetComponent<SpiralSphere>().radius;
                        if (scene_id < 3)
                        {
                            r -= 0.09f;
                        }
                        else
                        {
                            r += 0.09f;
                        }
                        accuracy_output = 2 * ((float)Math.Asin(dis / (2 * r)) * 180 / Mathf.PI);
                        float acc = 2 * ((float)Math.Asin(dis2 / (2 * r)) * 180 / Mathf.PI);

                        writer = new StreamWriter(path, true);
                        writer.WriteLine("Scene 2; Task: " + scene_id + "; Accuracy: " + accuracy_output + "; Selection: " + Last_count + "; Selection_Acc: " + acc + " ; Time: " + time_output);
                        writer.Close();

                        for (int n = 0; n < markers.Length; n++)
                        {
                            Destroy(GameObject.Find((n).ToString()));
                        }

                        Last_count = -1;
                        markers_existing = false;
                        Last_isOnSphere = false;
                        count = false;
                        isComplete = false;
                        isReported = false;
                        Current_Time = 0f;
                        Current_Fix_Time = 0f;

                    }

                }

                
                for (int n = 0; n<markers.Length; n++)
                {
                    Vector3 marker_Position = markers[n].transform.position;
                    count = Is_in_Touch(Current_Finger_Position, marker_Position);

                    if (count == false && Last_count == n)//Clear the timecount when finger removed
                    {
                        Current_Time = 0f;
                    }
                    if (count == true && isComplete == false)
                    {
                        Last_count = n;
                        Current_Time += Time.deltaTime;
                        if (Current_Time >= threshold_Hold_Time_second && isComplete == false)
                        {
                            markers[n].GetComponent<SpriteRenderer>().color = Color.green;
                            isComplete = true;
                            time_output = GameObject.FindWithTag("Big Sphere").GetComponent<Marker_Generator>().TimeCount();
                            
                        }
                        return;
                    }

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
