using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : Singleton<GameManager>
{
    public bool isGameOver = false;
    public bool MenuActivated;
    private CapsuleCollider2D m_Playercollider;
    private GameObject m_Cinemachine;
    private TMP_Text m_TmpText;
    private Text m_TotalScore;
    private GameObject m_GameOverCanvas;
    public GameObject m_Menu;
    private int ItemScore;

    private void Awake()
    {
        m_TmpText = GameObject.Find("ScoreText").GetComponent<TMP_Text>();
        m_Playercollider = GameObject.Find("Player").GetComponent<CapsuleCollider2D>();
        m_Cinemachine = GameObject.Find("PlayerCam");
        m_GameOverCanvas = GameObject.Find("GameOverCanvas").gameObject.transform.GetChild(0).gameObject;
        m_TotalScore = GameObject.Find("GameOverCanvas").gameObject.transform.GetChild(0).transform.GetChild(2).GetComponent<Text>();


        ItemScore = SaveScore.Instance.SaveScores;
        m_TmpText.text = "X " + ItemScore;
        m_GameOverCanvas.SetActive(false);


        Debug.Log("현재 스코어 = "+ ItemScore);
    }



    public void MenuActive()
    {
        if (m_Menu == null)
            return;

        if (MenuActivated)
        {
            m_Menu.SetActive(false);
            MenuActivated = false;
        }
        else if (!MenuActivated)
        {
            m_Menu.SetActive(true);
            MenuActivated = true;
        }
        
    }

   

    public void AddScore(int Score)
    {
        if (!isGameOver)
        {
            ItemScore += Score;
            m_TmpText.text = "X " + ItemScore;
            m_TotalScore.text = ItemScore.ToString();
            SaveScore.Instance.SaveScores = ItemScore;
        }
    }

    public void GameOver()
    {
        isGameOver = true;
        m_Playercollider.enabled = false;
        m_Cinemachine.SetActive(false);
        m_GameOverCanvas.SetActive(true);
    }
}
