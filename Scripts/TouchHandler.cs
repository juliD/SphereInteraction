using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TouchHandler : MonoBehaviour
{
    public Camera cam;
    private Renderer small;
    private Renderer big;

    void Start()
    {
        cam = GetComponent<Camera>();
        small = GameObject.Find("Small Sphere").GetComponent<Renderer>();
        big = GameObject.Find("Big Sphere").GetComponent<Renderer>();
    }

    void Update()
    {
        if (!Input.GetMouseButton(0))
            return;

        RaycastHit hit;
        if (!Physics.Raycast(cam.ScreenPointToRay(Input.mousePosition), out hit,100.0f))
        {
            Debug.Log("No raycast hit");
            small.material.SetFloat("_Radius", 0.0f);
            big.material.SetFloat("_Radius", 0.0f);
            return;
        }
        Renderer rend = hit.transform.GetComponent<Renderer>();
        MeshCollider meshCollider = hit.collider as MeshCollider;
        Debug.Log("x: "+hit.textureCoord.x+" y:"+ hit.textureCoord.y);
        if (rend == null || rend.sharedMaterial == null || rend.sharedMaterial.mainTexture == null || meshCollider == null)
        {
            small.material.SetFloat("_Radius", 0.0f);
            big.material.SetFloat("_Radius", 0.0f);
            return;
        }
        
        small.material.SetFloat("_Radius", 0.05f);
        big.material.SetFloat("_Radius", 0.01f);
        small.material.SetFloat("_PointX", hit.textureCoord.x);
        small.material.SetFloat("_PointY", hit.textureCoord.y);
        big.material.SetFloat("_PointX", hit.textureCoord.x);
        big.material.SetFloat("_PointY", hit.textureCoord.y);


    }
}