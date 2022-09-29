using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootState<T> : State<T>
{
    private Enemy _enemy;
    private EnemyController _enemyController;
    private EnemyAnimations _enemyAnim;
    private float _shootCoolDown;
    private INode _root;
    public ShootState(EnemyController enemyController, Enemy enemy, EnemyAnimations enemyAnim, INode root)
    {
        _enemy = enemy;
        _enemyAnim = enemyAnim;
        _enemyController = enemyController;
        _root = root;
    }

    public override void Init()
    {
        //Debug.Log("shoot");
        _enemyAnim.Running(true);
        _enemy.speed = _enemy.RunSpeed;
        _shootCoolDown = 0.5f;
    }
    public override void Exit()
    {
        _shootCoolDown = _enemy.ShootRate;
    }
    public override void Execute()
    {
        Vector3 diff = _enemy.target.position - _enemy.transform.position;
        float distance = diff.magnitude;
        _enemy.LookDir(diff.normalized);

        if (distance < _enemy.ShootRange)
        {
            if (_shootCoolDown <= 0)
            {
                _enemyAnim.FiringRifle(true);
                _enemy.Shoot();
                _shootCoolDown = _enemy.ShootRate;
                
                _root.Execute();    //llamo al root para ver is el player is dead y transicione
            }
            _shootCoolDown -= Time.deltaTime;
        }
        else
        {
            _shootCoolDown -= Time.deltaTime;
            _enemyAnim.FiringRifle(false);
            _enemyController.SteeringUpdate();
        }
    }
}
