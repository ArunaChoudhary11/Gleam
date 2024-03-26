using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class EnemyAI : MonoBehaviour
{
    private Vector2 Origin;
    public enum EnemyType { SOLDIER, ICE_ILLUMINOS, ICE_CLEAVERS, DARK_ROOT, BEEE }
    [SerializeField] private EnemyType enemyType;
    private float speed;
    [SerializeField] private Transform[] patrolPoints;
    private int patrolPointIndex;
    private float phaseTimer;
    private float stopTime;

    public enum States { IDLE, PATROL, SUSPISIOUS, CHASE, ATTACK, DEAD, NONE }

    [System.Serializable]
    public struct EnemyStates
    {
        public string name;
        public string[] stateClip;
    }

    private int index = 0;
    private States state;
    public List<EnemyStates> subStates = new List<EnemyStates>();

    private int currentStateIndex;
    private float subStateChangeTimer;
    private float subStateChangeThreshold;

    [SerializeField] private string currentAnimationClip;
    [SerializeField] private AnimationClip animationClip;

    private float animationLength;
    private int animationPlayCountLimit;
    private int currentAnimationPlayCount;

    private AIDestinationSetter destinationSetter;
    private AIPath aIPath;
    private IEnumerator StateChangeCoroutine;

    [SerializeField] private float suspisiousRange;
    [SerializeField] private float chaseRange;
    [SerializeField] private float attackRange;

    private float distance;
    private float OriginDistance;
    private HealthSystem healthSystem;

    void Start()
    {
        aIPath = GetComponent<AIPath>();
        destinationSetter = GetComponent<AIDestinationSetter>();
        healthSystem = GetComponent<HealthSystem>();
        Origin = transform.position;
        ChangeState(States.PATROL);
    }
    void Update()
    {
        OriginDistance = Vector2.Distance(PlayerManager.Instance.transform.position, Origin);
        distance = Vector2.Distance(PlayerManager.Instance.transform.position, transform.position);
        CurrentSubState();
        aIPath.maxSpeed = speed;
    }
    private void ChangeState(States newState)
    {
        state = newState;
        CurrentState();
    }
    private void CurrentState()
    {    
        for(int i = 0; i < subStates.Count; i++)
        {
            if(state.ToString() == subStates[i].name.ToUpper())
            {
                index = 0;
                subStateChangeTimer = 0;
                subStateChangeThreshold = 0;
                currentStateIndex = i;
                return;
            }
        }
    }
    private void CurrentSubState()
    {
        if(healthSystem.Health() <= 0) ChangeState(States.DEAD);

        switch(state)
        {
            case States.IDLE: Idle(); break;

            case States.PATROL: Movement(3, 0.7f, States.PATROL); break;

            case States.SUSPISIOUS: Movement(5, 1.5f, States.SUSPISIOUS); break;

            case States.CHASE: Chase(); break;

            case States.ATTACK: Attack(); break;

            case States.DEAD: Dead(); return;

            case States.NONE: return;

            default: Debug.Log("Does not exists"); break;
        }

        if(distance <= chaseRange && distance > attackRange) ChangeState(States.CHASE);

        if(distance <= attackRange)
        {
            return;
        }

        if(OriginDistance > suspisiousRange) ChangeState(States.PATROL);

       // SetAnimationClip();
    }
    private void SetAnimationClip()
    {
        subStateChangeTimer += Time.deltaTime;

        if(subStateChangeThreshold == 0) subStateChangeThreshold = animationClip != null ? animationClip.length * Random.Range(1, 3) : Random.Range(1, 3);
        
        if(subStateChangeTimer >= subStateChangeThreshold)
        {
            index = Random.Range(0, subStates[currentStateIndex].stateClip.Length);

            subStateChangeTimer = 0;

            subStateChangeThreshold = animationClip != null ? animationClip.length * Random.Range(1, 3) : Random.Range(1, 3);
        }

        currentAnimationClip = subStates[currentStateIndex].stateClip[index];
        animationLength = animationClip != null ? animationClip.length : Random.Range(1, 3);
    }
    private void Idle()
    {
        /*
            Soldier -
                1. will patrol b/w points.
                2. will stay at points longer and speed will increase.
                3. will chase the player.
                4. will attack when player nearby.
                5. will dissolve and destroy in dead state.

            Dark Root - 
                1. will be a black slime like thing
                2. will have a boiling visual in suspisius phase.
                3. will activate in the chasing phase.
                4. will attack when player nearby.
                5. will get back to the black slime like thing in dead state.

            Ice Illuminos -
                1. will patrol b/w points.
                2. will stay at points longer and speed will increase.
                3. will chase the player.
                4. will attack when player nearby.
                5. will dissolve and destroy in dead state.

            Ice Cleavers -
                1. will patrol b/w points.
                2. will stay at points longer and speed will increase.
                3. will chase the player.
                4. will attack when player nearby.
                5. will dissolve and destroy in dead state.
        */

        phaseTimer = 0;
        destinationSetter.target = transform;

        // move on given checkpoints - stay at a point.
        // player in range - change to chase
    }
    private void Movement(float _speed, float restTime, States _enemystate)
    {
        phaseTimer = 0;
        destinationSetter.target = patrolPoints[patrolPointIndex];
        speed = _speed;
        
        if(distance <= suspisiousRange) ChangeState(States.SUSPISIOUS);
        else ChangeState(States.PATROL);

        if(Vector2.Distance(destinationSetter.target.position, transform.position) <= aIPath.endReachedDistance)
        {
            patrolPointIndex++;

            if(patrolPointIndex > patrolPoints.Length - 1)
            {
                patrolPointIndex = 0;
            }

            ChangeState(States.IDLE);

            StateChangeCoroutine = State(restTime, _enemystate);
            StartCoroutine(StateChangeCoroutine);
            return;
        }

        if(StateChangeCoroutine != null) StopCoroutine(StateChangeCoroutine);
    }
    private IEnumerator State(float duration, States _state)
    {
        yield return new WaitForSeconds(duration);
        ChangeState(_state);
    }
    private void Chase()
    {
        speed = 5;

        if(StateChangeCoroutine != null) StopCoroutine(StateChangeCoroutine);

        destinationSetter.target = PlayerManager.Instance.transform;

        if(distance <= attackRange) ChangeState(States.ATTACK);
        else
        {
            if(StateChangeCoroutine != null) StopCoroutine(StateChangeCoroutine);
        }

        if(distance > chaseRange)
        {
            phaseTimer += Time.deltaTime;

            if(phaseTimer >= 2) ChangeState(States.IDLE);
            return;
        }

        phaseTimer = 0;
        // move towards the player till the given range.
    }
    private void Attack()
    {
        destinationSetter.target = transform;

        if(stopTime == 0) stopTime = animationClip != null ? animationClip.length : 2f;
        
        if(animationPlayCountLimit == 0) animationPlayCountLimit = Random.Range(1, 5);

        if(currentAnimationPlayCount < animationPlayCountLimit)
        {
            phaseTimer += Time.deltaTime;

            if(phaseTimer >= stopTime)
            {
                phaseTimer = 0;
                currentAnimationPlayCount++;
            }

            return;
        }

        phaseTimer = 0;
        currentAnimationPlayCount = 0;

        stopTime = 0;
        animationPlayCountLimit = 0;

        ChangeState(States.IDLE);

        StateChangeCoroutine = State(1, States.ATTACK);

        StartCoroutine(StateChangeCoroutine);
    }
    private void AttackingBehaviour()
    {
        if(TryGetComponent<PlayerManager>(out var player))
        {
            player.TakeDamage(5);
        }
    }
    private void Dead()
    {
        aIPath.canMove = false;

        stopTime = animationClip != null ? animationClip.length : 1f;

        Destroy(transform.parent.gameObject, stopTime);

        ChangeState(States.NONE);
    }
    void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(Origin, suspisiousRange);

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, chaseRange);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}

/*
    Movement
    Speed
    Direction
*/