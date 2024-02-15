using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerStateMachine : MonoBehaviour
{
    protected PlayerState State;
    public void SetState(PlayerState state)
    {
        State = state;
        State.Start();
    }
}