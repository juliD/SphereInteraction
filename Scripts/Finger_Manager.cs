using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

//generate a black dot when finger is in interaction
//detected by distance between finger and center of sphere
//Handmodel from Hi5Glove can be active with: Find Gameobject "RightHand" under "HI5_Right_Human", Aktive Component "Skinned Mesh Renderer"
public class Finger_Manager : MonoBehaviour {
    public GameObject finger;
    public GameObject finger_big;
    private bool is_in_interaction;
    public float threshold_dis_finger_center;//threshold
    private string path;
    private bool finger_state_last_frame;
    private StreamWriter writer;
    private int scene_id;
    
    private Quaternion angle_last;
    private Quaternion angle_now;
    public GameObject small;
    private float Time_interaction;
    // Use this for initialization

    public bool Is_in_interaction()
    {

        return is_in_interaction;

    }

    void Start () {
        is_in_interaction = false;
        finger_state_last_frame = false;
        path = "Assets/Log/finger_logs.txt";
        angle_last = small.transform.rotation;
        

        
        
        Time_interaction = 0f;
    }

    // Update is called once per frame
    void Update () {
        scene_id = GameObject.Find("Interaction Handler").GetComponent<RotationInteraction>().scene_id;

        Vector3 Index_position = GameObject.Find("Human_RightHandIndex3").transform.position;
        Vector3 center = small.transform.position;
        float distance = Vector3.Distance(Index_position, center);

        if (distance > threshold_dis_finger_center)
        {
            if (GameObject.FindWithTag("Finger"))
            {
                Destroy(GameObject.FindWithTag("Finger"));
            }
            is_in_interaction = false;

            if(is_in_interaction!= finger_state_last_frame)
            {
                float current_time = GameObject.Find("Interaction Handler").GetComponent<RotationInteraction>().Time_Stamp();
                writer = new StreamWriter(path, true);
                writer.WriteLine("Current Time:"+ current_time+"; Event: Hand out.");
                writer.Close();
                if (Time_interaction> 0.1f) {
                    
					angle_now = small.transform.rotation;
					float angle = Quaternion.Angle(angle_now, angle_last);
					if (angle > 5f)
					{
						writer = new StreamWriter(path, true);
						writer.WriteLine("Finished operation: Rotation; It lasts:" + Time_interaction + "Seconds");
						writer.Close();
					}
					else
					{
						writer = new StreamWriter(path, true);
						writer.WriteLine("Finished operation: Tap; It lasts:" + Time_interaction + "Seconds");
						writer.Close();
					}
                    
                }
                finger_state_last_frame = is_in_interaction;
                Time_interaction = 0f;
            }
        }
        else
        {

            Time_interaction += Time.deltaTime;
            if (!GameObject.FindWithTag("Finger"))
            {
                Instantiate(finger, new Vector3(0, 0, 0), Quaternion.identity);
                Instantiate(finger_big, new Vector3(0, 0, 0), Quaternion.identity); //TODO: change for small inverse 
                if (scene_id < 3)
                {
                    finger_big.GetComponent<finger_position_tracking>().inv = true;
                }
                if (scene_id == 2|| scene_id == 3)
                {
                    finger.GetComponent<finger_position_tracking>().inv = true;
                }
            }
         
            is_in_interaction = true;
            if (is_in_interaction != finger_state_last_frame)
            {
                float current_time = GameObject.Find("Interaction Handler").GetComponent<RotationInteraction>().Time_Stamp();
                writer = new StreamWriter(path, true);
                writer.WriteLine("Current Time:" + current_time + "; Event: Hand in.");
                writer.Close();
                finger_state_last_frame = is_in_interaction;
				angle_last = small.transform.rotation;
                  
            }

        }

    }
}
