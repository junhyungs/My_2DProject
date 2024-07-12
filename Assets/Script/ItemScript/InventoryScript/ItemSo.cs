using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu] //에셋을 만들 수 있는 메뉴를 제공한다.
public class ItemSo : ScriptableObject
{
    public string m_ItemName; //아이템 이름
    public StatChange m_StatChange = new StatChange(); // 스탯을 바꾸는 이넘
    public int amountChangeStat; //변경할 스탯의 수

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
    }; //여기서 사용하는 enum은 ;이 붙음.

    public enum AttributesChange
    {
        none,
        Strength,
        defense
    };
}
