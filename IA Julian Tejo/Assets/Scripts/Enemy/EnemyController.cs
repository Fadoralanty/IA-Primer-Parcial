using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour 
    //TODO agregar roulette
{
    [SerializeField] private Enemy _enemy;
    [SerializeField] private EnemyAnimations _enemyAnim;
    [SerializeField] private float _waitPatrol;
    [SerializeField] private float _waitIdle;
    public bool _isPatrolFinished = false;
    public float _timePrediction;
    public Transform[] _waypoints;
    public float _obsRadius;
    public float _obsAngle;
    public float avoidanceWeight = 1;
    public float steeringWeight = 1;
    public LayerMask obsMask;
    Entity _model;
    ISteering _steering;
    ISteering _avoidance;
    private FSM<States> _fsm;
    private INode _root;
    PatrolState<States> _patrol;
    EnemyIdleState<States> _idle;
    ShootState<States> _shoot;
    public enum States
    {
        Idle,
        Shoot,
        Patrol
    }
    private void Awake()
    {
        _isPatrolFinished = false;
        _model = GetComponent<Entity>();
        InitializedSteering();
    }
    void InitializedSteering()
    {
        var seek = new Seek(transform, _enemy.target);
        //var flee = new Flee(transform, target.transform);
        //var pursuit = new Pursuit(transform, target.transform, target, timePrediction);
        //var evade = new Evade(transform, target.transform, target, timePrediction);
        var avoidance = new ObstacleAvoidance(transform, obsMask, _obsRadius, _obsAngle);
        _avoidance = avoidance;
        _steering = seek;
    }
    private void Start()
    {
        TreeInitialized();
        FSMinitialized();
    }
    void TreeInitialized()
    {
        INode idle = new ActionNode(() => _fsm.Transition(States.Idle));
        INode shoot = new ActionNode(() => _fsm.Transition(States.Shoot));
        INode patrol = new ActionNode(() => _fsm.Transition(States.Patrol));

        INode IsPlayerDetected = new QuestionNode(isPlayerDetected , shoot, patrol);
        INode IsPatrolFinished = new QuestionNode(() => _isPatrolFinished, idle, IsPlayerDetected);
        INode IsShoot = new QuestionNode(IsShooting, shoot, patrol);
        INode IsPatrol = new QuestionNode(IsPatrolling, IsPatrolFinished, IsShoot);
        INode IsIdle = new QuestionNode(IsOnIdle, IsPlayerDetected, IsPatrol);
        INode IsPlayerDead = new QuestionNode(() => _enemy.isabelle.IsDead, patrol, IsIdle); 

        _root = IsPlayerDead; 
    }
    void FSMinitialized() 
    {
        _fsm = new FSM<States>();

        _idle = new EnemyIdleState<States>(this, _enemyAnim, _enemy, _root, _waitIdle);
        _shoot = new ShootState<States>(this, _enemy,_enemyAnim, _root);
        _patrol = new PatrolState<States>(_enemy, this, _enemyAnim, _waypoints, _waitPatrol, _root);

        _idle.AddTransition(States.Shoot, _shoot);
        _idle.AddTransition(States.Patrol, _patrol);

        _patrol.AddTransition(States.Shoot, _shoot);
        _patrol.AddTransition(States.Idle, _idle);

        _shoot.AddTransition(States.Patrol, _patrol);
        //  shoot.AddTransition(States.Idle, idle);

        _fsm.SetInit(_patrol);
    }
    private void Update()
    {
        _fsm.OnUpdate();
    }
    public void SteeringUpdate()
    {
        var dir = (_avoidance.GetDir() * avoidanceWeight + _steering.GetDir() * steeringWeight).normalized;
        //Debug.Log(dir);
        _model.LookDir(dir);
        _model.Move(transform.forward);
    }
    public void SetNewSteering(ISteering newSteering)
    {
        _steering = newSteering;
    }

    private bool IsPatrolling()
    {
        return _fsm.GetCurrentState == _patrol;
    }

    private bool IsOnIdle()
    {
        return _fsm.GetCurrentState == _idle;
    }

    private bool IsShooting()
    {
        return _fsm.GetCurrentState == _shoot;
    }

    private bool isPlayerDetected()
    {
        return _enemy.LineOfSight(_enemy.target) && !_enemy.isabelle.IsDead;
    }
}
