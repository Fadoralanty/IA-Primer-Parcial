using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsabelleAnimations : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    public void Walking(bool isWalking)
    {
        _animator.SetBool("IsWalking", isWalking);
    }
    public void Defeat()
    {
        _animator.SetTrigger("DieTrigger");
    }
}
