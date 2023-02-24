using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float speed;
    public float walkSpeed;
    public float jumpPower;

    float hAxis;
    float vAxis;
    bool wDown;
    bool jDown;

    Vector3 moveVec;
    bool isJump;

    Animator anim;
    Rigidbody rigid;
    private void Awake() {
        anim = GetComponentInChildren<Animator>();
        rigid = GetComponent<Rigidbody>();
    }
    void Start()
    {
        
    }
    void Update()
    {
        //KeyBord Input
        GetInput();
        //Player Move
        Move();
        //Player Turn
        Turn();
        //Player Jump
        Jump();
    }

    ///<summary>
    /// hAxis, vAxis, wDown, jDonw
    ///을 입력받는함수
    ///</summary>
    void GetInput(){
        hAxis = Input.GetAxisRaw("Horizontal");
        vAxis = Input.GetAxisRaw("Vertical");
        wDown = Input.GetButton("Walk");
        jDown = Input.GetButtonDown("Jump");
    }
    
    ///<summary>
    ///플레이어 이동, 회전, 애니메이션 인자 수정
    ///</summary>
    /// <param name="인자이름"></param>
    void Move(){
        moveVec = new Vector3(hAxis,0,vAxis).normalized;

        anim.SetBool("isRun",moveVec != Vector3.zero);
        anim.SetBool("isWalk",wDown);
    }
    
    ///<summary>
    ///Player Turn
    ///</summary>
    void Turn(){
        transform.position += moveVec * speed * (wDown?walkSpeed:1f) * Time.deltaTime;
        //Player Rotation
        transform.LookAt(transform.position + moveVec);
    }
    
    ///<summary>
    ///AddForce로 점프
    ///</summary>
    /// <param name="인자이름"></param>
    void Jump(){
        //점프키를 누르고 점프상태가 아니라면
        if(jDown&&!isJump){
            //점프
            rigid.AddForce(Vector3.up*jumpPower,ForceMode.Impulse);
            //애니메이션 설정
            anim.SetBool("isJump",true);
            anim.SetTrigger("doJump");
            //점프상태로 변경
            isJump = true;
        }
    }

    ///<summary>
    /// 1. isJump확인
    /// 
    ///</summary>
    void OnCollisionEnter(Collision other) {
        if(other.gameObject.CompareTag("Floor")){
            anim.SetBool("isJump",false);
            isJump = false;
        }
    }
}
