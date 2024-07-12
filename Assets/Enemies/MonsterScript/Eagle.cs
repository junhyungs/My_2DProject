using Cinemachine;
using System.Collections;
using System.Collections.Generic;
//using System.Runtime.Serialization.Formatters;
//using TMPro.EditorUtilities;
//using UnityEditor.Rendering;
using UnityEngine;

public class Eagle : Monster
{
    private int boomAtk = 20;
    public Eagle()
    {
        m_MonsterName = "Eagle";
        m_MonsterHP = 20;
        m_MonsterSpeed = 4.0f;
        m_MonsterAtk = 10;
    }
    public float m_IdleDistance;
    public float m_MoveDistance;

    private Vector3 m_StartPos;
    private Vector3 m_PlayerPos;
    private Vector3 m_PlayerPosition;
    private bool isAttack = true;
    

    private void Awake()
    {
        m_MonsterAnimator = GetComponent<Animator>();
        m_MonsterRigidbody = GetComponent<Rigidbody2D>();
        m_Monstersprite = GetComponent<SpriteRenderer>();
        m_MonsterState = gameObject.AddComponent<StateMachine>();

        MonsterHPManager.Instance.AddMonster(this, m_MonsterHP);
        m_Player = GameObject.Find("Player").GetComponent<Player>();
        m_MonsterState.AddState(StateMachine.E_STATE.Start, new Start(this));
        m_MonsterState.AddState(StateMachine.E_STATE.Idle, new Idle(this));
        m_MonsterState.AddState(StateMachine.E_STATE.Attack, new AttackState(this));
        m_MonsterState.AddState(StateMachine.E_STATE.Move, new Move(this));
        isMove = true;
        m_StartPos = transform.position;
    }

    private IEnumerator EagleAttackReturn()
    {
        Debug.Log("코루틴 시작");

        float Timer = 0;

        m_MonsterAnimator.SetBool("isAttack", false);

        while (true && Timer <= 2.0f )
        {
            if (m_Player.transform.position.x > transform.position.x)
            {
                m_Monstersprite.flipX = true;
            }
            else if (m_Player.transform.position.x < transform.position.x)
            {
                m_Monstersprite.flipX = false;
            }

            Vector2 dir = (m_StartPos - transform.position).normalized * m_MonsterSpeed * Time.deltaTime;
            transform.Translate(dir);

            yield return null;

            Timer += Time.deltaTime;
        }
        isAttack = true;

        m_PlayerPos = m_Player.transform.position;

        if (Vector2.Distance(transform.position, m_PlayerPos) > m_MoveDistance)
            m_MonsterState.ChangeState(StateMachine.E_STATE.Idle);

        StopCoroutine(EagleAttackReturn());
    }
    public abstract class EagleState : BaseState
    {
        protected Eagle m_Eagle;
        public EagleState(Eagle eagle)
        {
            m_Eagle = eagle;
        }
    }
    public class Start : EagleState
    {
        public Start(Eagle eagle) : base(eagle) { }
        public override void OnCollisionEnter2D(Collision2D collision) { }
        public override void OnCollisionStay2D(Collision2D collision) { }
        public override void OnCollisionExit2D(Collision2D collision) { }
        public override void OnTriggerEnter2D(Collider2D collision) { }
        public override void OnTriggerExit2D(Collider2D collision) { }
        public override void StateEnter()
        {
            m_Eagle.m_MonsterState.ChangeState(StateMachine.E_STATE.Idle);
        }
        public override void StateExit() { }
        public override void StateFixedUpdate() { }
        public override void OnTriggerStay2D(Collider2D collision) { }
    }
    public class Idle : EagleState
    {
        public Idle(Eagle eagle) : base(eagle) { }
        public override void OnCollisionEnter2D(Collision2D collision) { }        
        public override void OnCollisionStay2D(Collision2D collision) { }
        public override void OnCollisionExit2D(Collision2D collision) { }
        public override void OnTriggerEnter2D(Collider2D collision)
        {
            if(collision.gameObject.tag == "Player")
            {
                MonsterHPManager.Instance.StartCoroutine(MonsterHPManager.Instance.MonsterDead(m_Eagle));
            }
        }
        public override void OnTriggerStay2D(Collider2D collision) { }
        public override void OnTriggerExit2D(Collider2D collision) { }
        public override void StateEnter()
        {
            m_Eagle.m_MonsterAnimator.SetBool("isAttack", false);
            m_Eagle.isMove = true;
        }

        public override void StateExit() { }
        public override void StateFixedUpdate()
        {
            PlayerPos();

            if (m_Eagle.isMove)
            {
                if (Vector2.Distance(m_Eagle.m_StartPos, m_Eagle.transform.position) < 0.1f)
                    m_Eagle.isMove = false;


                Vector2 dir = (m_Eagle.m_StartPos - m_Eagle.transform.position).normalized * m_Eagle.m_MonsterSpeed * Time.deltaTime;
                m_Eagle.transform.Translate(dir);
            }
        }
        public void PlayerPos()
        {
            if (Vector2.Distance(m_Eagle.m_Player.transform.position, m_Eagle.transform.position) < m_Eagle.m_MoveDistance)
            {
                m_Eagle.m_MonsterState.ChangeState(StateMachine.E_STATE.Move);
            }
        }
    }
    public class Move : EagleState
    {
        public Move(Eagle eagle) : base(eagle) { }

        private Transform m_Target;
        public override void OnCollisionEnter2D(Collision2D collision) { }
        public override void OnCollisionStay2D(Collision2D collision) { }
        public override void OnCollisionExit2D(Collision2D collision) { }
        public override void OnTriggerEnter2D(Collider2D collision) { }
        public override void OnTriggerStay2D(Collider2D collision) { }
        public override void OnTriggerExit2D(Collider2D collision) { }
        public override void StateEnter()
        {
            m_Target = m_Eagle.m_Player.transform;
        }
        public override void StateExit() { }        
        public override void StateFixedUpdate()
        {
            Return();
            ChangeAttackState();

            if (m_Target.position.x > m_Eagle.transform.position.x)
            {
                m_Eagle.m_Monstersprite.flipX = true;
            }
            else if (m_Target.position.x < m_Eagle.transform.position.x)
            {
                m_Eagle.m_Monstersprite.flipX = false;
            }

            Vector2 dir = (m_Target.position - m_Eagle.transform.position).normalized * m_Eagle.m_MonsterSpeed * Time.deltaTime;
            m_Eagle.transform.Translate(dir);
        }
        public void ChangeAttackState()
        {
            if (Vector2.Distance(m_Target.position, m_Eagle.transform.position) < 4.0f)
                m_Eagle.m_MonsterState.ChangeState(StateMachine.E_STATE.Attack);
        }
        public void Return()
        {
            if (Vector2.Distance(m_Target.position, m_Eagle.transform.position) > m_Eagle.m_MoveDistance)
                m_Eagle.m_MonsterState.ChangeState(StateMachine.E_STATE.Idle);
        }
    }

    public class AttackState : EagleState
    {
        public AttackState(Eagle eagle) : base(eagle) { }
        public override void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                PlayerHPManager.Instance.PlayerDamage(m_Eagle.boomAtk);
                MonsterHPManager.Instance.StartCoroutine(MonsterHPManager.Instance.MonsterDead(m_Eagle));
            }
        }
        public override void OnCollisionStay2D(Collision2D collision) { }
        public override void OnCollisionExit2D(Collision2D collision) { }
        public override void OnTriggerEnter2D(Collider2D collision) { }
        public override void OnTriggerStay2D(Collider2D collision) { }
        public override void OnTriggerExit2D(Collider2D collision) { }
        public override void StateEnter()
        {
            m_Eagle.m_PlayerPos = m_Eagle.m_Player.transform.position;
            m_Eagle.isAttack = true;
        }

        public override void StateExit()
        {
            m_Eagle.isMove = true;
        }
        
        public override void StateFixedUpdate()
        {

            if (m_Eagle.isAttack)
            {
                m_Eagle.m_MonsterAnimator.SetBool("isAttack", true);

                Vector2 Attackdir = (m_Eagle.m_PlayerPos - m_Eagle.transform.position).normalized * 5.5f * Time.deltaTime;
                m_Eagle.transform.Translate(Attackdir);

                if (Vector2.Distance(m_Eagle.m_PlayerPos, m_Eagle.transform.position) < 0.1f)
                {
                    m_Eagle.StartCoroutine(m_Eagle.EagleAttackReturn());
                    m_Eagle.isAttack = false;
                }
            }
        }
    }
}
