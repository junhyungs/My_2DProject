using System.Collections;
using System.Collections.Generic;
//using UnityEditor.Rendering;
using UnityEngine;

public class FireBall : MonoBehaviour
{
    private float FireballSpeed = 700.0f;
    [SerializeField] int FireballDmg;
    private Animator m_FireBallAnimator;
    private Rigidbody2D m_FireballRigidbody2D;
    private bool isCollided = false; //파이어볼의 움직임 제어
    private Player m_Player;
    
    private Vector2 m_Dir;

    private SpriteRenderer m_PlayerSpriteRenderer;
    public Animator FireBallAnimator => m_FireBallAnimator;
    public Rigidbody2D FireBallRigid2D => m_FireballRigidbody2D;

    private void Awake()
    {
        m_FireballRigidbody2D = GetComponent<Rigidbody2D>();
        m_FireBallAnimator = GetComponent<Animator>();
    }
    private void OnEnable()
    {
        m_Player = GameObject.Find("Player").GetComponent<Player>();
        m_PlayerSpriteRenderer = m_Player.Sprite;



        if (m_PlayerSpriteRenderer.flipX == true)
        {
            m_Dir = Vector2.left;
            transform.localScale = new Vector3(-0.2f, 0.2f, 0.2f);
        }
        else if(m_PlayerSpriteRenderer.flipX == false)
        {
            m_Dir = Vector2.right;
            transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
        }

        StartCoroutine(ReturnDelay());
    }

    private void FixedUpdate()
    {
        FireBallMove();
    }

    public void FireBallMove()
    {
        if (!isCollided)
        {
            m_FireballRigidbody2D.velocity = m_Dir * FireballSpeed * Time.deltaTime;
        }
    }

    //return pool 메소드 작성
    private void ReturnFireBall()
    {
        FireBallPool.Instance.ReturnFireBall(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!isCollided && collision.gameObject.CompareTag("Monster"))
        {
            Monster monster = collision.gameObject.GetComponent<Monster>();
            MonsterHPManager.Instance.Dmg(monster, FireballDmg);
            m_FireballRigidbody2D.velocity = Vector2.zero;
            isCollided = true;
            StartCoroutine(Explosions());
        }
        else if (!isCollided && collision.gameObject.CompareTag("Ground"))
        {
            StartCoroutine(Explosions());
        }
        else if(!isCollided && collision.gameObject.CompareTag("Wall"))
        {
            StartCoroutine(Explosions());
        }
        else if(!isCollided && collision.gameObject.tag == "Boss")
        {
            BossHP.Instance.BossDmg(FireballDmg);
            m_FireballRigidbody2D.velocity = Vector2.zero;
            isCollided = true;
            StartCoroutine(Explosions());
        }
            
    }

    private IEnumerator Explosions()
    {
        m_FireBallAnimator.SetTrigger("Explosions");

        yield return new WaitForSeconds(0.5f);

        isCollided = false;
        ReturnFireBall();
        
    }

    private IEnumerator ReturnDelay()
    {
        yield return new WaitForSeconds(4.0f);

        if (!isCollided)
        {
            ReturnFireBall();
        }
            
    }
}
