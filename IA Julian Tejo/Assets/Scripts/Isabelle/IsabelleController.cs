using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsabelleController : MonoBehaviour
{
    Isabelle _isabelle;
    IsabelleAnimations _isabelleAnimations;
    FSM<States> _fsm;
    Rigidbody _rb;
    CapsuleCollider _capsuleCollider;
    public enum States 
    {
        Idle,
        Walk,
        Death
    }
    private void Awake()
    {
        Cursor.visible = false; // escondo el cursor on play
        _rb = GetComponent<Rigidbody>();
        _capsuleCollider = GetComponent<CapsuleCollider>();
        _isabelle = GetComponent<Isabelle>();
        _isabelleAnimations = GetComponent<IsabelleAnimations>();
        InitializedFSM();
    }
    void InitializedFSM()
    {
        _fsm = new FSM<States>();
        IdleState<States> idle = new IdleState<States>(_isabelleAnimations, _fsm, _isabelle, States.Walk, States.Death);
        WalkState<States> walk = new WalkState<States>(_isabelleAnimations,_fsm,_isabelle,States.Idle, States.Death);
        DeathState<States> death = new DeathState<States>(_isabelleAnimations, _fsm, _isabelle, _rb, _capsuleCollider);

        idle.AddTransition(States.Walk, walk);
        idle.AddTransition(States.Death, death);

        walk.AddTransition(States.Idle, idle);
        walk.AddTransition(States.Death, death);

        _fsm.SetInit(walk);
    }
    private void Update()
    {
        _fsm.OnUpdate();
    }
}
