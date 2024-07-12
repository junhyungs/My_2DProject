using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;
using UnityEngine.UI;

//public class PlayerHPManager : Singleton<PlayerHPManager>
//{
//    private Player m_Player;

//    public void initPlayer(Player player)
//    {
//        m_Player = player;
//    }

//    public Player HPManager => m_Player;

//    public void PlayerDamage(int dmg)
//    {
//        Debug.Log(m_Player.PlayerHP);
//        m_Player.PlayerHP -= dmg;
//        m_Player.PlayerHpText.text = m_Player.PlayerHP + " / 100";
//        m_Player.PlayerHpSlider.value = m_Player.PlayerHP;

//        if(m_Player.PlayerHP <= 0)
//        {
//            Dead();
//        }
//    }

//    public void PlayerHealth(int healAmount)
//    {
//        if (m_Player.PlayerHP >= 100)
//        {
//            return;
//        }
//        m_Player.PlayerHP += healAmount;
//        m_Player.PlayerHpText.text = m_Player.PlayerHP + " / 100";
//        m_Player.PlayerHpSlider.value = m_Player.PlayerHP;
//    }

//    public void Dead()
//    {
//        GameManager.Instance.GameOver();
//    }
//}

public class PlayerHPManager : Singleton<PlayerHPManager>
{
    private Slider m_Hpslider;
    private Text m_HpText;
    private int m_PlayerHp;
    private int m_PlayerMaxHp;
    public Slider Hpslider
    {
        get { return m_Hpslider; }
        set { m_Hpslider = value; } 
    }
    public Text HpText
    {
        get { return m_HpText; }
        set { m_HpText = value; }
    }
    public int PlayerHp
    {
        get { return m_PlayerHp; }
        set { m_PlayerHp = value; }
    }
    public int PlayerMaxHp
    {
        get { return m_PlayerMaxHp; }
        set { m_PlayerMaxHp = value;}
    }

    private void Awake()
    {
        m_Hpslider = GameObject.Find("Slider").GetComponent<Slider>();  
        m_HpText = GameObject.Find("HP_Point").GetComponent<Text>();

        m_PlayerHp = 100;
        m_PlayerMaxHp = 100;

        m_Hpslider.maxValue = m_PlayerMaxHp;
        m_Hpslider.value = m_PlayerHp;
        m_HpText.text = m_PlayerMaxHp + " / 100";
    }


    public void PlayerDamage(int dmg)
    {
        Debug.Log(m_PlayerHp);
        m_PlayerHp -= dmg;
        if(m_PlayerHp >= 0)
        {
            m_HpText.text = m_PlayerHp + " / 100";
        }
        else
        {
            m_HpText.text = "0 / 100";
        }
        
        m_Hpslider.value = m_PlayerHp;

        if (m_PlayerHp <= 0)
        {
            Dead();
        }
    }

    public void PlayerHealth(int healAmount)
    {
        m_PlayerHp += healAmount;
        m_HpText.text = m_PlayerHp + " / 100";
        m_Hpslider.value = m_PlayerHp;
    }

    public void Dead()
    {
        GameManager.Instance.GameOver();
    }
}