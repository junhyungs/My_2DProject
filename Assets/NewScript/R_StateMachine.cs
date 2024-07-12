using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class R_StateMachine : MonoBehaviour
{
    public enum E_STATE
    {
        Idle,
        Run,
        Jump,
        Attack,
        Hurt,
        Climb,
        Move
    }

    private Dictionary<E_STATE, R_BaseState> m_StateDictionary = new Dictionary<E_STATE, R_BaseState>();
    private R_BaseState m_BaseState;

    private void Start()
    {
        m_BaseState = m_StateDictionary[E_STATE.Idle];
        m_BaseState.StateEnter();
    }

    private void FixedUpdate()
    {
        m_BaseState.StateFixedUpdate();
    }


    public void AddState(E_STATE state, R_BaseState r_BaseState)
    {
        m_StateDictionary.Add(state, r_BaseState);
    }

    public void ChangeState(E_STATE newState)
    {
        m_BaseState.StateExit();
        m_BaseState = m_StateDictionary[newState];
        m_BaseState.StateEnter();
    }
}

public abstract class R_BaseState
{
    public abstract void StateEnter();
    public abstract void StateFixedUpdate();
    public abstract void StateExit();

}