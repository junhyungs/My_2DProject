using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public class MonsterHPManager : Singleton<MonsterHPManager>
{
    public Dictionary<Monster, int> m_MonsterHPdic;
    
    private void Awake()
    {
        m_MonsterHPdic = new Dictionary<Monster, int>();
    }

    public void AddMonster(Monster monster, int monsterHp)
    {
        m_MonsterHPdic.Add(monster, monsterHp);
    }


   
    public void Dmg(Monster monster, int dmg)
    {
        if (monster == null)
            return;
        

        m_MonsterHPdic[monster] -= dmg;

        if (m_MonsterHPdic[monster] <= 0)
        {
            StartCoroutine(MonsterDead(monster));
        }
    }
 
    public int Atk(Monster monster)
    {
        return monster.MonsterAtk;
    }

    public IEnumerator MonsterDead(Monster monster)
    {
        if (monster == null)
            yield break;

        BoxCollider2D monstercoll = monster.GetComponent<BoxCollider2D>();
        Rigidbody2D monsterRigid = monster.GetComponent<Rigidbody2D>();

        monsterRigid.gravityScale = 0;
        monstercoll.enabled = false;

        monster.MonsterAnimator.SetTrigger("isDie");

        yield return new WaitForSeconds(0.4f);

        monster.gameObject.SetActive(false);
    }
}
