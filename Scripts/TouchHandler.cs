using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TouchHandler : MonoBehaviour
{
    
    private Renderer small;
    private Renderer big;
    public GameObject hand;
    public GameObject small_mesh;

    void Start()
    {
        small = GameObject.Find("Small Sphere").GetComponent<Renderer>();
        big = GameObject.Find("Big Sphere").GetComponent<Renderer>();
    }

    void Update()
    {
        Debug.Log(small);
        if (Vector3.Distance(small_mesh.transform.position, hand.transform.position) > 0.5)
        {
            Debug.Log("Too far apart");
            return;
        }
        Debug.Log("Close enough");
        
        RaycastHit hit;
        if (!Physics.Raycast(hand.transform.position, hand.transform.position-small_mesh.transform.position, out hit))
        {
            Debug.DrawLine(hand.transform.position, small_mesh.transform.position, Color.cyan);
            Debug.Log("No raycast hit");
            small.material.SetFloat("_Radius", 0.0f);
            big.material.SetFloat("_Radius", 0.0f);
            return;
        }
        Debug.DrawLine(hand.transform.position, hit.point, Color.red);
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