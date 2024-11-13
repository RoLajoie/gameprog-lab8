using UnityEngine;
using UnityEngine.AI;

public class AIController : MonoBehaviour
{
    public StateMachine StateMachine { get; private set; }
    public NavMeshAgent Agent { get; private set; }
    public Animator Animator { get; private set; } // Use a public property to access Animator
    public Transform[] Waypoints;
    public Transform Player;
    public float SightRange = 10f;
    public float AttackRange = 2f;
    public LayerMask PlayerLayer;
    public StateType currentState;

    void Start()
    {
        Agent = GetComponent<NavMeshAgent>();
        Animator = GetComponent<Animator>(); // Initialize Animator here

        StateMachine = new StateMachine();
        StateMachine.AddState(new IdleState(this));
        StateMachine.AddState(new PatrolState(this));
        StateMachine.AddState(new ChaseState(this));
        StateMachine.AddState(new AttackState(this));

        StateMachine.TransitionToState(StateType.Idle);
    }

    void Update()
    {
        StateMachine.Update();
        currentState = StateMachine.GetCurrentStateType();

        float distanceToPlayer = Vector3.Distance(transform.position, Player.position);

        if (distanceToPlayer <= AttackRange)
        {
            StateMachine.TransitionToState(StateType.Attack);
        }
        else if (distanceToPlayer <= SightRange)
        {
            StateMachine.TransitionToState(StateType.Chase);
        }
        else if (currentState == StateType.Chase)
        {
            StateMachine.TransitionToState(StateType.Patrol);
        }
    }

    public bool CanSeePlayer()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, Player.position);
        return distanceToPlayer <= SightRange;
    }

    public bool IsPlayerInAttackRange()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, Player.position);
        return distanceToPlayer <= AttackRange;
    }
}
