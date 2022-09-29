using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathState<T> : State<T>
{
    IsabelleAnimations _isabelleAnim;
    Isabelle _isabelle;
    FSM<T> _fsm;
    Rigidbody _rb;
    CapsuleCollider _capsuleCollider;
    public DeathState(IsabelleAnimations isabelleAnimations, FSM<T> fsm, Isabelle isabelle, Rigidbody rb, CapsuleCollider capsuleCollider)
    {
        _isabelleAnim = isabelleAnimations;
        _isabelle = isabelle;
        _fsm = fsm;
        _rb = rb;
        _capsuleCollider = capsuleCollider;
    }
    public override void Init()
    {
        _isabelleAnim.Defeat();
        _rb.useGravity = false; // hago esto para que el enemy siga patrullando y no choque con isabelle
        _capsuleCollider.isTrigger = true;
    }
}
