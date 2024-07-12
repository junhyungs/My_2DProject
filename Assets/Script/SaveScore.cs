using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;
using UnityEngine;
using Unity.VisualScripting;

public class SaveScore : Singleton<SaveScore>
{
    private int m_ItemScore;
    private int m_StageScore;
    private void Awake()
    {
        var scorecanvas = FindObjectsOfType<SaveScore>();

        if(scorecanvas.Length == 1)
        {
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        m_ItemScore = 0;
        m_StageScore = 0;
    }

    public int SaveScores
    {
        get { return m_ItemScore; }
        set { m_ItemScore = value; }
    }

    public int StageScore
    {
        get { return m_StageScore; }
        set { m_StageScore = value; }
    }
}

