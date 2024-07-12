using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemInventoryManager : MonoBehaviour
{

    public GameObject m_IteminventoryMenu;
    private bool m_ItemMenuActivated;
    public ItemSlot[] m_ItemSlot;
    public ItemPanel[] m_ItemPanel;
    public ItemSo[] m_ItemSo;

    void Start()
    {
        
    }

    public bool UseItem(string itemName)
    {
        for (int i = 0; i < m_ItemSo.Length; i++)
        {
            if (m_ItemSo[i].m_ItemName == itemName)
            {
                bool usable = m_ItemSo[i].ItemUse();
                return usable;
            }
        }
        return false;
    }

    public void ItemInventoryActive()
    {
        if (m_ItemMenuActivated)
        {
            m_IteminventoryMenu.SetActive(false);
            m_ItemMenuActivated = false;
        }
        else if (!m_ItemMenuActivated)
        {
            m_IteminventoryMenu.SetActive(true);
            m_ItemMenuActivated = true;
        }
        
    }
    
    public int InventoryManaterAddItem(string itemName, int itemquantity, Sprite itemsprite, string itemdescription, Animator itemAnimator)
    {
        for (int i = 0; i < m_ItemSlot.Length; i++)
        {
            if (m_ItemSlot[i].isItemFull == false && m_ItemSlot[i].m_ItemName == itemName || m_ItemSlot[i].m_Itemquantity == 0)
            {//아이템 슬롯의i번째 오브젝트의 이름이 현재 for문에서 걸린 i번째 오브젝트의 이름과 같거나 아이템 슬롯의 i번째의 수량이 0이라면
                int leftOverItems = m_ItemSlot[i].ItemSlotAddItem(itemName, itemquantity, itemsprite, itemdescription, itemAnimator); //아이템 추가
                if (leftOverItems > 0) // 반환되서 나온 아이템 수량이 0보다 크다면 (아이템이 존재한다는것)
                    leftOverItems = InventoryManaterAddItem(itemName, leftOverItems, itemsprite, itemdescription, itemAnimator);
                    //인벤토리 Add아이템 재귀 : 현재 아이템 수량을 가져가는것이 아니라 남은 수량을 가져가서 반복한다.


                    return leftOverItems;
            }
        }
        return itemquantity; //꽉차면 현재 수량을 리턴
    }

    public void DeselectAllSlot()
    {
        for (int i = 0; i < m_ItemSlot.Length; i++)
        {
            m_ItemSlot[i].m_SelectedShader.SetActive(false);
            m_ItemSlot[i].m_ThisItemSelected = false;
        }
    }
}
