using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemPanel : MonoBehaviour
{
    [SerializeField] private Image m_ItemImage;
    [SerializeField] private TMP_Text m_ItemquantityText;

    public int m_ItemQuantity;
    public Sprite m_ItemSprite;
    public GameObject m_SelectedShader;

    private ItemInventoryManager m_ItemInventoryManager;

    private void Start()
    {
        m_ItemInventoryManager = GameObject.Find("ItemInventoryCanvas").GetComponent<ItemInventoryManager>();
    }
    
    public void PanelAddItem(int quantity, Sprite sprite)
    {
        m_ItemSprite = sprite;
        m_ItemImage.sprite = sprite;
        m_ItemQuantity += quantity;

        m_ItemquantityText.text = m_ItemQuantity.ToString();
        m_ItemquantityText.enabled = true;
    }

}
