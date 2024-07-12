using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Dog : Monster
{
    public Dog()
    {
        m_MonsterHP = 20;
        m_MonsterSpeed = 5.0f;
        m_MonsterAtk = 10;
    }
    private void Awake()
    {
        m_MonsterState = gameObject.AddComponent<StateMachine>();
        m_Monstersprite = GetComponent<SpriteRenderer>();
        m_MonsterRigidbody = GetComponent<Rigidbody2D>();
        m_MonsterAnimator = GetComponent<Animator>();

        MonsterHPManager.Instance.AddMonster(this,m_MonsterHP);
        m_MonsterState.AddState(StateMachine.E_STATE.Start, new StartState(this));
        m_MonsterState.AddState(StateMachine.E_STATE.Idle, new IdleState(this));
        m_MonsterState.AddState(StateMachine.E_STATE.Move, new MoveState(this));
    }
    public abstract class DogState : BaseState
    {
        protected Dog m_Dog;
        public DogState(Dog dog)
        {
            m_Dog = dog;
        }
    }
    public class StartState : DogState
    {
        public StartState(Dog dog) : base(dog) { }
        public override void OnCollisionEnter2D(Collision2D collision) { }
        public override void OnCollisionStay2D(Collision2D collision) { }
        public override void OnCollisionExit2D(Collision2D collision) { }
        public override void OnTriggerEnter2D(Collider2D collision) { }
        public override void OnTriggerExit2D(Collider2D collision) { }
        public override void StateEnter() 
        {          
            m_Dog.m_MonsterState.ChangeState(StateMachine.E_STATE.Idle);
        }
        public override void StateExit() { }
        public override void StateFixedUpdate() { }
        public override void OnTriggerStay2D(Collider2D collision) { }
    }

    public class IdleState : DogState
    {
        public IdleState(Dog dog) : base(dog) { }
        
        public override void OnCollisionEnter2D(Collision2D collision) { }
        public override void OnCollisionExit2D(Collision2D collision) { }
        public override void OnCollisionStay2D(Collision2D collision) { }
        public override void OnTriggerEnter2D(Collider2D collision)
        {
            if(collision.tag == "Player")
            {
                MonsterHPManager.Instance.StartCoroutine(MonsterHPManager.Instance.MonsterDead(m_Dog));
            }
        }
        public override void OnTriggerExit2D(Collider2D collision) { }
        public override void OnTriggerStay2D(Collider2D collision) { }
        public override void StateEnter()
        {
            m_Dog.StartCoroutine(Move());
        }

        public override void StateExit()
        {
            m_Dog.isMove = false;
        }

        public override void StateFixedUpdate()
        {
            if(m_Dog.isMove)
            {
                m_Dog.m_MonsterState.ChangeState(StateMachine.E_STATE.Move);
            }
        }

        private IEnumerator Move()
        {
            yield return new WaitForSeconds(Random.Range(3.0f, 4.0f));
            m_Dog.isMove = true;
        }
    }
    public class MoveState : DogState
    {
        public MoveState(Dog dog) : base(dog) { }
        
        public override void OnCollisionEnter2D(Collision2D collision) { }
        public override void OnCollisionStay2D(Collision2D collision) { }
        public override void OnCollisionExit2D(Collision2D collision) { }
        public override void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.tag == "Player")
            {
                MonsterHPManager.Instance.StartCoroutine(MonsterHPManager.Instance.MonsterDead(m_Dog));
            }
        }
        public override void OnTriggerExit2D(Collider2D collision) { }
        public override void OnTriggerStay2D(Collider2D collision) { }
        public override void StateEnter()
        {
            m_Dog.m_MonsterAnimator.SetBool("isMove", true);
            m_Dog.StartCoroutine(MoveDir());
        }

        public override void StateExit()
        {
            m_Dog.m_fData = 0;
            m_Dog.m_MonsterAnimator.SetBool("isMove", false);
            m_Dog.StopCoroutine(MoveDir());
        }

        public override void StateFixedUpdate()
        {
            m_Dog.FlipX(m_Dog);
            m_Dog.Ray(m_Dog);
            Vector2 Move = new Vector2(m_Dog.m_MonsterPosX, m_Dog.m_MonsterRigidbody.velocity.y).normalized * m_Dog.m_MonsterSpeed * Time.deltaTime;
            m_Dog.transform.Translate(Move);
        }
        private IEnumerator MoveDir()
        {
            m_Dog.m_MonsterPosX = Random.Range(-1.0f, 1);
            yield return new WaitForSeconds(3.0f);
            m_Dog.m_MonsterState.ChangeState(StateMachine.E_STATE.Idle);
        }
    }
    
}
