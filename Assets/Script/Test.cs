//#define DEBUG_MODE

using UnityEngine;

/// <summary>
/// 测试背包功能是否正常所用脚本
/// </summary>
public class Test:MonoBehaviour
{
    /// <summary>
    /// 对应背包物体
    /// </summary>
    public ItemUI Item = null;
    public int AddNum = 0, SubNum = 0;

    private void Start()
    {
#if DEBUG_MODE
        if (Item == null) Debug.LogWarning("Item/对应物品未赋值");
#endif
    }

    /// <summary>
    /// 向背包中添加物品
    /// </summary>
    public void AddItem()
    {
        BagManager.AddItemsOnTable(Item, AddNum);
    }

    /// <summary>
    /// 减少背包物品的数量
    /// </summary>
    public void SubItem()
    {
        BagManager.SubItemsOnTable(Item, SubNum);
    }
}
