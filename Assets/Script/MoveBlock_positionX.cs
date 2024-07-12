using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveBlock_positionX : MonoBehaviour
{
    private float m_MoveSpeed = 2.0f;
    private float m_Dir = -1.0f;
    private RaycastHit2D[] m_Hit2D;
    private Vector2 m_rayPos;

    void Start()
    {
        
    }


    private void FixedUpdate()
    {

        MoveDir();

        Vector2 move = new Vector2(m_Dir,0) * m_MoveSpeed * Time.deltaTime;
        transform.Translate(move);
    }

    private void MoveDir()
    {
        if(m_Dir < 0)
        {
            m_rayPos = Vector2.left;
        }
        else if(m_Dir > 0)
        {
            m_rayPos = Vector2.right;
        }

        m_Hit2D = Physics2D.RaycastAll(transform.position, m_rayPos, 0.7f);

        foreach(RaycastHit2D item in m_Hit2D)
        {
            if (item.collider.CompareTag("Ground") || item.collider.CompareTag("Wall"))
            {
                m_Dir *= -1.0f;
                return;
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.transform.SetParent(transform);
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.transform.SetParent(null);
        }
    }
}
