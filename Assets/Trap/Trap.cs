using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Trap : MonoBehaviour
{
    //float m_MoveSpeed = 1.0f;
    //float m_Dir = 1.0f;
    //Vector2 m_StartPosition;

    private Player m_Player;


    void Start()
    {
        //m_StartPosition = transform.position;
    }

    
    void Update()
    {
        //if (Vector2.Distance(m_StartPosition, transform.position) > 0.5f)
        //    m_Dir *= -1.0f;
        //Vector2 move = new Vector2(0, m_Dir) * m_MoveSpeed * Time.deltaTime;
        //transform.Translate(move);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            m_Player = collision.gameObject.GetComponent<Player>();

            if (m_Player.Sprite.flipX == false)
            {
                Vector2 KnockBack = new Vector2(-200.0f,200.0f) * Time.deltaTime;
                m_Player.Rigid2D.velocity = KnockBack;  
            }
                


            PlayerHPManager.Instance.PlayerDamage(20);
        }
    }
}
