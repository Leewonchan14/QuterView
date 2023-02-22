using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float speed;
    float hAxis;
    float vAxis;
    void Start()
    {
        
    }
    void Update()
    {
        //input Vector
        hAxis = Input.GetAxisRaw("Horizontal");
        vAxis = Input.GetAxisRaw("Vertical");

        //Player Move
        Vector3 moveVec = new Vector3(hAxis,0,vAxis).normalized;
        transform.position += moveVec * speed*Time.deltaTime;
    }
}
