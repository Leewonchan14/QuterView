using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float speed;
    public float walkSpeed;
    float hAxis;
    float vAxis;
    bool wDown;
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
        wDown = Input.GetButton("Walk");

        //Player Move
        moveVec = new Vector3(hAxis,0,vAxis).normalized;
        transform.position += moveVec * speed * (wDown?walkSpeed:1f) * Time.deltaTime;

        transform.LookAt(transform.position + moveVec);

        anim.SetBool("isRun",moveVec != Vector3.zero);
        anim.SetBool("isWalk",wDown);
    }
}
