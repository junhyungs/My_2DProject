using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackGroundLoop : MonoBehaviour
{
    private float m_Width;
    private BoxCollider2D m_BoxColloder;

    private void Awake()
    {
        m_BoxColloder = GetComponent<BoxCollider2D>();
        m_Width = m_BoxColloder.size.x;
        Debug.Log(transform.position.x);
    }

    
    void Update()
    {
        if (transform.position.x <= -17.8f)
            Reposition();
            
    }

    private void Reposition()
    {
        Vector2 Offset = new Vector2(17.7f,0);
        transform.position = Offset;
    }
}
