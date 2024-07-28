using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 制作物品的对应配方，其中包含制作物品名称、制作物品描述、制作所需的物品贴图、制作所需的材料列表
/// </summary>
//在unity界面中生成可创建NewFormula文件的Bag菜单，文件默认名为NewFormula
[CreateAssetMenu(fileName ="NewFormula",menuName ="Bag/NewFormula")]
public class MakeFormulaUI : ScriptableObject
{
    /// <summary>
    /// 当前制作物品对应的ID
    /// </summary>
    public int MakeId=-1;
    
    /// <summary>
    /// 当前制作物品对应的名称
    /// </summary>
    public string MakeName = null;

    /// <summary>
    /// 当前制作物品对应的贴图
    /// </summary>
    public Sprite MakeImage = null;

    /// <summary>
    /// 当前制作物品对应的描述
    /// </summary>
    [TextArea]
    public string MakeInfo = string.Empty;

    /// <summary>
    /// 将制作所需的物品数量与名称封装为结构体，使其保持一致方便管理，并进行序列化使其在界面上显示
    /// </summary>
    [System.Serializable]
    public struct FormulaElem
    {
        /// <summary>
        /// 制作材料对应的ID
        /// </summary>
        public int NeedId;

        /// <summary>
        /// 制作材料对应的贴图
        /// </summary>
        public Sprite NeedImage;

        /// <summary>
        /// 制作材料对应的数量
        /// </summary>
        public int NeedNum;

        /// <summary>
        /// 制作材料对应的名称
        /// </summary>
        public string NeedName;
        public FormulaElem(int needId, Sprite needImage, int needNum, string needName)
        {
            NeedId = needId;
            NeedImage = needImage;
            NeedNum = needNum;
            NeedName = needName;
        }
    }

    /// <summary>
    /// 制作材料列表
    /// </summary>
    public List<FormulaElem> FormulaElemList = new List<FormulaElem>();
}
