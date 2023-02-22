using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float speed;
    float hAxis;
    float vAxis;
    Vector3 moveVec;
    Animator anim;
    private void Awake() {
        anim = GetComponentInChildren<Animator>();
    }
    void Start()
    {
        
    }
    void Update()
    {
        //input Vector
        hAxis = Input.GetAxisRaw("Horizontal");
        vAxis = Input.GetAxisRaw("Vertical");

        //Player Move
        moveVec = new Vector3(hAxis,0,vAxis).normalized;
        transform.position += moveVec * speed*Time.deltaTime;

        anim.SetBool("isRun",moveVec != Vector3.zero);
    }
}
