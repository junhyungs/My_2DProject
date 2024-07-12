using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneMgr : Singleton<SceneMgr>
{
    private float m_FadeCount;
    public int m_SceneNumber = 0;
    public Image m_FadeImg;
    private bool m_isNext = true;

    private void Awake()
    {
        var NextScene = FindObjectsOfType<SceneMgr>();

        if(NextScene.Length == 1)
        {
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void NextScene()
    {
        if (m_isNext)
        {
            m_isNext = false;
            StartCoroutine(FadeIn());
        }
    }

    public void End()
    {
        SaveScore.Instance.SaveScores = 0;
        PlayerHPManager.Instance.PlayerHp = 100;
        PlayerHPManager.Instance.Hpslider.value = PlayerHPManager.Instance.PlayerHp;
        PlayerHPManager.Instance.HpText.text = PlayerHPManager.Instance.PlayerHp + "/ 100";
        m_SceneNumber = 0;
        if (m_isNext)
        {
            m_isNext = false;
            StartCoroutine(FadeIn());
        }
    }
    public void Regame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        SaveScore.Instance.SaveScores = 0;
        PlayerHPManager.Instance.PlayerHp = 100;
        PlayerHPManager.Instance.Hpslider.value = PlayerHPManager.Instance.PlayerHp;
        PlayerHPManager.Instance.HpText.text = PlayerHPManager.Instance.PlayerHp + "/ 100";
    }

    private IEnumerator FadeIn()
    {
        m_FadeCount = 0;

        while (m_FadeCount <= 1.0f)
        {
            m_FadeCount += 0.01f;
            yield return new WaitForSeconds(0.02f);
            m_FadeImg.color = new Color(0, 0, 0, m_FadeCount);
        }

        m_SceneNumber++;
        SceneManager.LoadScene(m_SceneNumber);
        Debug.Log(m_SceneNumber);
        StartCoroutine(FadeOut());
    }

    private IEnumerator FadeOut()
    {
        Debug.Log("FadeOut");
        m_FadeCount = 1;

        yield return new WaitForSeconds(1.5f);

        while(m_FadeCount >= 0)
        {
            m_FadeCount -= 0.01f;
            yield return new WaitForSeconds(0.05f);
            m_FadeImg.color = new Color(0, 0, 0, m_FadeCount);
        }
        m_isNext = true;
    }
}
