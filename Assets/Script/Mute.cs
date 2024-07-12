using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;


public class Mute : MonoBehaviour
{
    public Sprite ClickImg1;
    public Sprite ClickImg2;
    public Button m_Button;
    private bool muted = false;

    public GameObject m_Menu;

    private void Awake()
    {

    }

    public void OnMute()
    {
        if (!muted)
        {
            muted = true;
            m_Button.image.sprite = ClickImg2;
            AudioListener.volume = 0;
            Debug.Log(AudioListener.volume);
        }
        else
        {
            muted = false;
            m_Button.image.sprite = ClickImg1;  
            AudioListener.volume = 1;
            Debug.Log(AudioListener.volume);
        }
    }

    public void OnPlay()
    {
        m_Menu.SetActive(false);
    }
    
    public void OnExit()
    {
        Application.Quit();
    }
}
