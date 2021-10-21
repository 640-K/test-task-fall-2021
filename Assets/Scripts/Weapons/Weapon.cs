using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Entities;

public abstract class Weapon : MonoBehaviour
{
    public Entity owner;
    public uint damage;
    public Vector2 aimDirection;

    public bool isAttack;

    public abstract void Use();
}
