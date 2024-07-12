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
            {//������ ������i��° ������Ʈ�� �̸��� ���� for������ �ɸ� i��° ������Ʈ�� �̸��� ���ų� ������ ������ i��°�� ������ 0�̶��
                int leftOverItems = m_ItemSlot[i].ItemSlotAddItem(itemName, itemquantity, itemsprite, itemdescription, itemAnimator); //������ �߰�
                if (leftOverItems > 0) // ��ȯ�Ǽ� ���� ������ ������ 0���� ũ�ٸ� (�������� �����Ѵٴ°�)
                    leftOverItems = InventoryManaterAddItem(itemName, leftOverItems, itemsprite, itemdescription, itemAnimator);
                    //�κ��丮 Add������ ��� : ���� ������ ������ �������°��� �ƴ϶� ���� ������ �������� �ݺ��Ѵ�.


                    return leftOverItems;
            }
        }
        return itemquantity; //������ ���� ������ ����
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
