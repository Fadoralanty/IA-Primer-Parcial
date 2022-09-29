using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimations : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    public bool IsRunning=>_isRunning;   
    private bool _isRunning;
    public bool IsWalking=> _isWalking;
    private bool _isWalking;
    public bool IsShooting => _isShooting;
    private bool _isShooting;

    public void Running(bool isRunning)
    {
        _isRunning = isRunning;
        _animator.SetBool("IsRunning", _isRunning);
    }

    public void Walking(bool isWalking)
    {
        _isWalking = isWalking;
        _animator.SetBool("IsWalking", _isWalking);
    }
    public void FiringRifle(bool isShooting)
    {
        _isShooting = isShooting;
        _animator.SetBool("FireRifle", _isShooting);
    }

}
