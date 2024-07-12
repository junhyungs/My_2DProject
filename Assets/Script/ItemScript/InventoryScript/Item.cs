using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class Item : MonoBehaviour
{
    [SerializeField] private string m_ItemName;
    [SerializeField] private int m_Itemquantity;
    [SerializeField] private Sprite m_Itemsprite;
    private Animator m_ItemAnimator;
    private ItemInventoryManager m_IteminventoryManager;
    bool isItem = true;

    [TextArea]
    [SerializeField] private string m_ItemDescription;

    public int ItemQuantity
    {
        get { return m_Itemquantity; }
        set { m_Itemquantity = value; }
    }
    public string ItemName
    {
        get { return m_ItemName; }
        set { m_ItemName = value; }
    }
    public Sprite ItemSprite
    {
        get { return m_Itemsprite; }
        set { m_Itemsprite = value; }
    }
    public string ItemDescription
    {
        get { return m_ItemDescription; }
        set { m_ItemDescription = value; }
    }

    
    void Start()
    {
        m_IteminventoryManager = GameObject.Find("ItemInventoryCanvas").GetComponent<ItemInventoryManager>();
        m_ItemAnimator = GetComponent<Animator>();
    }

   
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && isItem)
        {   //아이템 수량을 반환 
            isItem = false;
            int leftOverItems = m_IteminventoryManager.InventoryManaterAddItem(m_ItemName, m_Itemquantity, m_Itemsprite, m_ItemDescription,m_ItemAnimator);
            if (leftOverItems <= 0) //아이템 수량이 오버되었는지 0보다 작거나 같다면 오버되지않은거고 0보다 커지면 현재 오버상태
            {
                // 오버상태가 아니면 오브젝트를 파괴할 수 있다.
                StartCoroutine(GetItem());
                
            }
            else
                m_Itemquantity = leftOverItems; // 아니라면 현재 수량을 다시 업데이트.
        }
    }

    private IEnumerator GetItem()
    {

        m_ItemAnimator.SetTrigger("GetItem");
        yield return new WaitForSeconds(0.4f);
        Destroy(gameObject);
        
    }
}
