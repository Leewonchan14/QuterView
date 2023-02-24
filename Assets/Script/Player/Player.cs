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
  Vector3 dodgeVec;
  bool isJump;
  bool isDodge;

  Animator anim;
  Rigidbody rigid;
  private void Awake()
  {
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
    //Player Dodge
    Dodge();
  }

  ///<summary>
  /// hAxis, vAxis, wDown, jDonw
  ///을 입력받는함수
  ///</summary>
  void GetInput()
  {
    hAxis = Input.GetAxisRaw("Horizontal");
    vAxis = Input.GetAxisRaw("Vertical");
    wDown = Input.GetButton("Walk");
    jDown = Input.GetButtonDown("Jump");
  }

  ///<summary>
  ///플레이어 이동, 회전, 애니메이션 인자 수정
  ///</summary>
  /// <param name="인자이름"></param>
  void Move()
  {
    moveVec = new Vector3(hAxis, 0, vAxis).normalized;
    if (isDodge) moveVec = dodgeVec;

    anim.SetBool("isRun", moveVec != Vector3.zero);
    anim.SetBool("isWalk", wDown);
  }

  ///<summary>
  ///Player Turn
  ///</summary>
  void Turn()
  {
    transform.position += moveVec * speed * (wDown ? walkSpeed : 1f) * Time.deltaTime;
    //Player Rotation
    transform.LookAt(transform.position + moveVec);
  }

  ///<summary>
  ///AddForce로 점프
  ///</summary>
  /// <param name="인자이름"></param>
  void Jump()
  {
    //점프키를 누르고 점프상태가 아니고, 가만히있고, 회피상태가 아닐때
    if (jDown && !isJump && moveVec == Vector3.zero && !isDodge)
    {
      //점프
      rigid.AddForce(Vector3.up * jumpPower, ForceMode.Impulse);
      //애니메이션 설정
      anim.SetBool("isJump", true);
      anim.SetTrigger("doJump");
      //점프상태로 변경
      isJump = true;
    }
  }
  ///<summary>
  ///회피 함수
  ///</summary>
  void Dodge()
  {
    //점프키를 누르고 점프상태가 아니고, 방향키를 누르고, 회피상태가 아니고
    if (jDown && !isJump && moveVec != Vector3.zero && !isDodge)
    {
      dodgeVec = moveVec;
      speed *= 2;
      anim.SetTrigger("doDodge");
      isDodge = true;
      Invoke("DodgeOut", 0.5f);
    }
  }
  ///<summary>
  ///회피를 빠져나가는 함수
  ///</summary>
  void DodgeOut()
  {
    speed /= 2;
    isDodge = false;
  }

  ///<summary>
  /// 1. isJump확인
  /// 
  ///</summary>
  void OnCollisionEnter(Collision other)
  {
    //바닥에 닿으면 점프 끝
    if (other.gameObject.CompareTag("Floor"))
    {
      anim.SetBool("isJump", false);
      isJump = false;
    }
  }
}
