using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolState<T> : State<T> 
{
    private Enemy _enemy;
    private EnemyController _enemyController;
    private EnemyAnimations _enemyAnim;
    private Transform[] _patrolSpots;
    private Roulette<Transform> _roulette = new Roulette<Transform>();
    private Dictionary<Transform, int> _items;
    private float _waitTime;
    private float _wait;
    private int _index;
    private int _lastIndex = 0;
    private bool _isGoingBackwards;
    private INode _root;
    private Transform _idleRestSpot;
    public PatrolState(Enemy enemy, EnemyController enemyController, EnemyAnimations enemyAnim, Transform[] patrolSpots, float waitTime, INode root)
    {
        _enemyController = enemyController;
        _enemy = enemy;
        _enemyAnim = enemyAnim;
        _patrolSpots = patrolSpots;
        _waitTime = waitTime;
        _root = root;
    }
    public override void Init()
    {
        Vector3 dir = _patrolSpots[_index].position - _enemy.transform.position;
        _enemy.LookDir(dir.normalized);
        _items = new Dictionary<Transform, int>();
        foreach (var t in _patrolSpots)
        {
            _items[t] = 25;
        }
        //Debug.Log("patrol");
        _enemyAnim.Running(false);
        _enemyAnim.FiringRifle(false);
        _enemy.speed = _enemy.walkSpeed;
        _wait = _waitTime;
        _index = _lastIndex;
        _isGoingBackwards = false;
        _idleRestSpot = _roulette.Run(_items);
        //Debug.Log(_idleRestSpot);
    }
    public override void Execute()
    {
        if (_enemy.LineOfSight(_enemy.target) && !_enemy.isabelle.IsDead)
        {
            _root.Execute();
        }   

        if (Vector3.Distance(_patrolSpots[_index].position, _enemy.transform.position) < 1f) //si llego al waypoint
        {
            if (_wait <= 0) //termino de esperar
            {
                if(_index == _patrolSpots.Length-1) //si llego al ultimo waypoint que recorra usando- 
                    //-este flag el array al reves hasta el primer waypoint
                {
                    _isGoingBackwards = true;
                }
                if (_index == 0)
                {
                    _isGoingBackwards = false;
                }
                if (_isGoingBackwards) 
                { 
                    _index--;
                }
                else
                {
                    _index++;
                }
                
                _wait = _waitTime;
                
                if(Vector3.Distance(_idleRestSpot.position, _enemy.transform.position) < 1f)
                    _enemyController._isPatrolFinished = true;
                    
                if (_enemyController._isPatrolFinished) _root.Execute();
                
            }
            else //si estoy esperando
            {   
                _enemyAnim.Walking(false);
                _wait -= Time.deltaTime;
                if (_isGoingBackwards)
                {
                    if (_index == 0)
                    {
                        LookAtTarget(!_isGoingBackwards);
                    }
                    else
                    {
                        LookAtTarget(_isGoingBackwards);
                    }
                   
                }
                else
                {
                    if (_index == _patrolSpots.Length - 1)
                    {
                        LookAtTarget(!_isGoingBackwards);
                    }
                    else
                    {
                        LookAtTarget(_isGoingBackwards);
                    }    
                }

            }
            
        }
        else // si no llego al waypoint que se mueva hacia el waypoint numero index
        {
            if (_enemyAnim.IsWalking == false) { _enemyAnim.Walking(true); }
            Vector3 dir = _patrolSpots[_index].position - _enemy.transform.position;
            //Debug.DrawLine(_enemy.transform.position, _enemy.transform.position+dir, Color.red);
            _enemy.Move(dir.normalized);
            _enemy.LookDir(dir.normalized);
        }
    }
    public override void Exit()
    {
        _lastIndex = _index;
    }

    private void LookAtTarget(bool isGoingBackwards)
    {
        if (isGoingBackwards)
        {
            Vector3 dir = _patrolSpots[_index - 1].position - _enemy.transform.position;
            _enemy.LookDir(dir.normalized); 
        }
        else
        {
            Vector3 dir = _patrolSpots[_index + 1].position - _enemy.transform.position;
            _enemy.LookDir(dir.normalized);  
        }
    }

    // private Vector3 LookAtNextWaypoint()
    // {
    //     
    // }
}
