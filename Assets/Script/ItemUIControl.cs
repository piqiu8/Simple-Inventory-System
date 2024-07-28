//#define DEBUG_MODE

using UnityEngine.UI;
using TMPro;
using UnityEngine;

/// <summary>
/// 控制预制体模版的需替换内容，以便快速简易替换预制体模版的需替换部分以显示出不同物品UI
/// </summary>
public class ItemUIControl : MonoBehaviour
{
    /// <summary>
    /// 物品UI的对应物品，暂时用不上，用于之后扩展
    /// </summary>
    public ItemUI Item = null;

    /// <summary>
    /// 物品UI的对应物品名称
    /// </summary>
    public TMP_Text ItemName = null;

    /// <summary>
    /// 物品UI的对应物品贴图
    /// </summary>
    public Image ItemImage = null;

    /// <summary>
    /// 物品UI的对应数量文本
    /// </summary>
    public TMP_Text ItemNum = null;

    void Start()
    {
#if DEBUG_MODE
        //进行初始化检查
        if (Item == null) Debug.LogWarning("Item/物品未进行初始化");
        if (ItemName == null) Debug.LogWarning("ItemName/物品名称未进行初始化");
        if (ItemImage == null) Debug.LogWarning("ItemImage/物品贴图未进行初始化");
        if (ItemNum == null) Debug.LogWarning("ItemNum/物品数量文本未进行初始化");
#endif
    }
}
