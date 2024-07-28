//#define DEBUG_MODE

using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// 控制制作表的UI显示
/// </summary>
public class MakeUIControl : MonoBehaviour
{
    /// <summary>
    /// 制作配方，暂时用不上，用于之后扩展
    /// </summary>
    public MakeFormulaUI MakeFormula = null;
    
    /// <summary>
    /// 当前制作物品对应的贴图
    /// </summary>
    public Image MakeImage = null;
    
    /// <summary>
    /// 当前制作物品对应的名称
    /// </summary>
    public TMP_Text MakeName = null;
    
    /// <summary>
    /// 已有当前制作物品的数量
    /// </summary>
    public TMP_Text ItemNum = null;

    /// <summary>
    /// 当前制作物品对应的信息
    /// </summary>
    public TMP_Text Makeinfo = null;

    /// <summary>
    /// 要制作物品数量
    /// </summary>
    public TMP_Text MakeNum = null;

    /// <summary>
    /// 增加制作物品的按钮
    /// </summary>
    public GameObject AddButton = null;

    /// <summary>
    /// 减少制作物品的按钮
    /// </summary>
    public GameObject SubButton = null;

    void Start()
    {
        //进行初始化检查
#if DEBUG_MODE
        if (MakeFormula == null) Debug.LogWarning("MakeFormula/制作配方未进行初始化");
        if (MakeImage == null) Debug.LogWarning("MakeImage/制作物品贴图未进行初始化");
        if (MakeName == null) Debug.LogWarning("MakeName/制作物品名称未进行初始化");
        if (ItemNum == null) Debug.LogWarning("ItemNum/已有物品数量未进行初始化");
        if (Makeinfo == null) Debug.LogWarning("Makeinfo/制作物品信息未进行初始化");
        if (MakeNum == null) Debug.LogWarning("MakeNum/制作物品数量未进行初始化");
        if (AddButton == null) Debug.LogWarning("AddButton/增加按钮未进行初始化");
        if (SubButton == null) Debug.LogWarning("SubButton/减少按钮未进行初始化");
#endif
    }
}
