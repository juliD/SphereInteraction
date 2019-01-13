using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

//to load the gdp bar chart with heat color
public class gdp_loader : MonoBehaviour {
    List<GDP> all = new List<GDP>();
    float radious;
    float length;
    public float sphere_radius;
    public GameObject instance;
    private Transform target;
    public float scale;
    public Material color_blue;
    private bool is_loaded;

    public bool load() {
        return is_loaded;
    }

    // Use this for initialization
    void Start () {
        //data form (latitude,longitude)
        target = GameObject.Find("Empty").transform;
         TextAsset gdpdata = Resources.Load<TextAsset>("gdpdata"); 
        //TextAsset gdpdata = Resources.Load<TextAsset>("test");
        string[] data = gdpdata.text.Split(new char[] { '\n'});
        for(int i=1; i < data.Length - 1; i++) {
            string[] row = data[i].Split(new char[] { ';'});
            GDP q = new GDP();
            q.Country_Name = row[0];
            float.TryParse(row[1],out q.GDP_Value);
            float.TryParse(row[2], out q.latitude);
            float.TryParse(row[3], out q.longitude);
            all.Add(q);
            is_loaded = false;
        }

        foreach (GDP q in all)
        {
            
            if (q.GDP_Value > 1000000000)//only keep points > 1000000000
            {
                float length = q.GDP_Value / 100000000000000;
                // length is value from 0.186244 to 0
                GameObject ins;
                Vector3 loc = new Vector3();
                loc = countrypoint(q.longitude, q.latitude, length);
                ins = Instantiate(instance, loc, Quaternion.identity) as GameObject;
                ins.transform.parent = gameObject.transform;
                ins.name = q.Country_Name;
                ins.transform.localScale = new Vector3(scale, scale, length*2);
                ins.transform.LookAt(target);
                if (length < 0.0001f) {
                    ins.GetComponent<Renderer>().material = color_blue;//blue

                }
                if (length > 0.0001f && length < 0.05f)//from blue to cyan

                {
                    Renderer rend = ins.GetComponent<Renderer>();
                    rend.material = new Material(Shader.Find("Standard"));
                    float m = (length - 0.0001f) / (0.04665f - 0.0001f);
                 //   float n = 1 - m;
                    rend.material.color = new Color(0, m, 255f);
                    rend.material.EnableKeyword("_EMISSION");
                    rend.material.SetColor("_EmissionColor", new Color(0, m, 255f));
                    //ins.GetComponent<Renderer>().material = four;
                }
                if (length > 0.04665f && length < 0.0932f)//0.25-0.5 from cyan to green
                {
                    Renderer rend = ins.GetComponent<Renderer>();
                    rend.material = new Material(Shader.Find("Standard"));
                    float m = (length - 0.04665f) / (0.0932f - 0.04665f);
                    float n = 1 - m;
                    rend.material.color = new Color(0f, 255, n);
                    rend.material.EnableKeyword("_EMISSION");
                    rend.material.SetColor("_EmissionColor", new Color(0f, 255, n));
                    //ins.GetComponent<Renderer>().material = two;
                }
                if (length > 0.0932f && length < 0.13975f)//0.5-0.75 from green to yellow
                {
                    Renderer rend = ins.GetComponent<Renderer>();
                    rend.material = new Material(Shader.Find("Standard"));
                    float m = (length - 0.0932f) / (0.13975f- 0.0932f);
                  //  float n = 1 - m;
                    rend.material.color = new Color(m, 255f, 0f);
                    rend.material.EnableKeyword("_EMISSION");
                    rend.material.SetColor("_EmissionColor", new Color(m, 255f, 0f));
                    //ins.GetComponent<Renderer>().material = three;
                    // Debug.Log(length + "," + m);
                }
                if (length > 0.13975f)//>0.75 from yellow to red
                {
                    Renderer rend = ins.GetComponent<Renderer>();
                    rend.material = new Material(Shader.Find("Standard"));
                    float m = (length - 0.13975f) / 0.04655f;
                    float n = 1 - m;
                    rend.material.color = new Color(255f,n,0f);
                    rend.material.EnableKeyword("_EMISSION");
                    rend.material.SetColor("_EmissionColor", new Color(255f, n, 0f));
                }
            }
        }
        is_loaded = true;

    }
	
	// Update is called once per frame
	void Update () {
		
	}
    public Vector3 countrypoint(float longi,float lat,float l)
    {
        radious = sphere_radius + l;
        Vector3 location = new Vector3();
        float longitude = longi * Mathf.PI / 180;//fita
        float latitude = (90.0f - lat) * Mathf.PI / 180;//theta
        float X = (float)radious * (float)Math.Sin((float)latitude) * (float)Math.Cos((float)longitude);
        float Y = (float)radious * (float)Math.Cos((float)latitude);
        float Z = (float)radious * (float)Math.Sin((float)latitude) * (float)Math.Sin((float)longitude);
        location = new Vector3(X, Y, Z);
        return location;
    }
}
