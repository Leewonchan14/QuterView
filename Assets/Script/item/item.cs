using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class item : MonoBehaviour
{
  public float rotateSpeed;
  public enum itemType { Ammo, Coin, Grenade, Heart, Weapon };
  public itemType myType;
  public int value;
  private void Update()
  {
    transform.Rotate(Vector3.up * rotateSpeed * Time.deltaTime);
  }
}
