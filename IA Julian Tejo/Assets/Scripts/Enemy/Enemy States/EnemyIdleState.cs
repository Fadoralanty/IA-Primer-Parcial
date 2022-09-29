using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyIdleState<T> : State<T>
{
    private Enemy _enemy;
    private EnemyController _enemyController;
    private EnemyAnimations _enemyAnim;
    private INode _root;
    private float _wait;
    private float _waitTime = 7f;
    public EnemyIdleState(EnemyController enemyController, EnemyAnimations enemyAnim, Enemy enemy, INode root, float waitTime)
    {
        _waitTime = waitTime;
        _enemyController = enemyController;
        _enemyAnim = enemyAnim;
        _enemy = enemy;
        _root = root;
    }
    //TODO add resting Animation
    public override void Init()
    {
        _wait = _waitTime;
        //Debug.Log("idle");
        _enemyAnim.Running(false);
        _enemyAnim.Walking(false);
        _enemyAnim.FiringRifle(false);
    }
    public override void Execute()
    {
        if (_wait <= 0)
        {
            _enemyController._isPatrolFinished = false;
            _root.Execute();
        }
        _wait -= Time.deltaTime;
        if (_enemy.LineOfSight(_enemy.target) && !_enemy.isabelle.IsDead)
        {
            //if (_root == null) Debug.Log("root null");
            _root.Execute();
        }
    }
}
