using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Opossum : Monster
{
    public Opossum()
    {
        m_MonsterHP = 20;
        m_MonsterSpeed = 3.0f;
        m_MonsterAtk = 5;
    }
    private void Awake()
    {
        m_MonsterAnimator = GetComponent<Animator>();
        m_Monstersprite = GetComponent<SpriteRenderer>();
        m_MonsterRigidbody = GetComponent<Rigidbody2D>();
        MonsterHPManager.Instance.AddMonster(this, m_MonsterHP);

        StartCoroutine(MoveDir());
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            MonsterHPManager.Instance.StartCoroutine(MonsterHPManager.Instance.MonsterDead(this));
        }
    }

    private void FixedUpdate()
    {
        FlipX(this);
        Ray(this);

        Vector2 Move = new Vector2(m_MonsterPosX, m_MonsterRigidbody.velocity.y).normalized * m_MonsterSpeed * Time.deltaTime;
        transform.Translate(Move);
    }

    private IEnumerator MoveDir()
    {
        while (true)
        {
            m_MonsterPosX = Random.Range(-1.0f, 1);
            yield return new WaitForSeconds(4.0f);
        }
    }

}
