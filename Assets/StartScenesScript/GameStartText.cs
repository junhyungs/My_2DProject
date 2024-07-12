using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameStartText : MonoBehaviour
{
    private Text m_text;
    private float m_FadeCount;
    void Start()
    {
        m_text = GetComponent<Text>();
        StartCoroutine(StartUI());
    }

    private IEnumerator StartUI()
    {
        m_FadeCount = 0;

        while (m_FadeCount <= 1.0f)
        {
            m_FadeCount += 0.01f;
            yield return new WaitForSeconds(0.01f);
            m_text.color = new Color(1.0f, 1.0f, 1.0f, m_FadeCount);
        }
        StartCoroutine(FadeOut());
    }
    private IEnumerator FadeIn()
    {
        m_FadeCount = 0;

        while (m_FadeCount <= 1.0f)
        {
            m_FadeCount += 0.01f;
            yield return new WaitForSeconds(0.01f);
            m_text.color = new Color(1.0f, 1.0f, 1.0f, m_FadeCount);
        }
        StartCoroutine(FadeOut());
    }

    private IEnumerator FadeOut()
    {
        while(m_FadeCount >= 0)
        {
            m_FadeCount -= 0.01f;
            yield return new WaitForSeconds(0.01f);
            m_text.color = new Color(1.0f, 1.0f, 1.0f, m_FadeCount);
        }
        StartCoroutine(FadeIn());
    }
}
