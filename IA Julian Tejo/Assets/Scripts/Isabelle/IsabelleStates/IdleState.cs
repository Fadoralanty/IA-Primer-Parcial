using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState<T> : State<T>
{
    private IsabelleAnimations _isabelleAnim;
    private Isabelle _isabelle;
    private T _inputToWalk;
    private T _inputToDeath;
    private FSM<T> _fsm;
    public IdleState(IsabelleAnimations isabelleAnimations, FSM<T> fsm, Isabelle isabelle, T input, T inputToDead)
    {
        _fsm = fsm;
        _isabelleAnim = isabelleAnimations;
        _isabelle = isabelle;
        _inputToWalk = input;
        _inputToDeath = inputToDead;
    }
    public override void Init()
    {
        _isabelleAnim.Walking(false);
    }
    public override void Execute()
    {
        if (_isabelle.IsDead)
        {
            _fsm.Transition(_inputToDeath);
        }

        //player input
        if (Input.GetAxis("Vertical") !=0 || Input.GetAxis("Horizontal") != 0)
            _fsm.Transition(_inputToWalk);
    }
}
