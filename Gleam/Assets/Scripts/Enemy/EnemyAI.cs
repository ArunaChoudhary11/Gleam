using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public enum States{ IDLE, PETROL, SUSPICIUS, INTERACTION, RETREAT, ATTACK, DEAD}

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
            case States.IDLE:
                Idle();
            break;
            case States.PETROL:
                Petrol();
            break;
            case States.RETREAT:
                Retreat();
            break;
            case States.SUSPICIUS:
                Suspicius();
            break;
            case States.INTERACTION:
                Interaction();
            break;
            case States.ATTACK:
                Attack();
            break;
            case States.DEAD:
                Dead();
            break;
            default:
                Debug.Log("Does not exists");
            break;
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
    }
    private void Idle()
    {

    }
    private void Petrol()
    {

    }
    private void Retreat()
    {

    }
    private void Suspicius()
    {

    }
    private void Interaction()
    {
        
    }
    private void Attack()
    {

    }
    private void Dead()
    {
        
    }
}

/*
    Movement
    Speed
    Direction
    Dialogue
*/