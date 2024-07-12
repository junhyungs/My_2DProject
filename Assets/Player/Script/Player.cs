using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TMPro;
using System;


public class Player : MonoBehaviour
{
    [SerializeField] int m_PlayerAtk;
    [SerializeField] float m_Playerjumpforce;
    [SerializeField] float m_PlayerKnockBackForceX;
    [SerializeField] float m_PlayerKnockBackForceY;

    private float m_Playerspeed = 7.0f;
    private bool isJump = true;
    private bool m_Ladder = false;
    private bool isNext = true;
    private float m_Playerjumpcount;
    private ItemInventoryManager m_ItemInventoryManager;
    private StateMachine m_Playerstatemachine;
    private Rigidbody2D m_Rigid2D;
    private SpriteRenderer m_Sprite;
    private Animator m_Animator;
    private ShootComponent m_ShootComponent;
    private Monster m_Monster;
    
    
    private Vector2 m_Input;
    private Vector2 m_LadderInput;

    public float PlayerKnockBackForceX => m_PlayerKnockBackForceX;
    public float PlayerKnockBackForceY => m_PlayerKnockBackForceY;
    public float PlayerJumpForce => m_Playerjumpforce;
    public float PlayerSpeed
    {
        get { return m_Playerspeed; }
        set { m_Playerspeed = value; }
    }
    public bool IsJump
    {
        get { return isJump; }
        set { isJump = value; }
    }
    public bool Ladder
    {
        get { return m_Ladder; }
        set { m_Ladder = value; }
    }
    public float PlayerJumpCount
    {
        get { return m_Playerjumpcount; }
        set { m_Playerjumpcount = value; }
    }

    public StateMachine PlayerStateMachine => m_Playerstatemachine;
    public Rigidbody2D Rigid2D => m_Rigid2D;
    public SpriteRenderer Sprite => m_Sprite;
    public Animator Animator => m_Animator;    
    public Vector2 Input => m_Input;
    public Vector2 LadderInput
    {
        get { return m_LadderInput; }
        set { m_LadderInput = value;}
    }
    public ShootComponent ShootComponent => m_ShootComponent;

    void OnMove(InputValue Playerinput)
    {
        m_Input = Playerinput.Get<Vector2>();
    }
    void OnJump()
    {
        if (isJump)
        {
            m_Playerstatemachine.ChangeState(StateMachine.E_STATE.Jump);
        }
    }

    void OnItemInventory()
    {
        m_ItemInventoryManager.ItemInventoryActive();
    }

    void OnAttack()
    {
        m_ShootComponent.Shoot();
    }
    void OnClimb(InputValue input)
    {
        if (m_Ladder)
        {
            m_LadderInput = input.Get<Vector2>();
        }
    }
    void OnMenu()
    {
        GameManager.Instance.MenuActive();
    }

    
    private void Awake()
    {
        
        m_Animator = GetComponent<Animator>();
        m_Rigid2D = GetComponent<Rigidbody2D>();
        m_Sprite = GetComponent<SpriteRenderer>();
        m_ShootComponent = GetComponent<ShootComponent>();
        m_ItemInventoryManager = GameObject.Find("ItemInventoryCanvas").GetComponent<ItemInventoryManager>();


        m_Playerstatemachine = gameObject.AddComponent<StateMachine>();
        m_Playerstatemachine.AddState(StateMachine.E_STATE.Idle, new IdleState(this));
        m_Playerstatemachine.AddState(StateMachine.E_STATE.Run, new RunState(this));
        m_Playerstatemachine.AddState(StateMachine.E_STATE.Jump, new JumpState(this));
        m_Playerstatemachine.AddState(StateMachine.E_STATE.Hurt, new HurtState(this));
        m_Playerstatemachine.AddState(StateMachine.E_STATE.Start, new Start(this));
        m_Playerstatemachine.AddState(StateMachine.E_STATE.Climb, new ClimbState(this));    
    }



    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("DeadZone"))
        {
            m_Animator.SetBool("isHurt", true);

            GameManager.Instance.GameOver();
        }
        else if (collision.gameObject.CompareTag("NextStage"))
        {
            if (isNext)
            {
                m_Animator.SetTrigger("Next");
                SceneMgr.Instance.NextScene();
                isNext = false;
            }
        }
        if (collision.gameObject.layer == 6)
        {
            Debug.Log("레이어변경전" +collision.gameObject.layer);
            collision.gameObject.layer = 7;
            Debug.Log("레이어변경후"+collision.gameObject.layer);
            GameManager.Instance.AddScore(1);
        }
            
    }


    public void Move(Vector2 Inputvalue, SpriteRenderer PlayerSpriteRenderer, Player player)
    {
        if (Inputvalue.x < 0)
        {
            PlayerSpriteRenderer.flipX = true;
        }
        else if (Inputvalue.x > 0)
        {
            PlayerSpriteRenderer.flipX = false;
        }
        Vector2 PlayerMove = Inputvalue * player.PlayerSpeed * Time.deltaTime;
        player.transform.Translate(PlayerMove);
    }


   
    public class Start : BaseState
    {
        private Player m_Player;
        public Start(Player player)
        {
            m_Player = player;
        }
        public override void OnCollisionEnter2D(Collision2D collision) { }
        public override void OnCollisionExit2D(Collision2D collision) { }
        public override void OnCollisionStay2D(Collision2D collision) { }
        public override void OnTriggerStay2D(Collider2D collision) { }
        public override void OnTriggerEnter2D(Collider2D collision) { }
        public override void OnTriggerExit2D(Collider2D collision) { }
        public override void StateEnter()
        {
            m_Player.PlayerStateMachine.ChangeState(StateMachine.E_STATE.Idle);
        }
        public override void StateExit() { }
        public override void StateFixedUpdate() { }
    }
    public class IdleState : BaseState
    {
        private Player m_Player;
        public IdleState(Player player)
        {
            m_Player = player;
        }
        public override void OnCollisionEnter2D(Collision2D collision)
        {
            bool isHurt_x = collision.gameObject.CompareTag("Monster");
            bool isHurt_y = collision.gameObject.CompareTag("Monster") && collision.contacts[0].normal.y < 0;

            if (isHurt_x || isHurt_y)
            {
                m_Player.m_Monster = collision.gameObject.GetComponent<Monster>();
                m_Player.m_Playerstatemachine.ChangeState(StateMachine.E_STATE.Hurt);
            }
            else if(collision.gameObject.tag == "Boss")
            {
                m_Player.m_Monster = collision.gameObject.GetComponent<Monster>();
                m_Player.m_Playerstatemachine.ChangeState(StateMachine.E_STATE.Hurt);
            }
            
        }
        public override void OnCollisionStay2D(Collision2D collision) { }
        public override void OnCollisionExit2D(Collision2D collision) { }
        public override void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Monster"))
            {
                m_Player.Rigid2D.velocity = Vector2.zero;
                m_Player.Rigid2D.AddForce(new Vector2(0, m_Player.PlayerJumpForce));
            }
        }
        public override void OnTriggerStay2D(Collider2D collision) { }
        public override void OnTriggerExit2D(Collider2D collision) { }
        public override void StateEnter() { }
        public override void StateExit() { }
        public override void StateFixedUpdate()
        {
            if (m_Player.Input.x < 0 || m_Player.Input.x > 0)
            {
                m_Player.PlayerStateMachine.ChangeState(StateMachine.E_STATE.Run);
            }
        }
    }

    public class RunState : BaseState
    {
        private Player m_Player;
        public RunState(Player player)
        {
            m_Player = player;
        }
        public override void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.CompareTag("Monster"))
            {
                m_Player.m_Monster = collision.gameObject.GetComponent<Monster>();
                m_Player.m_Playerstatemachine.ChangeState(StateMachine.E_STATE.Hurt);
            }
            else if(collision.gameObject.tag == "Boss")
            {
                m_Player.m_Monster = collision.gameObject.GetComponent<Monster>();
                m_Player.m_Playerstatemachine.ChangeState(StateMachine.E_STATE.Hurt);
            }
        }
        public override void OnCollisionStay2D(Collision2D collision) { }
        public override void OnCollisionExit2D(Collision2D collision) { }
        public override void OnTriggerEnter2D(Collider2D collision)
        {         
            if (collision.tag == "ladder")
            {
                m_Player.PlayerStateMachine.ChangeState(StateMachine.E_STATE.Climb);
            }
            else if (collision.CompareTag("Monster"))
            {
                m_Player.Rigid2D.velocity = Vector2.zero;
                m_Player.Rigid2D.AddForce(new Vector2(0, m_Player.PlayerJumpForce));
            }
        }
        public override void OnTriggerExit2D(Collider2D collision) { }
        public override void StateEnter()
        {
            m_Player.Animator.SetBool("isRun", true);
        }
        public override void StateExit()
        {
            m_Player.Animator.SetBool("isRun", false);
        }
        public override void StateFixedUpdate()
        {
            if(m_Player.Input.x != 0)
            {
                m_Player.Move(m_Player.Input, m_Player.Sprite, m_Player);
            }
            else
            {
                m_Player.PlayerStateMachine.ChangeState(StateMachine.E_STATE.Idle);
            }
        }
        public override void OnTriggerStay2D(Collider2D collision) { }
    }
    public class JumpState : BaseState
    {
        private Player m_Player;
        public JumpState(Player player)
        {
            m_Player = player;
        }
        public override void StateEnter()
        {
            if (m_Player.PlayerJumpCount < 1 && m_Player.IsJump)
            {
                m_Player.PlayerJumpCount++;
                m_Player.Rigid2D.velocity = Vector2.zero;
                m_Player.Rigid2D.AddForce(new Vector2(0, m_Player.PlayerJumpForce));
                m_Player.Animator.SetBool("isGround", false);
                m_Player.IsJump = false;
            }
        }
        public override void StateExit()
        {
            m_Player.Animator.SetBool("isGround", true);
            m_Player.Animator.SetBool("isJumpFall", false);
            m_Player.IsJump = true;
        }
        public override void StateFixedUpdate()
        {
            if (m_Player.Rigid2D.velocity.y < 0)
            {
                m_Player.Animator.SetBool("isJumpFall", true);
            }

            m_Player.Move(m_Player.Input, m_Player.Sprite, m_Player);
        }

        public override void OnCollisionEnter2D(Collision2D collision)
        {
            bool isHurt = collision.gameObject.CompareTag("Monster");

            if (isHurt)
            {
                m_Player.m_Monster = collision.gameObject.GetComponent<Monster>();
                m_Player.m_Playerstatemachine.ChangeState(StateMachine.E_STATE.Hurt);
            }
            else if(collision.gameObject.tag == "Boss")
            {
                m_Player.m_Monster = collision.gameObject.GetComponent<Monster>();
                m_Player.m_Playerstatemachine.ChangeState(StateMachine.E_STATE.Hurt);
            }
        }
        public override void OnCollisionStay2D(Collision2D collision) { }
        public override void OnCollisionExit2D(Collision2D collision) { }
        public override void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Ground") || collision.tag == "Box")
            {
                m_Player.PlayerStateMachine.ChangeState(StateMachine.E_STATE.Idle);
                m_Player.PlayerJumpCount = 0;
            }
            else if (collision.CompareTag("Monster"))
            {
                m_Player.Rigid2D.velocity = Vector2.zero;
                m_Player.Rigid2D.AddForce(new Vector2(0, m_Player.PlayerJumpForce));
            }
            else if (collision.CompareTag("ladder"))
            {
                m_Player.PlayerStateMachine.ChangeState(StateMachine.E_STATE.Climb);
                m_Player.PlayerJumpCount = 0;
            }
                
        }
        public override void OnTriggerExit2D(Collider2D collision) { }
        public override void OnTriggerStay2D(Collider2D collision) { }
    }


    public class HurtState : BaseState
    {
        private Player m_Player;

        public HurtState(Player player)
        {
            m_Player = player;
        }
        public override void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.CompareTag("Ground"))
            {
                m_Player.m_Playerjumpcount = 0;

                if (m_Player.Input.x > 0 || m_Player.Input.x < 0)
                {
                    m_Player.PlayerStateMachine.ChangeState(StateMachine.E_STATE.Run);
                }
                else
                {
                    m_Player.PlayerStateMachine.ChangeState(StateMachine.E_STATE.Idle);
                }
            }
        }
        public override void OnCollisionStay2D(Collision2D collision) { }
        public override void OnCollisionExit2D(Collision2D collision) { }
        public override void OnTriggerEnter2D(Collider2D collision) { }
        public override void OnTriggerStay2D(Collider2D collision) { }
        public override void OnTriggerExit2D(Collider2D collision) { }
        public override void StateEnter()
        {
            m_Player.Animator.SetBool("isHurt", true);


            if (m_Player.Sprite.flipX == true)
            {
                Vector2 KnockBack = new Vector2(m_Player.PlayerKnockBackForceX, m_Player.PlayerKnockBackForceY) * Time.deltaTime;
                m_Player.Rigid2D.velocity = KnockBack;
            }
            else if (m_Player.Sprite.flipX == false)
            {
                Vector2 KnockBack = new Vector2(-m_Player.PlayerKnockBackForceX, m_Player.PlayerKnockBackForceY) * Time.deltaTime;
                m_Player.Rigid2D.velocity = KnockBack;
            }

            PlayerHPManager.Instance.PlayerDamage(MonsterHPManager.Instance.Atk(m_Player.m_Monster));
        }
        public override void StateExit()
        {
            m_Player.Animator.SetBool("isHurt", false);
        }
        public override void StateFixedUpdate() { }
    }

    public class ClimbState : BaseState
    {
        private Player m_Player;
        
        public ClimbState(Player player)
        {
            m_Player = player;
        }
        public override void OnCollisionEnter2D(Collision2D collision) { }
        public override void OnCollisionExit2D(Collision2D collision) { }
        public override void OnCollisionStay2D(Collision2D collision) { }
        public override void OnTriggerEnter2D(Collider2D collision) { }
        public override void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.CompareTag("ladder"))
            {
                m_Player.PlayerStateMachine.ChangeState(StateMachine.E_STATE.Run);
            }
        }
        public override void OnTriggerStay2D(Collider2D collision)
        {
            if (collision.CompareTag("ladder"))
            {
                m_Player.Rigid2D.gravityScale = 0;
            }
        }
        public override void StateEnter()
        {
            m_Player.Ladder = true;
            m_Player.Animator.SetBool("isClimb", true);   
        }

        public override void StateExit()
        {
            m_Player.Animator.SetBool("isClimb", false);
            m_Player.Rigid2D.gravityScale = 1.0f;
            m_Player.LadderInput = Vector2.zero; //얘도 Get이니까 값이 당연히 남을 수 밖에 없다.
            m_Player.Ladder = false;
        }
        public override void StateFixedUpdate()
        {
            if (m_Player.Ladder)
            {
                Vector2 PlayerMove = new Vector2(m_Player.Input.x * m_Player.PlayerSpeed * Time.deltaTime, m_Player.LadderInput.y*5.0f* Time.deltaTime);
                m_Player.transform.Translate(PlayerMove);
            }
        }
    }
}
