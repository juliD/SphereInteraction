using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class TaskHandler : MonoBehaviour {
    
    public GameObject outlined, outlined_2;
    private Quaternion[] rotations;
    private int task;
    private float time;
    private string path;
    private StreamWriter writer;
    private int scene_id;
    private int old_id;
    private float time_in_parimeter;
    public float threshold;
    private bool in_task;
    GameObject current_outline;

    // Use this for initialization
    void Start () {
        task = 0;
        time = 0f;
        time_in_parimeter = 0f;
        in_task = false;
        path = "Assets/Log/logs.txt";
        rotations = new Quaternion[] {
            //Quaternion.Euler(10,70,0),
            //Quaternion.Euler(40,20,10),
            //Quaternion.Euler(0,-30,90),
            Quaternion.Euler(-60,150,270),
            Quaternion.Euler(100,30,-100)
        };
        scene_id = GameObject.Find("Interaction Handler").GetComponent<RotationInteraction>().scene_id;
    }
	
	// Update is called once per frame
	void Update () {
        scene_id = GameObject.Find("Interaction Handler").GetComponent<RotationInteraction>().scene_id;
        
        //reset everything when scene changes
        if (old_id != scene_id)
        {
            Destroy(current_outline);
            time = 0f;
            task = 0;
            old_id = scene_id;
            in_task = false;
            GameObject.Find("Small Sphere").transform.rotation = Quaternion.identity;
        }

        //new task
        if (Input.GetMouseButtonDown(0))
        {
            
            
            
            if (task > rotations.Length-1)
            {
                Debug.Log("All Tasks Completed");

                in_task = false;
            }
            else
            {
                
                if (scene_id > 2)
                {
                    current_outline = Instantiate(outlined_2, GameObject.Find("Big Sphere").transform.position, rotations[task]) as GameObject;
                }
                else{
                    current_outline = Instantiate(outlined, GameObject.Find("Big Sphere").transform.position, rotations[task]) as GameObject;
                }
                time_in_parimeter = 0f;
                
                in_task = true;
            }
            
        }
        if (current_outline != null)
        {
            current_outline.transform.position = GameObject.Find("Big Sphere").transform.position;
            time += Time.deltaTime;

            float angle = Quaternion.Angle(current_outline.transform.rotation, GameObject.Find("Big Sphere").transform.rotation);
            
            Debug.Log("Angle: "+angle);
            if (angle < threshold && in_task)
            {
                current_outline.GetComponent<Renderer>().material.mainTexture = Resources.Load<Texture2D>("Outlined_green");
                time_in_parimeter += Time.deltaTime;
                Debug.Log("Time in parimeter: " + time_in_parimeter);
                if (time_in_parimeter > 5f)
                {
                    Debug.Log("Close!! Angle. "+angle+ "; Time: " + time+ "; Task "+task);
                    writer = new StreamWriter(path, true);
                    float current_time = GameObject.Find("Interaction Handler").GetComponent<RotationInteraction>().Time_Stamp();
                    
                    writer.WriteLine("Scene 1; Perspective: "+scene_id +"Task: "+task+"; Accuracy: "+ angle+ "; Time: "+time);
                    writer.Close();
                    time_in_parimeter = 0f;

                    Destroy(current_outline);
                    in_task = false;
                    task++;
                }
            
            }

        }
        
        

	}
}
