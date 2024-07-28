//#define DEBUG_MODE

using UnityEngine;
using TMPro;
using System.Collections.Generic;
using System;

/// <summary>
/// 管理玩家的背包，其中包括物品单元格的添加显示、刷新，同时修改背包容量文本
/// </summary>
public class BagManager : MonoBehaviour
{
    /// <summary>
    /// Bag_Manager的单例实例
    /// </summary>
    static BagManager Instance;

    /// <summary>
    /// 背包UI的网格
    /// </summary>
    public GameObject Grid = null;

    /// <summary>
    /// 物品UI的预制体，所有物品都以这个为模版在背包中进行显示
    /// </summary>
    public ItemUIControl ItemPrefab = null;

    /// <summary>
    /// 背包的当前物品所占单元格数量文本
    /// </summary>
    public TMP_Text CapeCountWord = null;

    /// <summary>
    /// 数据库管理脚本
    /// </summary>
    public DatabaseManager dbManager = null;

    /// <summary>
    /// 用于存储物品数据的列表，一个在其最大堆叠范围内的物品为一个元素
    /// </summary>
    private List<ItemTable> ItemsTableList = new List<ItemTable>();

    /// <summary>
    /// 用于存储物品UI的列表，其中物品UI存放顺序与物品ID一一对应，确保可以根据物品ID来确定其UI
    /// 其中物品UI通过ScriptableObject提前生成好文件存储在本地，需要时则根据列表索引获取相应物品UI
    /// </summary>
    public List<ItemUI> ItemsUIList = new List<ItemUI>();

    /// <summary>
    /// 背包最大单元格数，默认为30
    /// </summary>
    public int BagMax = 30;

    //确保只有一个BagManger实例
    private void Awake()
    {
        if (Instance != null)
            Destroy(this);
        Instance = this;
#if DEBUG_MODE
        Debug.Log("BagManager脚本执行Awake()");
#endif
    }

    private void OnEnable()
    {
        RefreshItemUI();
#if DEBUG_MODE
        Debug.Log("BagManager脚本执行OnEnable()");
#endif
    }

    private void Start()
    {
        //先进行初始化检查
        BagDeBug();
        //获取数据库里的全部数据
        Instance.ItemsTableList = Instance.dbManager.GetAllItems();
        RefreshItemUI();
#if DEBUG_MODE
        Debug.Log("背包所有物品如下：");
        foreach (var item in Instance.ItemsTableList)
        {
            Debug.Log("ID: " + item.Id + ", Num: " + item.ItemNum + ", Type: " + item.ItemType);
        }
        Debug.Log("BagManager脚本执行Start()");
#endif
    }

    /// <summary>
    /// 向背包里显示物品UI
    /// 设计思路：在设定好的Grid网格下生成预制体模版，使其可以有序排列，预制体模版再根据不同物品替换对应部分UI来形成不同物品UI
    /// </summary>
    /// <param name="Item">对应物品数据</param>
    private static void AddItemOnUI(ItemTable Item)
    {
        //以预制体为模版生成一个Grid的子物体
        ItemUIControl ItemUIC = Instantiate(Instance.ItemPrefab, Instance.Grid.transform);
        //将模版的对应部分替换为实际物品对应部分
        ItemUIC.Item = Instance.ItemsUIList[Item.Id];
        ItemUIC.ItemName.text = Instance.ItemsUIList[Item.Id].ItemName;
        ItemUIC.ItemImage.sprite = Instance.ItemsUIList[Item.Id].ItemImage;
        ItemUIC.ItemNum.text = Item.ItemNum.ToString();
    }

    /// <summary>
    /// 向背包增加指定数量的物品
    /// 设计思路：通过将要添加的物品数量根据其最大堆叠数，拆分为满堆叠数与剩余数，其中满堆叠数算作一个新元素，直接插入物品列表与数据表即可，剩余数则在列表寻找未满物品并加进去
    /// ，若加完后还有剩余，则剩余部分算作一个新元素插入物品列表与数据表
    /// </summary>
    /// <param name="Item">要增加的物品</param>
    /// <param name="AddNum">增加的数量</param>
    public static void AddItemsOnTable(ItemUI Item,int AddNum)
    {
        //若背包已满则不用操作
        if (Instance.ItemsTableList.Count < Instance.BagMax)
        {
            //确保新添加物品后背包格子数不会超过背包最大容量：将背包对应物品总数+需添加数量/对应物品最大堆叠数并向上取整获得添加后对应物品将占用的格子数，再减去未添加前对应物品占用的格子数，以此获得新添加物品需要的格子数
            //，再加上现背包已用格子数，这样就算得添加物品后背包所用格子数
            if ((int)Math.Ceiling((double)(GetItemSum(Item.ItemId) + AddNum) / Item.ItemMax) - GetItemGridSum(Item.ItemId) + GetBagNowCapa() <= Instance.BagMax)
            {
                //需新增的满堆叠的数据条数
                int x = AddNum / Item.ItemMax;
#if DEBUG_MODE
                Debug.Log("新增单元格数：" + x);
#endif
                //剩余需新增数目
                int y = AddNum - Item.ItemMax * x;
                //先处理剩余新增数目，避免添加满堆叠数据后增加循环次数
                //y=0则说明没有剩余，数目正好为满堆叠数的倍数，不必专门处理，直接插入满堆叠数据就行
                if (y != 0)
                {
                    foreach (ItemTable ItemTable in Instance.ItemsTableList)
                    {
                        //通过ID寻找对应物体，且比较当前数量是否超过最大堆叠数，若未超过则用剩余数进行补足
                        if (ItemTable.Id == Item.ItemId && ItemTable.ItemNum < Item.ItemMax)
                        {
                            //相加小于等于堆叠最大值则直接全部加进去就行，不用分开处理
                            if (ItemTable.ItemNum + y <= Item.ItemMax)
                            {
                                UpdateItemList(ItemTable, ItemTable.ItemNum + y);
                                y = 0;
                            }
                            //相加大于最大堆叠数，则还需处理最后多出来的部分
                            else
                            {
                                y -= Item.ItemMax - ItemTable.ItemNum;
                                UpdateItemList(ItemTable, Item.ItemMax);
                            }
                            //因为不足最大堆叠数的对应物品最多只有一个，毕竟其他都叠满了才会剩下这一个，所以不必继续循环
                            break;
                        }
                    }
                    //处理最后剩下的部分
                    if (y != 0) AddItemList(Item, y);
                }
                //进行满堆叠数据插入
                for (int i = 0; i < x; i++) AddItemList(Item, Item.ItemMax);
                //刷新背包状态
                RefreshItemUI();
#if DEBUG_MODE
                Debug.Log("成功添加" + AddNum + "个" + Item.ItemName);
#endif
            }
#if DEBUG_MODE
            else Debug.Log("新增物品超过背包容量");
#endif
        }
#if DEBUG_MODE
        else Debug.Log("背包已满");
#endif
    }

    /// <summary>
    /// 从背包中减少指定数量的物品
    /// 设计思路：通过将要减少的物品数量根据其最大堆叠数，拆分为最大堆叠数与剩余数，其中直接删除满堆叠数元素来对应减少最大堆叠数，剩余数则寻找未满物品元素来减少其数量，减完数量为0则删除该元素
    /// ，若剩余数减完仍有剩余则继续寻找未满物品元素再减一次即可
    /// </summary>
    /// <param name="Item">要减少的物品</param>
    /// <param name="SubNum">减少的数量</param>
    public static void SubItemsOnTable(ItemUI Item,int SubNum)
    {
        //物品总数大于等于要减少的数量才进行操作
        int Residue = GetItemSum(Item.ItemId) - SubNum;
        if (Residue >= 0)
        {
            //计算要移除多少个物品单元格
            int SubGridNum = SubNum / Item.ItemMax;
#if DEBUG_MODE
            Debug.Log("移除单元格数：" + SubGridNum);
#endif
            //计算剩下无法构成一个单元格的数量为多少
            int z = SubNum - Item.ItemMax * SubGridNum;
            //从前往后依次移除单元格
            for (int i = 0, j = 0; i < Instance.ItemsTableList.Count && j < SubGridNum; i++)
            {
                if (Instance.ItemsTableList[i].Id == Item.ItemId)
                {
                    DeleteItemList(Instance.ItemsTableList[i]);
                    //更新列表索引
                    i--;
                    j++;
                }
            }
            //处理多余部分
            if (z != 0)
            {
                //定位最后一个对应物品的位置
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
            Debug.Log("成功减少了"+Item.ItemName+SubNum+"个");
#endif
            RefreshItemUI();
        }
#if DEBUG_MODE
        else Debug.Log("减少数超过背包对应物品数");
#endif
    }

    /// <summary>
    /// 刷新背包状态，使物品增加或减少时可正常显示物品数量，并整理物品顺序
    /// 设计思路：直接删除Grid网格下的所有子物体即背包里所有物品UI，再对物品列表按ID从小到大进行排序，ID相同则按数量从大到小进行排序，再重新生成物品UI，以此刷新物品的信息与背包物品排列
    /// </summary>
    private static void RefreshItemUI()
    {
        //遍历整个背包，将背包物品全部删除
        for (int i = 0; i < Instance.Grid.transform.childCount; i++)
        {
            //删除当前物品
            Destroy(Instance.Grid.transform.GetChild(i).gameObject);
        }
        //按ID从小到大对列表进行排序，如果ID相同则按ItemNum从大到小进行排序
        Instance.ItemsTableList.Sort((x, y) =>
        {
            int IdCompare = x.Id.CompareTo(y.Id);
            if (IdCompare == 0)
            {
                return y.ItemNum.CompareTo(x.ItemNum);
            }
            return IdCompare;
        });
        //再将刷新后的物品添加回背包
        for (int i = 0; i < Instance.ItemsTableList.Count; i++)
        {
            AddItemOnUI(Instance.ItemsTableList[i]);
        }
        //将背包列表中元素数以00的形式转化为字符串赋给文本
        Instance.CapeCountWord.text = Instance.ItemsTableList.Count.ToString("D2");
#if DEBUG_MODE
        Debug.Log("背包状态已刷新");
#endif
    }

    /// <summary>
    /// 更新数据表及修改列表物品数量
    /// </summary>
    /// <param name="itemTable">要更新的数据</param>
    /// <param name="UpdateNum">修改后的数量</param>
    private static void UpdateItemList(ItemTable itemTable,int UpdateNum)
    {
        itemTable.ItemNum = UpdateNum;
        Instance.dbManager.UpdateItem(itemTable);
    }

    /// <summary>
    /// 在数据表和列表新增数据
    /// </summary>
    /// <param name="item">新增的物品</param>
    /// <param name="AddNum">新增的数量</param>
    private static void AddItemList(ItemUI item,int AddNum)
    {
        ItemTable NewItemTable = new ItemTable { Id = item.ItemId, ItemNum = AddNum, ItemType = item.Type.ToString() };
        Instance.ItemsTableList.Add(NewItemTable);
        Instance.dbManager.InsertItem(NewItemTable);
#if DEBUG_MODE
        Debug.Log("插入数据：Id:" + item.ItemId + " ItemNum:" + AddNum + " ItemType:" + item.Type.ToString());
#endif
    }

    /// <summary>
    /// 删除数据表及列表内的物品信息
    /// </summary>
    /// <param name="itemTable">要删除的物品信息</param>
    private static void DeleteItemList(ItemTable itemTable)
    {
        //先在数据表进行删除，防止删除列表内容后列表顺序变化而造成出错
        Instance.dbManager.DeleteItem(itemTable);
        Instance.ItemsTableList.Remove(itemTable);
#if DEBUG_MODE
        Debug.Log("已从列表移除数据");
#endif
    }

    /// <summary>
    /// 用于获取背包某物品总数
    /// </summary>
    /// <param name="ItemId">需要获取总数的物品</param>
    /// <returns>该物品总数整形数字</returns>
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
    /// 返回当前背包所用格子数
    /// </summary>
    /// <returns>返回背包所用格子数整型数字</returns>
    public static int GetBagNowCapa()
    {
        return Instance.ItemsTableList.Count;
    }

    /// <summary>
    /// 用于获取某物品所占的格子数
    /// </summary>
    /// <param name="ItemId">对应物品</param>
    /// <returns>所占格子数整形数字</returns>
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
    /// 用于获取背包最大容量
    /// </summary>
    /// <returns>背包最大容量整型数字</returns>
    public static int GetBagMax()
    {
        return Instance.BagMax;
    }

    /// <summary>
    /// 用于获取物品UI列表
    /// </summary>
    /// <returns>物品UI列表</returns>
    public static List<ItemUI> GetItemUIList()
    {
        return Instance.ItemsUIList;
    }

    /// <summary>
    /// 调试信息
    /// </summary>
    private static void BagDeBug()
    {
#if DEBUG_MODE
        if (Instance.Grid == null) Debug.LogWarning("Grid/背包网格未赋值");
        if (Instance.ItemPrefab == null) Debug.LogWarning("ItemPrefab/物体UI预制体未赋值");
        if (Instance.CapeCountWord == null) Debug.LogWarning("CapeCountWord/单元格数量文本未赋值");
        if (Instance.dbManager == null) Debug.LogWarning("dbManager/数据库管理脚本未赋值");
#endif
    }
}
