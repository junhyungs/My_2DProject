using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EndText : MonoBehaviour
{
    private TMP_Text m_TotalScoreText;
    private int m_TotalScore;

    void Start()
    {
        m_TotalScoreText = GetComponent<TMP_Text>();
        m_TotalScore = SaveScore.Instance.SaveScores;

        m_TotalScoreText.text = "Gem : " + m_TotalScore;

    }

}
