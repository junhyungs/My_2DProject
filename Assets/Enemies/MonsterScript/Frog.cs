using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;
using static Frog;

public class Frog : Monster
{
    
    public Frog()
    {
        m_MonsterHP = 50;
        m_MonsterAtk = 5;
        m_MonsterJumpForce = 200.0f;
        m_MonsterSpeed = 10.0f;
        m_MonsterPosY = 300.0f;
    }
    
    private void Awake()
    {
        m_MonsterAnimator = GetComponent<Animator>();
        m_MonsterRigidbody = GetComponent<Rigidbody2D>();
        m_Monstersprite = GetComponent<SpriteRenderer>();
        m_MonsterState = gameObject.AddComponent<StateMachine>();
        m_CircleCollider = GetComponent<CircleCollider2D>();
        isMove = false;
        CheckStateMove = false;
        
        MonsterHPManager.Instance.AddMonster(this, m_MonsterHP);    
        m_MonsterState.AddState(StateMachine.E_STATE.Idle, new IdleState(this));
        m_MonsterState.AddState(StateMachine.E_STATE.Move, new MoveState(this));
        m_MonsterState.AddState(StateMachine.E_STATE.Start, new Start(this));
        
    }
    
    public class Start : BaseState
    {
        private Frog m_Frog;
        public Start(Frog frog)
        {
            m_Frog = frog;
        }
        public override void OnCollisionEnter2D(Collision2D collision) { }
        public override void OnCollisionExit2D(Collision2D collision) { }
        public override void OnCollisionStay2D(Collision2D collision) { }
        public override void OnTriggerEnter2D(Collider2D collision) { }
        public override void OnTriggerExit2D(Collider2D collision) { }
        public override void OnTriggerStay2D(Collider2D collision) { }        
        public override void StateEnter()
        {
            m_Frog.m_MonsterState.ChangeState(StateMachine.E_STATE.Idle);
        }
        public override void StateExit() { }
        public override void StateFixedUpdate() { }
    }

    public class IdleState : BaseState
    {
        private Frog m_Frog;
        public IdleState(Frog frog)
        {
            m_Frog = frog;
        }
        public override void OnCollisionEnter2D(Collision2D collision) { }
        public override void OnCollisionStay2D(Collision2D collision) { }
        public override void OnCollisionExit2D(Collision2D collision) { }
        public override void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.tag == "Player")
            {
                MonsterHPManager.Instance.StartCoroutine(MonsterHPManager.Instance.MonsterDead(m_Frog));
            }
        }
        public override void OnTriggerExit2D(Collider2D collision) { }
        public override void OnTriggerStay2D(Collider2D collision) { }
        public override void StateEnter()
        {
            m_Frog.m_MonsterPosX = Random.Range(-1.0f, 1);
            m_Frog.Ray(m_Frog);
        }
        public override void StateFixedUpdate()
        {
            m_Frog.m_fData += Time.deltaTime;

            if (m_Frog.m_fData > 3.0f)
            {
                m_Frog.m_MonsterState.ChangeState(StateMachine.E_STATE.Move);
            }
        }

        public override void StateExit()
        {
            m_Frog.m_fData = 0;
        }

    }

    public class MoveState : BaseState
    {
        private Frog m_Frog;
        public MoveState(Frog frog)
        {
            m_Frog = frog;
        }
        public override void OnCollisionEnter2D(Collision2D collision) { }
        public override void OnCollisionExit2D(Collision2D collision) { }
        public override void OnCollisionStay2D(Collision2D collision) { }
        public override void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.CompareTag("Ground"))
            {
                m_Frog.m_MonsterState.ChangeState(StateMachine.E_STATE.Idle);
            }
        }
        public override void OnTriggerExit2D(Collider2D collision) { }
        public override void OnTriggerStay2D(Collider2D collision) { }
        public override void StateEnter()
        {
            m_Frog.m_MonsterAnimator.SetBool("isJump", true);

            if (m_Frog.m_MonsterPosX > 0)
            {
                m_Frog.m_Monstersprite.flipX = true;
            }
            else if (m_Frog.m_MonsterPosX < 0)
            {
                m_Frog.m_Monstersprite.flipX = false;
            }
            m_Frog.m_MonsterRigidbody.velocity = Vector2.zero;
            m_Frog.m_MonsterRigidbody.AddForce(new Vector2(m_Frog.m_MonsterPosX * 200.0f, m_Frog.m_MonsterPosY));
        }
        public override void StateExit()
        {
            m_Frog.m_MonsterAnimator.SetBool("isJump", false);
            m_Frog.m_MonsterAnimator.SetBool("isMove", false);
        }
        public override void StateFixedUpdate()
        {
            if (m_Frog.m_MonsterRigidbody.velocity.y < 0)
            {   
                m_Frog.m_MonsterAnimator.SetBool("isMove", true);
            }
        }
    }
  
}
