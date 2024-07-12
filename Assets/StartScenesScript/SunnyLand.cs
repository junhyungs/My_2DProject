using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SunnyLand : MonoBehaviour
{
    private TMP_Text m_text;
    private float m_FadeCount;
    void Start()
    {
        m_text = GetComponent<TMP_Text>();
        StartCoroutine(SunnyLandUI());
    }

    
    
    private IEnumerator SunnyLandUI()
    {
        m_FadeCount = 0;

        while(m_FadeCount < 1.0f)
        {
            m_FadeCount += 0.01f;
            yield return new WaitForSeconds(0.01f);
            m_text.color = new Color(1.0f, 1.0f, 1.0f, m_FadeCount);
        }
    }
}
