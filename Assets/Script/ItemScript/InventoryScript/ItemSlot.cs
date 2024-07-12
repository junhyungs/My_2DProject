using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;


public class ItemSlot : MonoBehaviour, IPointerClickHandler
{
    public string m_ItemName;
    public int m_Itemquantity;
    public Sprite m_Itemsprite;
    public bool isItemFull;
    public string m_ItemDescription;
    public Sprite m_EmptySprite;
    public Animator m_ItemAnimator;

    [SerializeField] private TMP_Text m_ItemquantityText;
    [SerializeField] private Image m_Itemimage;
    [SerializeField] private int maxNumberOfItems;

    public Image m_ItemDescriptionImage;
    public TMP_Text m_ItemDescriptionNameText;
    public TMP_Text m_ItemDescriptionText;

    public GameObject m_SelectedShader;
    public bool m_ThisItemSelected;

    private ItemInventoryManager m_InventoryManager;

    private void Start()
    {
        m_InventoryManager = GameObject.Find("ItemInventoryCanvas").GetComponent<ItemInventoryManager>();
    }

    public int ItemSlotAddItem(string itmName, int quantity,Sprite itemsprite, string itemdescription,Animator itemAnimator)
    {
        if (isItemFull)
            return m_Itemquantity; //꽉차면 현재수량 리턴

        m_ItemName = itmName;

        m_Itemsprite = itemsprite;
        m_Itemimage.sprite = itemsprite;
        m_ItemAnimator = itemAnimator;

        m_ItemDescription = itemdescription;

        m_Itemquantity += quantity; //현재 아이템 수량에 들어온 수량을 더해준다.

        if (m_Itemquantity >= maxNumberOfItems) // 최대 수량보다 높아진다면
        {
            m_ItemquantityText.text = maxNumberOfItems.ToString(); //수량텍스트에 최대 수량을 표기하고
            m_ItemquantityText.enabled = true; //텍스트를 활성화한다.
            isItemFull = true;

            int extraItems = m_Itemquantity - maxNumberOfItems;
            m_Itemquantity = maxNumberOfItems;
            return extraItems;
        }
        m_ItemquantityText.text = m_Itemquantity.ToString();
        m_ItemquantityText.enabled = true;

        return 0;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if(eventData.button == PointerEventData.InputButton.Left)
        {
            OnLeftClick();
        }
        if(eventData.button == PointerEventData.InputButton.Right)
        {
            OnRightClick();
        }
    }
    public void OnLeftClick()
    {
        if (m_ThisItemSelected)
        {
            bool usable = m_InventoryManager.UseItem(m_ItemName);
            if (usable)
            {
                m_Itemquantity -= 1; // 아이템을 사용하면 슬롯수량을 1 감소
                m_ItemquantityText.text = m_Itemquantity.ToString(); //아이템 수량텍스트 업데이트
                if (m_Itemquantity <= 0) //아이템이 없으면
                    EmptySlot(); //빈슬롯으로 바꾼다.
            }
        }
        else
        {
            m_InventoryManager.DeselectAllSlot();
            m_SelectedShader.SetActive(true);
            m_ThisItemSelected = true;
            m_ItemDescriptionNameText.text = m_ItemName;
            m_ItemDescriptionText.text = m_ItemDescription;
            m_ItemDescriptionImage.sprite = m_Itemsprite;

            if (m_ItemDescriptionImage.sprite == null)
            {
                m_ItemDescriptionImage.sprite = m_EmptySprite;
            }
        }
            
        
    }
    public void OnRightClick()
    {
        GameObject ItemToDrop = new GameObject(m_ItemName);
        Item newItem = ItemToDrop.AddComponent<Item>();
        newItem.ItemQuantity = 1;
        newItem.ItemName = m_ItemName;
        newItem.ItemSprite = m_Itemsprite;
        newItem.ItemDescription = m_ItemDescription;

        SpriteRenderer newSprite = ItemToDrop.AddComponent<SpriteRenderer>();
        newSprite.sprite = m_Itemsprite;
        newSprite.sortingOrder = 5;
        newSprite.sortingLayerName = "Ground";

        ItemToDrop.AddComponent<BoxCollider2D>();
        BoxCollider2D boxCollider = ItemToDrop.GetComponent<BoxCollider2D>();
        boxCollider.isTrigger = true;
        ItemToDrop.transform.position = GameObject.FindWithTag("Player").transform.position + new Vector3(1.0f, 0, 0);
        ItemToDrop.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);

        ItemToDrop.AddComponent<Animator>();
        

        m_Itemquantity -= 1; 
        m_ItemquantityText.text = m_Itemquantity.ToString(); 
        if (m_Itemquantity <= 0)
            EmptySlot();
    }

    private void EmptySlot()
    {
        m_ItemquantityText.enabled = false; //슬롯 수량 텍스트 비활성화
        m_Itemimage.sprite = m_EmptySprite;

        m_ItemDescriptionNameText.text = "";
        m_ItemDescriptionText.text = "";
        m_ItemDescriptionImage.sprite = m_EmptySprite;
    }

}
