using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public enum EnemyType { SOLDIER, ICE_ILLUMINOS, ICE_CLEAVERS, DARK_ROOT, BEEE }
    [SerializeField] private EnemyType enemyType;
    [SerializeField] private float speed;
    [SerializeField] private Transform[] patrolPoints;
    [SerializeField] private float phaseTimer;
    [SerializeField] private float stopTime;
    [SerializeField] private bool canAttack;
    [SerializeField] private bool isAttacking;

    public enum States { IDLE, PATROL, SUSPISIUS, CHASE, ATTACK, DEAD }

    [System.Serializable]
    public struct EnemyStates
    {
        public string name;
        public string[] stateClip;
    }
    private int index = 0;
    public States state;
    public List<EnemyStates> subStates = new List<EnemyStates>();
    private int currentStateIndex;
    private float subStateChangeTimer;
    private float subStateChangeThreshold;
    [SerializeField] private string currentAnimationClip;
    [SerializeField] private AnimationClip animationClip;
    [SerializeField] private float animationLength;
    [SerializeField] private int animationPlayCountLimit;
    [SerializeField] private int currentAnimationPlayCount;

    void Start()
    {
        ChangeState(States.IDLE);
    }
    void Update()
    {
        CurrentSubState();
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
        switch(state)
        {
            case States.IDLE: Idle(); break;

            case States.PATROL: Patrol(); break;

            case States.SUSPISIUS: Suspisius(); break;

            case States.CHASE: Chase(); break;

            case States.ATTACK: Attack(); break;

            case States.DEAD: Dead(); break;

            default: Debug.Log("Does not exists"); break;
        }

        SetAnimationClip();
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

        // move on given checkpoints - stay at a point.
        // player in range - change to chase
    }
    private void Patrol()
    {

    }
    private void Suspisius()
    {
        speed = 3;
        stopTime = 2.5f;
        // move with confusion and change in behaviour while doing patrolling.
    }
    private void Chase()
    {
        speed = 5;
        // move towards the player till the given range.
    }
    private void Attack()
    {
        if(stopTime == 0) stopTime = animationClip != null ? animationClip.length : 2f;
        if(animationPlayCountLimit == 0) animationPlayCountLimit = Random.Range(1, 5);

        if(currentAnimationPlayCount < animationPlayCountLimit)
        {
            phaseTimer += Time.deltaTime;

            if(phaseTimer >= stopTime)
            {
                Debug.Log("Attack");
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

        Invoke(nameof(RevertToAttackState), stopTime);
    }
    private void RevertToAttackState()
    {
        ChangeState(States.ATTACK);
    }
    private void Dead()
    {
        stopTime = animationClip != null ? animationClip.length : 0.1f;
        Destroy(gameObject, stopTime);
        // health gets zero - dead.
    }
}

/*
    Movement
    Speed
    Direction
*/