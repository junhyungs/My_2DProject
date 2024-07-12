using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class StateMachine : MonoBehaviour
{
    public enum E_STATE
    {
        Idle,
        Run,
        Jump,
        Attack,
        Hurt,
        Start,
        Climb,

        Move,
        Check

    }
    private Dictionary<E_STATE, BaseState>m_StateDic = new Dictionary<E_STATE, BaseState>();
    private BaseState m_State;

    private void Start()
    {
        m_State = m_StateDic[E_STATE.Start];
        m_State.StateEnter();
    }

    private void FixedUpdate()
    {
        m_State.StateFixedUpdate();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        m_State.OnCollisionEnter2D(collision);
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        m_State.OnCollisionExit2D(collision);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        m_State.OnTriggerEnter2D(collision);
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        m_State.OnTriggerStay2D(collision);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        m_State.OnTriggerExit2D(collision);
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        m_State.OnCollisionStay2D(collision);
    }    

    public void AddState(E_STATE state, BaseState baseState)
    {
        m_StateDic.Add(state, baseState);
    }

    public void ChangeState(E_STATE state)
    {
        m_State.StateExit();
        m_State = m_StateDic[state];
        m_State.StateEnter();
    }
}



public abstract class BaseState
{
    public abstract void StateEnter();
    public abstract void StateFixedUpdate();
    public abstract void StateExit();
    public abstract void OnCollisionEnter2D(Collision2D collision);
    public abstract void OnCollisionExit2D(Collision2D collision);
    public abstract void OnCollisionStay2D(Collision2D collision);
    public abstract void OnTriggerEnter2D(Collider2D collision);
    public abstract void OnTriggerStay2D(Collider2D collision);
    public abstract void OnTriggerExit2D(Collider2D collision);
}