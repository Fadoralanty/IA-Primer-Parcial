using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkState<T> : State<T>
{
    IsabelleAnimations _isabelleAnim;
    Isabelle _isabelle;
    T _inputToIdle;
    T _inputToDeath;
    FSM<T> _fsm;
    public WalkState(IsabelleAnimations isabelleAnimations, FSM<T> fsm, Isabelle isabelle, T input, T inputToDead)
    {
        _isabelleAnim = isabelleAnimations;
        _isabelle = isabelle;
        _fsm = fsm;
        _inputToIdle = input;
        _inputToDeath = inputToDead;
    }
    public override void Init()
    {
        _isabelleAnim.Walking(true);
    }
    public override void Execute()
    {
        if (_isabelle.IsDead)
        {
            _fsm.Transition(_inputToDeath);
        }

        //player input
        float hor = Input.GetAxis("Horizontal");
        float fwd = Input.GetAxis("Vertical");
        Vector3 dir = new Vector3(hor, 0, fwd);

        if (dir != Vector3.zero)
            _isabelle.Movement(dir.normalized);
        else
            _fsm.Transition(_inputToIdle);
    }
}
