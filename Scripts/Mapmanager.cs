using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Newtonsoft.Json.Linq;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using System;

//draw country borders from JSON data
public class Mapmanager : MonoBehaviour {

    private string filename = "Imports/geodata_cultural_medium.json";//source data
    private string jsonstring;
    public float radious;
    public GameObject center;
    public GameObject empty;
    public List<CountryInfo> info = new List<CountryInfo>();
    public bool loaded = false;
    private LineRenderer lineRenderer;

    // Use this for initialization
    void Start() {
        CreatMap();
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    void CreatMap()
    {
        //read data
        jsonstring = File.ReadAllText(Application.dataPath + "/" + filename);
        JObject obj = JObject.Parse(jsonstring);


        for (int i = 0; i < 240; i++)
        {
            //get and clean data
            string countryname;
            string coordinate;
            string before;
            string polygon;
            countryname = obj["features"][i]["properties"]["NAME"].ToString();
            //Debug.Log (countryname);
            polygon = obj["features"][i]["geometry"]["type"].ToString();
            before = obj["features"][i]["geometry"]["coordinates"].ToString();


            char[] buffer = new char[before.Length];
            int index = 0;
            foreach (char c in before)
            {
                List<char>  list = new List<char>{ '1', '2', '3', '4', '5', '6','7','8','9','0','-','[',']',',','.' };
                if (list.Contains(c))
                {
                    buffer[index] = c;
                    index++;
                }
            }

            coordinate = new string(buffer, 0, index);

            if (polygon == "Polygon")//there are two kinds of polygon type in source data:Polygon and MultiPolygon
            {
                //transform and save data
                string[] coorset = coordinate.Split(new char[] { ',' });
                Vector2[] V2 = GetCoordinateData(coorset);
                Vector3[] V3 = TransformCoordinateData(V2);
                LineRender(V3, countryname);
                //Add info to list
                CountryInfo thisInfo = new CountryInfo();
                thisInfo.num = i;
                thisInfo.countryname = countryname;
                thisInfo.SphCoor = V2;
                thisInfo.KartCoor = V3;
                info.Add(thisInfo);


            }
            else
            {

                String[] polygons= coordinate.Split(new[] { "]]],[[[" }, StringSplitOptions.None);

                for (int poly = 0; poly < polygons.Length; poly++)

                {
                    string[] coorset = polygons[poly].Split(new char[] { ',' });
                    Vector2[] V2Part = GetCoordinateData(coorset);
                    Vector3[] V3Part = TransformCoordinateData(V2Part);
                    LineRender(V3Part, countryname);
                    CountryInfo thisInfo = new CountryInfo();
                    thisInfo.num = i;
                    thisInfo.countryname = countryname;
                    thisInfo.SphCoor = V2Part;
                    thisInfo.KartCoor = V3Part;
                    info.Add(thisInfo);
                }
            }




            

            

        }
        loaded = true;
    }


    

            //for Each country get and caculate the coordinate data
            public Vector2[] GetCoordinateData(String[] coordinates)
            {
                Vector2[] coordinatesV2 = new Vector2[coordinates.Length / 2];
                for (int m = 0; m < coordinates.Length; m++)
                {


                    //delete []symble
                    String str = coordinates[m];
                    char[] buffer = new char[str.Length];
                    int index = 0;
                    foreach (char c in str)
                    {
                        if (c != '[' && c != ']')
                        {
                            buffer[index] = c;
                            index++;
                        }
                    }

                    //string to float and save as V2 points
                    str = new string(buffer, 0, index);
                    float flo = float.Parse(str);
                    int num = (int)Math.Floor((double)m / 2);
                    if (m % 2 == 0)
                    {
                        coordinatesV2[num].x = flo;
                        //Debug.Log ("Vectr2__" + num+"__" + coorV2 [num].x+".X was saved");
                    }
                    else
                    {
                        coordinatesV2[num].y = flo;
                        //Debug.Log ("Vectr2__" + num +"__"+ coorV2 [num].y+".Y was saved");
                    }
                }
                return coordinatesV2;
            }


            //sohere to XYZ
            public Vector3[] TransformCoordinateData(Vector2[] SphericalCoordinate) {

                Vector3[] coordinatesV3 = new Vector3[SphericalCoordinate.Length];

                for (int c = 0; c < SphericalCoordinate.Length; c++)
                {
                    float longitude = SphericalCoordinate[c].x * Mathf.PI / 180;//fita
                    float latitude = (90.0f - SphericalCoordinate[c].y) * Mathf.PI / 180;//theta
                    float X = (float)radious * (float)Math.Sin((float)latitude) * (float)Math.Cos((float)longitude);
                    float Y = (float)radious * (float)Math.Cos((float)latitude);
                    float Z = (float)radious * (float)Math.Sin((float)latitude) * (float)Math.Sin((float)longitude);
                    coordinatesV3[c] = new Vector3(X, Y, Z);
                }
                return coordinatesV3;
            }

            public void LineRender(Vector3[] coorV3, String countryname) {

                if (coorV3.Length > 150)
                {

                    GameObject instant;// = new GameObject();



                    instant = Instantiate(empty, new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
                    instant.transform.parent = gameObject.transform;
                    instant.name = countryname;

                    lineRenderer = (LineRenderer)instant.GetComponent("LineRenderer");
               //     lineRenderer.material = new Material(Shader.Find("Particles/Additive"));
                 //   lineRenderer.SetColors(Color.black, Color.black);
                    lineRenderer.useWorldSpace = false;
                    lineRenderer.positionCount = coorV3.Length;
                    lineRenderer.SetPositions(coorV3);


                }

            }


        } 

//data structure 
public class CountryInfo{
	public int num;
	public string countryname;
	public Vector2[] SphCoor; //spherical coordinates
	public Vector3[] KartCoor;//kartesian coordinates
    public GameObject[] points;//currentposition

}





