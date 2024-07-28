//#define DEBUG_MODE

using UnityEngine;
using TMPro;
using System.Collections.Generic;
using System;

/// <summary>
/// ������ҵı��������а�����Ʒ��Ԫ��������ʾ��ˢ�£�ͬʱ�޸ı��������ı�
/// </summary>
public class BagManager : MonoBehaviour
{
    /// <summary>
    /// Bag_Manager�ĵ���ʵ��
    /// </summary>
    static BagManager Instance;

    /// <summary>
    /// ����UI������
    /// </summary>
    public GameObject Grid = null;

    /// <summary>
    /// ��ƷUI��Ԥ���壬������Ʒ�������Ϊģ���ڱ����н�����ʾ
    /// </summary>
    public ItemUIControl ItemPrefab = null;

    /// <summary>
    /// �����ĵ�ǰ��Ʒ��ռ��Ԫ�������ı�
    /// </summary>
    public TMP_Text CapeCountWord = null;

    /// <summary>
    /// ���ݿ����ű�
    /// </summary>
    public DatabaseManager dbManager = null;

    /// <summary>
    /// ���ڴ洢��Ʒ���ݵ��б�һ���������ѵ���Χ�ڵ���ƷΪһ��Ԫ��
    /// </summary>
    private List<ItemTable> ItemsTableList = new List<ItemTable>();

    /// <summary>
    /// ���ڴ洢��ƷUI���б�������ƷUI���˳������ƷIDһһ��Ӧ��ȷ�����Ը�����ƷID��ȷ����UI
    /// ������ƷUIͨ��ScriptableObject��ǰ���ɺ��ļ��洢�ڱ��أ���Ҫʱ������б�������ȡ��Ӧ��ƷUI
    /// </summary>
    public List<ItemUI> ItemsUIList = new List<ItemUI>();

    /// <summary>
    /// �������Ԫ������Ĭ��Ϊ30
    /// </summary>
    public int BagMax = 30;

    //ȷ��ֻ��һ��BagMangerʵ��
    private void Awake()
    {
        if (Instance != null)
            Destroy(this);
        Instance = this;
#if DEBUG_MODE
        Debug.Log("BagManager�ű�ִ��Awake()");
#endif
    }

    private void OnEnable()
    {
        RefreshItemUI();
#if DEBUG_MODE
        Debug.Log("BagManager�ű�ִ��OnEnable()");
#endif
    }

    private void Start()
    {
        //�Ƚ��г�ʼ�����
        BagDeBug();
        //��ȡ���ݿ����ȫ������
        Instance.ItemsTableList = Instance.dbManager.GetAllItems();
        RefreshItemUI();
#if DEBUG_MODE
        Debug.Log("����������Ʒ���£�");
        foreach (var item in Instance.ItemsTableList)
        {
            Debug.Log("ID: " + item.Id + ", Num: " + item.ItemNum + ", Type: " + item.ItemType);
        }
        Debug.Log("BagManager�ű�ִ��Start()");
#endif
    }

    /// <summary>
    /// �򱳰�����ʾ��ƷUI
    /// ���˼·�����趨�õ�Grid����������Ԥ����ģ�棬ʹ������������У�Ԥ����ģ���ٸ��ݲ�ͬ��Ʒ�滻��Ӧ����UI���γɲ�ͬ��ƷUI
    /// </summary>
    /// <param name="Item">��Ӧ��Ʒ����</param>
    private static void AddItemOnUI(ItemTable Item)
    {
        //��Ԥ����Ϊģ������һ��Grid��������
        ItemUIControl ItemUIC = Instantiate(Instance.ItemPrefab, Instance.Grid.transform);
        //��ģ��Ķ�Ӧ�����滻Ϊʵ����Ʒ��Ӧ����
        ItemUIC.Item = Instance.ItemsUIList[Item.Id];
        ItemUIC.ItemName.text = Instance.ItemsUIList[Item.Id].ItemName;
        ItemUIC.ItemImage.sprite = Instance.ItemsUIList[Item.Id].ItemImage;
        ItemUIC.ItemNum.text = Item.ItemNum.ToString();
    }

    /// <summary>
    /// �򱳰�����ָ����������Ʒ
    /// ���˼·��ͨ����Ҫ��ӵ���Ʒ�������������ѵ��������Ϊ���ѵ�����ʣ�������������ѵ�������һ����Ԫ�أ�ֱ�Ӳ�����Ʒ�б������ݱ��ɣ�ʣ���������б�Ѱ��δ����Ʒ���ӽ�ȥ
    /// �����������ʣ�࣬��ʣ�ಿ������һ����Ԫ�ز�����Ʒ�б������ݱ�
    /// </summary>
    /// <param name="Item">Ҫ���ӵ���Ʒ</param>
    /// <param name="AddNum">���ӵ�����</param>
    public static void AddItemsOnTable(ItemUI Item,int AddNum)
    {
        //�������������ò���
        if (Instance.ItemsTableList.Count < Instance.BagMax)
        {
            //ȷ���������Ʒ�󱳰����������ᳬ�����������������������Ӧ��Ʒ����+���������/��Ӧ��Ʒ���ѵ���������ȡ�������Ӻ��Ӧ��Ʒ��ռ�õĸ��������ټ�ȥδ���ǰ��Ӧ��Ʒռ�õĸ��������Դ˻���������Ʒ��Ҫ�ĸ�����
            //���ټ����ֱ������ø���������������������Ʒ�󱳰����ø�����
            if ((int)Math.Ceiling((double)(GetItemSum(Item.ItemId) + AddNum) / Item.ItemMax) - GetItemGridSum(Item.ItemId) + GetBagNowCapa() <= Instance.BagMax)
            {
                //�����������ѵ�����������
                int x = AddNum / Item.ItemMax;
#if DEBUG_MODE
                Debug.Log("������Ԫ������" + x);
#endif
                //ʣ����������Ŀ
                int y = AddNum - Item.ItemMax * x;
                //�ȴ���ʣ��������Ŀ������������ѵ����ݺ�����ѭ������
                //y=0��˵��û��ʣ�࣬��Ŀ����Ϊ���ѵ����ı���������ר�Ŵ���ֱ�Ӳ������ѵ����ݾ���
                if (y != 0)
                {
                    foreach (ItemTable ItemTable in Instance.ItemsTableList)
                    {
                        //ͨ��IDѰ�Ҷ�Ӧ���壬�ұȽϵ�ǰ�����Ƿ񳬹����ѵ�������δ��������ʣ�������в���
                        if (ItemTable.Id == Item.ItemId && ItemTable.ItemNum < Item.ItemMax)
                        {
                            //���С�ڵ��ڶѵ����ֵ��ֱ��ȫ���ӽ�ȥ���У����÷ֿ�����
                            if (ItemTable.ItemNum + y <= Item.ItemMax)
                            {
                                UpdateItemList(ItemTable, ItemTable.ItemNum + y);
                                y = 0;
                            }
                            //��Ӵ������ѵ��������账����������Ĳ���
                            else
                            {
                                y -= Item.ItemMax - ItemTable.ItemNum;
                                UpdateItemList(ItemTable, Item.ItemMax);
                            }
                            //��Ϊ�������ѵ����Ķ�Ӧ��Ʒ���ֻ��һ�����Ͼ������������˲Ż�ʣ����һ�������Բ��ؼ���ѭ��
                            break;
                        }
                    }
                    //�������ʣ�µĲ���
                    if (y != 0) AddItemList(Item, y);
                }
                //�������ѵ����ݲ���
                for (int i = 0; i < x; i++) AddItemList(Item, Item.ItemMax);
                //ˢ�±���״̬
                RefreshItemUI();
#if DEBUG_MODE
                Debug.Log("�ɹ����" + AddNum + "��" + Item.ItemName);
#endif
            }
#if DEBUG_MODE
            else Debug.Log("������Ʒ������������");
#endif
        }
#if DEBUG_MODE
        else Debug.Log("��������");
#endif
    }

    /// <summary>
    /// �ӱ����м���ָ����������Ʒ
    /// ���˼·��ͨ����Ҫ���ٵ���Ʒ�������������ѵ��������Ϊ���ѵ�����ʣ����������ֱ��ɾ�����ѵ���Ԫ������Ӧ�������ѵ�����ʣ������Ѱ��δ����ƷԪ������������������������Ϊ0��ɾ����Ԫ��
    /// ����ʣ������������ʣ�������Ѱ��δ����ƷԪ���ټ�һ�μ���
    /// </summary>
    /// <param name="Item">Ҫ���ٵ���Ʒ</param>
    /// <param name="SubNum">���ٵ�����</param>
    public static void SubItemsOnTable(ItemUI Item,int SubNum)
    {
        //��Ʒ�������ڵ���Ҫ���ٵ������Ž��в���
        int Residue = GetItemSum(Item.ItemId) - SubNum;
        if (Residue >= 0)
        {
            //����Ҫ�Ƴ����ٸ���Ʒ��Ԫ��
            int SubGridNum = SubNum / Item.ItemMax;
#if DEBUG_MODE
            Debug.Log("�Ƴ���Ԫ������" + SubGridNum);
#endif
            //����ʣ���޷�����һ����Ԫ�������Ϊ����
            int z = SubNum - Item.ItemMax * SubGridNum;
            //��ǰ���������Ƴ���Ԫ��
            for (int i = 0, j = 0; i < Instance.ItemsTableList.Count && j < SubGridNum; i++)
            {
                if (Instance.ItemsTableList[i].Id == Item.ItemId)
                {
                    DeleteItemList(Instance.ItemsTableList[i]);
                    //�����б�����
                    i--;
                    j++;
                }
            }
            //������ಿ��
            if (z != 0)
            {
                //��λ���һ����Ӧ��Ʒ��λ��
                ItemTable ExistingItem = Instance.ItemsTableList.FindLast(i => i.Id == Item.ItemId);
                if (ExistingItem.ItemNum - z > 0) UpdateItemList(ExistingItem, ExistingItem.ItemNum - z);
                else
                {
                    z -= ExistingItem.ItemNum;
                    DeleteItemList(ExistingItem);
                    if (z > 0)
                    {
                        ExistingItem = Instance.ItemsTableList.FindLast(i => i.Id == Item.ItemId);
                        UpdateItemList(ExistingItem, ExistingItem.ItemNum - z);
                    }
                }
            }
#if DEBUG_MODE
            Debug.Log("�ɹ�������"+Item.ItemName+SubNum+"��");
#endif
            RefreshItemUI();
        }
#if DEBUG_MODE
        else Debug.Log("����������������Ӧ��Ʒ��");
#endif
    }

    /// <summary>
    /// ˢ�±���״̬��ʹ��Ʒ���ӻ����ʱ��������ʾ��Ʒ��������������Ʒ˳��
    /// ���˼·��ֱ��ɾ��Grid�����µ����������弴������������ƷUI���ٶ���Ʒ�б�ID��С�����������ID��ͬ�������Ӵ�С��������������������ƷUI���Դ�ˢ����Ʒ����Ϣ�뱳����Ʒ����
    /// </summary>
    private static void RefreshItemUI()
    {
        //����������������������Ʒȫ��ɾ��
        for (int i = 0; i < Instance.Grid.transform.childCount; i++)
        {
            //ɾ����ǰ��Ʒ
            Destroy(Instance.Grid.transform.GetChild(i).gameObject);
        }
        //��ID��С������б�����������ID��ͬ��ItemNum�Ӵ�С��������
        Instance.ItemsTableList.Sort((x, y) =>
        {
            int IdCompare = x.Id.CompareTo(y.Id);
            if (IdCompare == 0)
            {
                return y.ItemNum.CompareTo(x.ItemNum);
            }
            return IdCompare;
        });
        //�ٽ�ˢ�º����Ʒ��ӻر���
        for (int i = 0; i < Instance.ItemsTableList.Count; i++)
        {
            AddItemOnUI(Instance.ItemsTableList[i]);
        }
        //�������б���Ԫ������00����ʽת��Ϊ�ַ��������ı�
        Instance.CapeCountWord.text = Instance.ItemsTableList.Count.ToString("D2");
#if DEBUG_MODE
        Debug.Log("����״̬��ˢ��");
#endif
    }

    /// <summary>
    /// �������ݱ��޸��б���Ʒ����
    /// </summary>
    /// <param name="itemTable">Ҫ���µ�����</param>
    /// <param name="UpdateNum">�޸ĺ������</param>
    private static void UpdateItemList(ItemTable itemTable,int UpdateNum)
    {
        itemTable.ItemNum = UpdateNum;
        Instance.dbManager.UpdateItem(itemTable);
    }

    /// <summary>
    /// �����ݱ���б���������
    /// </summary>
    /// <param name="item">��������Ʒ</param>
    /// <param name="AddNum">����������</param>
    private static void AddItemList(ItemUI item,int AddNum)
    {
        ItemTable NewItemTable = new ItemTable { Id = item.ItemId, ItemNum = AddNum, ItemType = item.Type.ToString() };
        Instance.ItemsTableList.Add(NewItemTable);
        Instance.dbManager.InsertItem(NewItemTable);
#if DEBUG_MODE
        Debug.Log("�������ݣ�Id:" + item.ItemId + " ItemNum:" + AddNum + " ItemType:" + item.Type.ToString());
#endif
    }

    /// <summary>
    /// ɾ�����ݱ��б��ڵ���Ʒ��Ϣ
    /// </summary>
    /// <param name="itemTable">Ҫɾ������Ʒ��Ϣ</param>
    private static void DeleteItemList(ItemTable itemTable)
    {
        //�������ݱ����ɾ������ֹɾ���б����ݺ��б�˳��仯����ɳ���
        Instance.dbManager.DeleteItem(itemTable);
        Instance.ItemsTableList.Remove(itemTable);
#if DEBUG_MODE
        Debug.Log("�Ѵ��б��Ƴ�����");
#endif
    }

    /// <summary>
    /// ���ڻ�ȡ����ĳ��Ʒ����
    /// </summary>
    /// <param name="ItemId">��Ҫ��ȡ��������Ʒ</param>
    /// <returns>����Ʒ������������</returns>
    public static int GetItemSum(int ItemId)
    {
        int Record = 0;
        foreach (ItemTable Item in Instance.ItemsTableList)
        {
            if (Item.Id==ItemId)
            {
                Record += Item.ItemNum;
            }
        }
        return Record;
    }

    /// <summary>
    /// ���ص�ǰ�������ø�����
    /// </summary>
    /// <returns>���ر������ø�������������</returns>
    public static int GetBagNowCapa()
    {
        return Instance.ItemsTableList.Count;
    }

    /// <summary>
    /// ���ڻ�ȡĳ��Ʒ��ռ�ĸ�����
    /// </summary>
    /// <param name="ItemId">��Ӧ��Ʒ</param>
    /// <returns>��ռ��������������</returns>
    public static int GetItemGridSum(int ItemId)
    {
        int Record = 0;
        foreach(ItemTable Item in Instance.ItemsTableList)
        {
            if (Item.Id == ItemId)
            {
                Record++;
            }
        }
        return Record;
    }

    /// <summary>
    /// ���ڻ�ȡ�����������
    /// </summary>
    /// <returns>�������������������</returns>
    public static int GetBagMax()
    {
        return Instance.BagMax;
    }

    /// <summary>
    /// ���ڻ�ȡ��ƷUI�б�
    /// </summary>
    /// <returns>��ƷUI�б�</returns>
    public static List<ItemUI> GetItemUIList()
    {
        return Instance.ItemsUIList;
    }

    /// <summary>
    /// ������Ϣ
    /// </summary>
    private static void BagDeBug()
    {
#if DEBUG_MODE
        if (Instance.Grid == null) Debug.LogWarning("Grid/��������δ��ֵ");
        if (Instance.ItemPrefab == null) Debug.LogWarning("ItemPrefab/����UIԤ����δ��ֵ");
        if (Instance.CapeCountWord == null) Debug.LogWarning("CapeCountWord/��Ԫ�������ı�δ��ֵ");
        if (Instance.dbManager == null) Debug.LogWarning("dbManager/���ݿ����ű�δ��ֵ");
#endif
    }
}
