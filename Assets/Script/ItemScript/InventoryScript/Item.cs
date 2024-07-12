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
        {   //������ ������ ��ȯ 
            isItem = false;
            int leftOverItems = m_IteminventoryManager.InventoryManaterAddItem(m_ItemName, m_Itemquantity, m_Itemsprite, m_ItemDescription,m_ItemAnimator);
            if (leftOverItems <= 0) //������ ������ �����Ǿ����� 0���� �۰ų� ���ٸ� �������������Ű� 0���� Ŀ���� ���� ��������
            {
                // �������°� �ƴϸ� ������Ʈ�� �ı��� �� �ִ�.
                StartCoroutine(GetItem());
                
            }
            else
                m_Itemquantity = leftOverItems; // �ƴ϶�� ���� ������ �ٽ� ������Ʈ.
        }
    }

    private IEnumerator GetItem()
    {

        m_ItemAnimator.SetTrigger("GetItem");
        yield return new WaitForSeconds(0.4f);
        Destroy(gameObject);
        
    }
}
