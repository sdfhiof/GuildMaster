using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(BoxCollider))]

public class WarUnitSet : MonoBehaviour
{

    

    Rigidbody rb;
    BoxCollider col;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void Init()
    {
        rb = GetComponent<Rigidbody>();
        col = GetComponent<BoxCollider>();

        col.size = new Vector3(1f, 1f, 3f);
    }
}