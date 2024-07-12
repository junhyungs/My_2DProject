using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Slimer : Monster
{
    public Slimer()
    {
        m_MonsterAtk = 10;
        m_MonsterHP = 200;
        m_MonsterJumpForce = 200.0f;
        m_MonsterSpeed = 2.0f;
        
    }
    private GameObject m_BossHPslider;
    private BoxCollider2D m_BossCollider;

    private int m_BossMaxHP = 200;

    public float m_AttackStatePos;
    
    public int BossMaxHP => m_BossMaxHP;
    public int BossHP
    {
        get { return m_MonsterHP; }
        set { m_MonsterHP = value; }
    }



    private void Awake()
    {
        m_MonsterState = gameObject.AddComponent<StateMachine>();
        m_BossCollider = GetComponent<BoxCollider2D>();
        m_MonsterRigidbody = GetComponent<Rigidbody2D>();
        m_Monstersprite = GetComponent<SpriteRenderer>();
        m_MonsterAnimator = GetComponent<Animator>();
        m_Player = GameObject.Find("Player").GetComponent<Player>();
        m_BossHPslider = GameObject.Find("BossHPCanvas").gameObject.transform.GetChild(0).gameObject;

        m_MonsterState.AddState(StateMachine.E_STATE.Start, new StartState(this));
        m_MonsterState.AddState(StateMachine.E_STATE.Idle,new IdleState(this));
        m_MonsterState.AddState(StateMachine.E_STATE.Move,new MoveState(this));
        m_MonsterState.AddState(StateMachine.E_STATE.Attack,new AttackState(this));
    }

    public abstract class SlimerState : BaseState
    {
        protected Slimer m_Slimer;
        protected Vector3 m_PlayerPos;

        public SlimerState(Slimer slimer)
        {
            m_Slimer = slimer;
        }
    }
    public class StartState : SlimerState
    {
        public StartState(Slimer slimer) : base(slimer) { }

        public override void OnCollisionEnter2D(Collision2D collision)
        {

        }

        public override void OnCollisionExit2D(Collision2D collision)
        {

        }

        public override void OnCollisionStay2D(Collision2D collision)
        {

        }

        public override void OnTriggerEnter2D(Collider2D collision)
        {

        }

        public override void OnTriggerExit2D(Collider2D collision)
        {

        }

        public override void OnTriggerStay2D(Collider2D collision)
        {

        }

        public override void StateEnter()
        {
            m_Slimer.m_MonsterState.ChangeState(StateMachine.E_STATE.Idle);
        }

        public override void StateExit()
        {

        }

        public override void StateFixedUpdate()
        {

        }
    }

    public class IdleState : SlimerState
    {
        private bool isMove = false;
        public IdleState(Slimer slimer) : base(slimer) { }

        public override void OnCollisionEnter2D(Collision2D collision)
        {
            
        }

        public override void OnCollisionExit2D(Collision2D collision)
        {
            
        }

        public override void OnCollisionStay2D(Collision2D collision)
        {
            
        }

        public override void OnTriggerEnter2D(Collider2D collision)
        {
         
        }

        public override void OnTriggerExit2D(Collider2D collision)
        {
         
        }

        public override void OnTriggerStay2D(Collider2D collision)
        {
         
        }

        public override void StateEnter()
        {
            m_Slimer.StartCoroutine(MoveReady());
            Debug.Log("슬라임 아이들");
        }
        

        public override void StateExit()
        {
            isMove = false;
        }

        public override void StateFixedUpdate()
        {
            if (isMove)
            {
                m_Slimer.m_MonsterState.ChangeState(StateMachine.E_STATE.Move);
            }
        }
        private IEnumerator MoveReady()
        {
            yield return new WaitForSeconds(4.0f);
            isMove = true;
        }
    }

    public class MoveState : SlimerState
    {
        public MoveState(Slimer slimer) : base(slimer) { }

        private float m_ChangeIdle;

        public override void OnCollisionEnter2D(Collision2D collision)
        {
            
        }

        public override void OnCollisionExit2D(Collision2D collision)
        {
            
        }

        public override void OnCollisionStay2D(Collision2D collision)
        {
            
        }

        public override void OnTriggerEnter2D(Collider2D collision)
        {
            
        }

        public override void OnTriggerExit2D(Collider2D collision)
        {
            
        }

        public override void OnTriggerStay2D(Collider2D collision)
        {
         
        }

        public override void StateEnter()
        {
            Debug.Log("슬라임 무브상태");
            m_Slimer.StartCoroutine(MovePos());
        }

        public override void StateExit()
        {
            m_Slimer.StopCoroutine(MovePos());
            m_ChangeIdle = 0;
        }

        public override void StateFixedUpdate()
        {
            
            m_ChangeIdle += Time.deltaTime;

            if (m_ChangeIdle > 10.0f)
            {
                m_Slimer.m_MonsterState.ChangeState(StateMachine.E_STATE.Idle);
            }

            PlayerPos();
            m_Slimer.FlipX(m_Slimer);

            Vector2 move = new Vector2(m_Slimer.m_MonsterPosX, m_Slimer.m_MonsterRigidbody.velocity.y).normalized * m_Slimer.m_MonsterSpeed * Time.deltaTime;

            m_Slimer.transform.Translate(move);


        }
        public void PlayerPos()
        {
            if (Vector2.Distance(m_Slimer.m_Player.transform.position, m_Slimer.transform.position) < m_Slimer.m_AttackStatePos)
                m_Slimer.m_MonsterState.ChangeState(StateMachine.E_STATE.Attack);
        }

        private IEnumerator MovePos()
        {
            while (true)
            {
                m_Slimer.m_MonsterPosX = Random.Range(-1.0f, 1.0f);

                yield return new WaitForSeconds(4.0f);
            }
        }
    }

    public class AttackState : SlimerState
    {
        public AttackState(Slimer slimer) : base(slimer) { }
        private float m_CoolTime;
        
        private bool JumpOn = true;
        private bool OnPoison = true;
        private bool isAttackOn = true;
        private float m_Pos;
        
        enum BossSkill
        {
            JumpAttack = 1,
            Attack
        }

        public override void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.tag == "Player")
            {
                m_Slimer.StartCoroutine(Poison());
            }
            
        }

        public override void OnCollisionExit2D(Collision2D collision)
        {
            
        }

        public override void OnCollisionStay2D(Collision2D collision)
        {
         
        }

        public override void OnTriggerEnter2D(Collider2D collision)
        {
         
        }

        public override void OnTriggerExit2D(Collider2D collision)
        {
         
        }

        public override void OnTriggerStay2D(Collider2D collision)
        {
         
        }

        public override void StateEnter()
        {
            Debug.Log("슬라임 어택상태");
            m_Slimer.m_BossHPslider.SetActive(true);
        }

        public override void StateExit()
        {
            m_Slimer.m_BossHPslider.SetActive(false);
        }

        public override void StateFixedUpdate()
        {
            BossflipX();

            m_PlayerPos = m_Slimer.m_Player.transform.position;

            ChangeIdle(m_PlayerPos);

            if (Vector2.Distance(m_PlayerPos, m_Slimer.transform.position) > 10.0f)
                Jump();
            else if(Vector2.Distance(m_PlayerPos, m_Slimer.transform.position) < 5.0f && isAttackOn)
            {
                isAttackOn = false;
                m_Slimer.m_MonsterAnimator.SetBool("isAttack", true);
                m_Slimer.StartCoroutine(Attack());
            }
            Vector2 move = new Vector2(m_PlayerPos.x - m_Slimer.transform.position.x, m_Slimer.m_MonsterRigidbody.velocity.y).normalized * m_Slimer.m_MonsterSpeed * Time.deltaTime;
            m_Slimer.transform.Translate(move);

        }

        private void ChangeIdle(Vector3 playerPos)
        {
            if (Vector2.Distance(playerPos, m_Slimer.transform.position) > 15.0f)
                m_Slimer.m_MonsterState.ChangeState(StateMachine.E_STATE.Idle);
        }

        private void BossflipX()
        {
            if (m_Slimer.m_Player.transform.position.x > m_Slimer.transform.position.x)
                m_Slimer.m_Monstersprite.flipX = true;
            else if (m_Slimer.m_Player.transform.position.x < m_Slimer.transform.position.x)
                m_Slimer.m_Monstersprite.flipX = false;
        }
        private void Jump()
        {
            if (JumpOn)
            {
                JumpOn = false;

                if (m_Slimer.m_Player.transform.position.x > m_Slimer.transform.position.x)
                    m_Pos = -40.0f;
                else if (m_Slimer.m_Player.transform.position.x < m_Slimer.transform.position.x)
                    m_Pos = 40.0f;


                m_Slimer.m_MonsterRigidbody.velocity = Vector2.zero;
                m_Slimer.m_MonsterRigidbody.AddForce(new Vector2(m_PlayerPos.x - m_Slimer.transform.position.x * m_Pos, 2000.0f));
                m_Slimer.StartCoroutine(SkillCoolTime());
            }
        }
       
        private IEnumerator SkillCoolTime()
        {
            yield return new WaitForSeconds(5.0f);
            JumpOn = true;
        }

        private IEnumerator Attack()
        {
            m_Slimer.m_MonsterAtk += 20;

            yield return new WaitForSeconds(2.0f);

            m_Slimer.m_MonsterAtk -= 20;
            isAttackOn = true;
            m_Slimer.m_MonsterAnimator.SetBool("isAttack", false);
        }




        private IEnumerator Poison()
        {
            if (OnPoison)
            {
                float Poison = 0;

                OnPoison = false;

                while (Poison < 5.0f)
                {
                    PlayerHPManager.Instance.PlayerDamage(1);
                    yield return new WaitForSeconds(1.0f);

                    Poison++;
                }

                OnPoison = true;
            }
        }
    }

}
