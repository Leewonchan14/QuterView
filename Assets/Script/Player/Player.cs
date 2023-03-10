using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
  public GameObject[] weapons;
  public bool[] hasWeapon;
  public float speed;
  public float walkSpeed;
  public float jumpPower;

  float hAxis;
  float vAxis;
  bool wDown;
  bool jDown;
  bool iDown;
  bool sDown1;
  bool sDown2;
  bool sDown3;

  Vector3 moveVec;
  Vector3 dodgeVec;
  bool isJump;
  bool isDodge;
  bool isSwap;
  Animator anim;
  Rigidbody rigid;
  GameObject nearObject;
  GameObject equipWeapon;
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
    //Player InterAction
    Interaction();
    //Player Swap
    Swap();
  }

  ///<summary>
  ///입력받는함수
  ///</summary>
  void GetInput()
  {
    hAxis = Input.GetAxisRaw("Horizontal");
    vAxis = Input.GetAxisRaw("Vertical");
    wDown = Input.GetButton("Walk");
    jDown = Input.GetButtonDown("Jump");
    iDown = Input.GetButtonDown("Interaction");
    sDown1 = Input.GetButtonDown("Swap1");
    sDown2 = Input.GetButtonDown("Swap2");
    sDown3 = Input.GetButtonDown("Swap3");
  }

  ///<summary>
  ///플레이어 이동, 회전, 애니메이션 인자 수정
  ///</summary>
  /// <param name="인자이름"></param>
  void Move()
  {
    moveVec = new Vector3(hAxis, 0, vAxis).normalized;
    //회피중 움직임 제어
    if (isDodge) moveVec = dodgeVec;

    //무기교체중 움직임 제어
    if (isSwap) moveVec = Vector3.zero;

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
    if (jDown && !isJump && moveVec == Vector3.zero && !isDodge && !isSwap)
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
    if (jDown && !isJump && moveVec != Vector3.zero && !isDodge && !isSwap)
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
  ///무기 입수 함수
  ///</summary>
  void Interaction()
  {
    //근처에 아이템이 있고, 점프나 회피중이 아닐때
    if (iDown && nearObject != null && !isJump && !isDodge)
    {
      //무기라면
      if (nearObject.tag == "Weapon")
      {
        Item item = nearObject.GetComponent<Item>();
        int weaponIndex = item.value;
        hasWeapon[weaponIndex] = true;

        Destroy(nearObject);
      }
    }
  }
  ///<summary>
  ///무기 스왑
  ///</summary>
  void Swap()
  {
    if (sDown1 && (!hasWeapon[0] || equipWeapon == weapons[0])) return;
    if (sDown2 && (!hasWeapon[1] || equipWeapon == weapons[1])) return;
    if (sDown3 && (!hasWeapon[2] || equipWeapon == weapons[2])) return;

    int weaponIndex = -1;
    if (sDown1) weaponIndex = 0;
    if (sDown2) weaponIndex = 1;
    if (sDown3) weaponIndex = 2;
    //스왑하거나, 점프,회피상태가 아닐때
    if ((sDown1 || sDown2 || sDown3) && !isJump && !isDodge)
    {
      if (equipWeapon != null) equipWeapon.SetActive(false);
      weapons[weaponIndex].SetActive(true);
      equipWeapon = weapons[weaponIndex];

      isSwap = true;
      anim.SetTrigger("doSwap");
      CancelInvoke("SwapOut");
      Invoke("SwapOut", 0.4f);
    }
  }
  ///<summary>
  ///무기교체가 끝나는 함수
  ///</summary>
  void SwapOut()
  {
    isSwap = false;
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
  void OnTriggerStay(Collider other)
  {
    //웨폰을 만났다면
    if (other.CompareTag("Weapon"))
    {
      nearObject = other.gameObject;
      Debug.Log(nearObject.name);
    }
  }
  void OnTriggerExit(Collider other)
  {
    if (other.CompareTag("Weapon"))
    {
      nearObject = null;
    }
  }
}
