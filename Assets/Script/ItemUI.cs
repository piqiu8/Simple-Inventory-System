using UnityEngine;

/// <summary>
/// 创建物品对应属性并保存在本地，其中包含物品的名称、贴图、数量、类型、最大堆叠数
/// </summary>
//在unity界面中生成可创建NewItem文件的Bag菜单，文件默认名为NewNewItem
[CreateAssetMenu(fileName = "NewItem",menuName = "Bag/NewItem")]
public class ItemUI : ScriptableObject
{
    /// <summary>
    /// 物品对应id，初始值为-1
    /// </summary>
    public int ItemId = -1;

    /// <summary>
    /// 物品对应名字
    /// </summary>
    public string ItemName = string.Empty;

    /// <summary>
    /// 物品对应贴图
    /// </summary>
    public Sprite ItemImage = null;

    /// <summary>
    /// 物品的不同类型
    /// </summary>
    public enum ItemType{ 材料,武器,弹药,未定类型 }

    /// <summary>
    /// 物品的对应类型
    /// </summary>
    public ItemType Type = ItemType.未定类型;

    /// <summary>
    /// 物品的最大堆叠数
    /// </summary>
    public int ItemMax = 1;
}
