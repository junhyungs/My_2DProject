using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossHP : Singleton<BossHP>
{
    private Slider m_BossHP;
    private Slimer m_Slimer;
    private void Awake()
    {
        m_BossHP = GameObject.Find("BossHPCanvas").gameObject.transform.GetChild(0).GetComponent<Slider>();
        m_Slimer = GetComponent<Slimer>();

        m_BossHP.maxValue = m_Slimer.BossMaxHP;
        m_BossHP.value = m_Slimer.BossHP;
    }



    public void BossDmg(int dmg)
    {
        m_Slimer.BossHP -= dmg;

        m_BossHP.value = m_Slimer.BossHP;

        if(m_Slimer.BossHP <= 0)
        {
            StartCoroutine(MonsterDead());
        }
    }

    public IEnumerator MonsterDead()
    {
        m_Slimer.MonsterAnimator.SetTrigger("isDie");
        yield return new WaitForSeconds(0.4f);
        gameObject.SetActive(false);
        m_BossHP.gameObject.SetActive(false);
    }
}
