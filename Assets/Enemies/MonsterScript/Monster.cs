using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class Monster : MonoBehaviour
{
    public enum E_MONSTERSTATE
    {
        Start,
        Idle,
        Move,
        Check,
        Atk
   
    }

    protected float m_MonsterSpeed;
    protected string m_MonsterName;
    protected float m_MonsterJumpForce;
    protected float m_MonsterPosX;
    protected float m_MonsterPosY;
    protected bool isMove;
    protected bool OnTarget;
    protected bool CheckStateMove;
    protected CircleCollider2D m_CircleCollider;
    protected Player m_Player;
    protected Animator m_MonsterAnimator;
    protected Rigidbody2D m_MonsterRigidbody;
    protected SpriteRenderer m_Monstersprite;
    protected StateMachine m_MonsterState;
    protected RaycastHit2D[] m_isWallRay;
    protected RaycastHit2D m_isNullRay;
    protected Vector2 m_isWallRaydirection;
    protected Vector2 m_isNullRaydirection;
    protected Timer m_Timer;
    protected float m_fData;
    public int m_MonsterHP;
    protected int m_MonsterAtk;
    public LayerMask layer;
    
    public Animator MonsterAnimator => m_MonsterAnimator;
    public int MonsterAtk => m_MonsterAtk;

    public void FlipX(Monster monster)
    {
        if (monster.m_MonsterPosX > 0)
        {
            monster.m_Monstersprite.flipX = true;
        }
        else if (monster.m_MonsterPosX < 0)
        {
            monster.m_Monstersprite.flipX = false;
        }
    }
    public void Ray(Monster monster)
    {
        float Ray = 0;

        monster.m_isNullRaydirection = Vector2.down;
         
        if(monster.m_MonsterPosX > 0)
        {
            monster.m_isWallRaydirection = Vector2.right;
            Ray = 1.0f;
        }
        else if(monster.m_MonsterPosX < 0)
        {
            monster.m_isWallRaydirection = Vector2.left;
            Ray = -1.0f;
        }

        Vector2 RayPosition = new Vector2(monster.transform.position.x + Ray, monster.transform.position.y);

        monster.m_isWallRay = Physics2D.RaycastAll(RayPosition, monster.m_isWallRaydirection, 0.5f);
        monster.m_isNullRay = Physics2D.Raycast(RayPosition, monster.m_isNullRaydirection, 1.0f, layer);

        Debug.DrawRay(RayPosition, monster.m_isWallRaydirection * 0.5f, Color.black);
        Debug.DrawRay(RayPosition, monster.m_isNullRaydirection * 1.0f, Color.black);

     
        if (!monster.m_isNullRay)
        {
            monster.m_MonsterPosX *= -1;
            return;
        }
        foreach (RaycastHit2D item in monster.m_isWallRay)
        {
            if (item.collider.CompareTag("Wall") || item.collider.CompareTag("Monster"))
            {
                monster.m_MonsterPosX *= -1;
                return;
            }
        }
    }





}
