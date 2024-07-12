using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveBlock_positionY : MonoBehaviour
{
    float m_MoveSpeed = 2.0f;
    float m_MoveDir = 1.0f;

    Vector3 m_StartPosition;

    void Start()
    {
        m_StartPosition = transform.position;
    }

    private void FixedUpdate()
    {
        if (Vector2.Distance(m_StartPosition, transform.position) > 5.0f)
            m_MoveDir *= -1.0f;
        if (m_MoveDir < 0 && transform.position == m_StartPosition)
            m_MoveDir *= -1.0f;

        Vector2 move = new Vector2(0,m_MoveDir) * m_MoveSpeed * Time.deltaTime;
        transform.Translate(move);
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
