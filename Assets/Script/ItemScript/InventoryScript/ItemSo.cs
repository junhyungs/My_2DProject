using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu] //������ ���� �� �ִ� �޴��� �����Ѵ�.
public class ItemSo : ScriptableObject
{
    public string m_ItemName; //������ �̸�
    public StatChange m_StatChange = new StatChange(); // ������ �ٲٴ� �̳�
    public int amountChangeStat; //������ ������ ��

    public AttributesChange m_AttributesChange = new AttributesChange();

    public bool ItemUse()
    {
        if(m_StatChange == StatChange.Health)
        {
            if(PlayerHPManager.Instance.PlayerHp == PlayerHPManager.Instance.PlayerMaxHp)
            {
                return false;
            }
            else if(PlayerHPManager.Instance.PlayerHp + amountChangeStat > PlayerHPManager.Instance.PlayerMaxHp)
            {
                amountChangeStat = PlayerHPManager.Instance.PlayerHp + amountChangeStat - PlayerHPManager.Instance.PlayerMaxHp;
                PlayerHPManager.Instance.PlayerHealth(amountChangeStat);
            }
            else
            {
                PlayerHPManager.Instance.PlayerHealth(amountChangeStat);
                return true;
            }
            
        }
        return false;
    }

    public enum StatChange
    {
        none,
        Health,
        Mana
    }; //���⼭ ����ϴ� enum�� ;�� ����.

    public enum AttributesChange
    {
        none,
        Strength,
        defense
    };
}
