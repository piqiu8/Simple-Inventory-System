//#define DEBUG_MODE

using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 进行制作界面的管理
/// </summary>
public class MakeManager : MonoBehaviour
{
    /// <summary>
    /// 制作界面单例
    /// </summary>
    static MakeManager Instance;

    /// <summary>
    /// 制作界面右侧的相关制作界面UI，其中包含需要替换的部分
    /// </summary>
    public MakeUIControl MakeUI = null;

    /// <summary>
    /// 底下所需材料列表里的单个材料UI模版，因为比较简单，所以直接使用预制体
    /// </summary>
    public GameObject FormulaPrefab = null;

    /// <summary>
    /// 所需材料列表的网格，主要是为了可以灵活改变材料列表里的所需材料类型与数量所以使用网格
    /// </summary>
    public GameObject Formula = null;

    /// <summary>
    /// 存放制作配方及相关信息的列表，其中0号位存放铁弓的制作配方，1号位存放铁刀的制作配方，2号位存放羽毛箭的制作配方
    /// </summary>
    public List<MakeFormulaUI> MakeFormulaUIlist = new List<MakeFormulaUI>();

    /// <summary>
    /// 控制能否制作物品
    /// </summary>
    private bool IfMake = true;

    /// <summary>
    /// 用于标记当前制作的为哪一个物品，0代表铁弓，1代表铁刀，2代表羽毛箭，默认为-1
    /// </summary>
    private int MakeTag = -1;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(this);
        }
        Instance = this;
#if DEBUG_MODE
        Debug.Log("MakeManger脚本执行Awake()");
#endif
    }

    private void OnEnable()
    {
        MakeDeBug();
        //隐藏制作UI
        Instance.transform.Find("UI/Make/MakeUI").gameObject.SetActive(false);
#if DEBUG_MODE
        Debug.Log("MakeManger脚本执行OnEnable()");
#endif

    }

    /// <summary>
    /// 通过按钮给方法赋值来显示不同物品的制作界面
    /// 设计思路：每个制作物品按钮对应一个制作物品，点击按钮时将MakeTag修改为制作物品的ID，以及辨别在制作什么物品，然后显示出右侧制作栏UI，并进行UI刷新
    /// </summary>
    /// <param name="MakeId">对应物品的ID</param>
    public static void ChooseMakeUI(int MakeId)
    {
        //记录当前制作的物品
        Instance.MakeTag = MakeId;
#if DEBUG_MODE
        Debug.Log("当前制作物品为：" + Instance.MakeFormulaUIlist[Instance.MakeTag].MakeName + " 对应MakeTag为：" + Instance.MakeTag);
#endif
        //刷新整个制作界面
        Instance.transform.Find("UI/Make/MakeUI").gameObject.SetActive(true);
        RefreshMakeInfo(Instance.MakeFormulaUIlist[Instance.MakeTag]);
        RefreshFormulaList(1);
    }

    /// <summary>
    /// 刷新右侧上边的制作物品信息
    /// 设计思路：通过MakeTag辨别制作什么物品，然后获取物品配方，根据MakeUI模版，替换对应部分来显示出不同制作物品的制作表UI
    /// </summary>
    /// <param name="makeFormulaUI">对应的制作配方信息</param>
    private static void RefreshMakeInfo(MakeFormulaUI makeFormulaUI)
    {
        //将右侧的相关制作界面UI对应部分替换为实际UI，其余部分保持不变
        //赋予对应制作配方，方便之后扩展使用
        Instance.MakeUI.MakeFormula = makeFormulaUI;
        Instance.MakeUI.MakeImage.sprite = makeFormulaUI.MakeImage;
        Instance.MakeUI.MakeName.text = makeFormulaUI.MakeName;
        Instance.MakeUI.ItemNum.text = BagManager.GetItemSum(makeFormulaUI.MakeId).ToString();
        Instance.MakeUI.Makeinfo.text = makeFormulaUI.MakeInfo;
    }

    /// <summary>
    /// 在底下生成对应物品的制作素材列表
    /// 设计思路：根据MakeTag辨别当前制作的物品，然后获取物品配方里的所需材料列表，之后在Formula网格下生成单个材料UI模版，并替换相应部分使其转换为对应材料UI，
    /// 再乘以当前制作数来显示出最终材料需求数目，最后将背包物品数目与所需材料数目进行比较，若小于所需材料则把文本标红并标记IfMake为false，代表无法制作该物品。
    /// 之后循环该操作直到所需材料UI全部生成。
    /// </summary>
    /// <param name="makeFormulaUI">对应的制作物品</param>
    private static void ShowFormulaListUI(MakeFormulaUI makeFormulaUI)
    {
        //获取制作数量
        int makeNum = GetMakeNum();
        //初始设为true
        Instance.IfMake = true;
        //通过循环动态添加制作所需的材料列表，这样做就能在即使只需要2种或1种材料时也能自动正常显示，无需人为修改
        for (int i = 0; i < makeFormulaUI.FormulaElemList.Count; i++)
        {
            //在所需材料列表下生成模版子物体
            GameObject FormulaPrefabElem = Instantiate(Instance.FormulaPrefab, Instance.Formula.transform);
            //将模版贴图替换为实际材料贴图
            FormulaPrefabElem.transform.Find("FormulaImage/ItemImage").GetComponent<Image>().sprite = makeFormulaUI.FormulaElemList[i].NeedImage;
            int ItemSum = BagManager.GetItemSum(makeFormulaUI.FormulaElemList[i].NeedId);
            //将模版文本通过字符串插值的方式，替换为实际对应文本
            TMP_Text PrefabText = FormulaPrefabElem.transform.Find("FormulaNum").GetComponent<TextMeshProUGUI>();
            PrefabText.text = $"({ItemSum} / {makeFormulaUI.FormulaElemList[i].NeedNum * makeNum}) {makeFormulaUI.FormulaElemList[i].NeedName}";
            //若材料数量不足制作所需数量，则将文本改为红色
            if (ItemSum < makeFormulaUI.FormulaElemList[i].NeedNum * makeNum)
            {
                PrefabText.color = new Color(165f / 255f, 0f / 255f, 0f / 255f, 255f / 255f);
                //只要有一个条件不满足则无法制作
                Instance.IfMake = false;
            }
        }
    }

    /// <summary>
    /// 增加制作物品数量，制作数与背包物品相加不可超过背包最大容量
    /// 设计思路：通过点击增加按钮增加制作数，对制作数进行判断，看看制作后会不会超过背包最大容量，不会的话则才修改制作数，并刷新底下的所需材料列表，使其
    /// 根据当前制作数重新显示
    /// </summary>
    public static void AddMakeNum()
    {
        int addMakeNum = GetMakeNum() + 1;
        if (!IfMakeOut(addMakeNum)) RefreshFormulaList(addMakeNum);
#if DEBUG_MODE
        else Debug.Log("本次制作将超出背包物品上限");
#endif
    }

    /// <summary>
    /// 减少制作物品数量，不可小于1
    /// 设计思路：通过点击减少按钮减少制作数，制作数不可小于1，满足条件才修改制作数，并刷新底下的所需材料列表，使其根据当前制作数重新显示
    /// </summary>
    public static void SubMakeNum()
    {
        int subMakeNum = GetMakeNum() - 1;
        if (subMakeNum > 0) RefreshFormulaList(subMakeNum);
#if DEBUG_MODE
        else Debug.Log("制作物品数量不可小于1");
#endif
    }

    /// <summary>
    /// 控制制作的材料消耗和物品的获得
    /// 设计思路：通过IfMake判断当前是否可以制作，若可以的话，点击制作按钮，通过MakeTag获取所需材料列表再乘以制作数，通过循环依次从背包扣除对应材料数量，
    /// 扣完后再获得对应制作数量的制作物品，最后重置刷新底部所需材料列表，并将制作数回调至1
    /// </summary>
    public static void MakeButton()
    {
        if (Instance.IfMake)
        {
            //防止在背包已满的情况下仍然可以做一个物品：因为默认显示就是制作一个，这一个物品不经过背包容量检查
            if (BagManager.GetBagNowCapa() < BagManager.GetBagMax())
            {
                foreach (var formulaElem in Instance.MakeFormulaUIlist[Instance.MakeTag].FormulaElemList)
                {
                    //消耗对应材料
                    BagManager.SubItemsOnTable(BagManager.GetItemUIList()[formulaElem.NeedId], formulaElem.NeedNum * GetMakeNum());
                }
                //获取对应物品
                BagManager.AddItemsOnTable(BagManager.GetItemUIList()[Instance.MakeTag], GetMakeNum());
#if DEBUG_MODE
                Debug.Log("成功制作" + Instance.MakeFormulaUIlist[Instance.MakeTag].MakeName + GetMakeNum() + "个");
#endif
                RefreshFormulaList(1);
                //刷新对应物品的总数
                Instance.MakeUI.ItemNum.text = BagManager.GetItemSum(Instance.MakeTag).ToString();
            }
#if DEBUG_MODE
            else Debug.Log("背包已满");
#endif
        }
#if DEBUG_MODE
        else Debug.Log("未满足制作条件");
#endif
    }

    /// <summary>
    /// 控制加减按钮透明度
    /// 设计思路：对于减按钮，若制作数为1，则使其变为半透明，反之正常显示，对于加按钮，先在现制作数上加一，再判断是否会超过背包最大容量，会的话使其变为半透明，
    /// 不会则正常显示
    /// </summary>
    private static void ButtonTranControl()
    {
        //若制作数为1，则减按钮透明，反之恢复原样
        if (GetMakeNum() == 1) ButtonTran(Instance.MakeUI.SubButton);
        else ButtonUnTran(Instance.MakeUI.SubButton);
        //若当前制作数+1会超过背包容量则使加按钮透明，反之恢复原样
        if (IfMakeOut(GetMakeNum() + 1)) ButtonTran(Instance.MakeUI.AddButton);
        else ButtonUnTran(Instance.MakeUI.AddButton);
    }

    /// <summary>
    /// 刷新底部的材料列表、制作数及按钮状态
    /// 设计思路：先刷新制作数至参数值，再移除整个材料列表然后重新再生成来刷新材料显示，最后再判断制作数来刷新按钮状态
    /// </summary>
    /// <param name="MakeNum">制作数</param>
    private static void RefreshFormulaList(int MakeNum)
    {
        //刷新制作数
        Instance.MakeUI.MakeNum.text = $"制作x{MakeNum}";
        //移除底部材料表
        RemoveFormulaListUI();
        //重新生成底部材料表以此刷新数量
        ShowFormulaListUI(Instance.MakeFormulaUIlist[Instance.MakeTag]);
        //刷新按钮状态
        ButtonTranControl();
    }

    /// <summary>
    /// 删除底下材料列表
    /// </summary>
    private static void RemoveFormulaListUI()
    {
        //遍历整个所需材料列表，将所需材料UI全部删除
        for (int i = 0; i < Instance.Formula.transform.childCount; i++)
        {
            //删除当前物品
            Destroy(Instance.Formula.transform.GetChild(i).gameObject);
        }
    }

    /// <summary>
    /// 获取要制作的物品数量
    /// </summary>
    /// <returns>返回对应整数</returns>
    private static int GetMakeNum()
    {
        //通过正则表达式获取当前要制作的物品数量文本，即获取x后面的多个数字
        string MakeNum = Regex.Match(Instance.MakeUI.MakeNum.text, @"x(\d+)").Groups[1].Value;
        return int.Parse(MakeNum);
    }

    /// <summary>
    /// 判断当前制作数会不会超过背包最大容量
    /// </summary>
    /// <param name="addMakeNum">当前制作数</param>
    /// <returns>超过则返回true，未超过则返回false</returns>
    private static bool IfMakeOut(int addMakeNum)
    {
        if (Instance.MakeTag != -1)
        {
            if (Instance.MakeTag == 0 || Instance.MakeTag == 1)
            {
                //若为单个为一格的物品
                if (BagManager.GetBagNowCapa() + addMakeNum > BagManager.GetBagMax()) return true;
                else return false;
            }
            else
            {
                //若为多个为一格的物品，将该物品总数+制作数量/该物品最大堆叠数并向上取整获得制作后物品将占用的格子数，再减去未制作前物品占用的格子数，以此获得新制作的物品需要的格子数，再和已用格子数相加进行判断
                if ((int)Math.Ceiling(((double)(BagManager.GetItemSum(Instance.MakeTag) + addMakeNum)) / BagManager.GetItemUIList()[Instance.MakeTag].ItemMax) - BagManager.GetItemGridSum(Instance.MakeTag) + BagManager.GetBagNowCapa() > BagManager.GetBagMax())
                    return true;
                else return false;
            }
        }
        else
        {
#if DEBUG_MODE
            Debug.LogWarning("制作标签未进行赋值");
#endif
            //默认返回false
            return false; 
        }
    }

    /// <summary>
    /// 将按钮变为透明
    /// </summary>
    /// <param name="button">要处理的按钮</param>
    private static void ButtonTran(GameObject button)
    {
        button.transform.GetComponent<Image>().color = new Color(1, 1, 1, 120f / 255f);
        button.transform.GetChild(0).GetComponent<TextMeshProUGUI>().color = new Color(1, 1, 1, 120f / 255f);
    }

    /// <summary>
    /// 将按钮恢复原样
    /// </summary>
    /// <param name="button">要处理的按钮</param>
    private static void ButtonUnTran(GameObject button)
    {
        button.transform.GetComponent<Image>().color = Color.white;
        button.transform.GetChild(0).GetComponent<TextMeshProUGUI>().color = Color.white;
    }

    /// <summary>
    /// 调试信息
    /// </summary>
    private static void MakeDeBug()
    {
#if DEBUG_MODE
        if (Instance.MakeUI == null) Debug.LogWarning("MakeUI/制作表UI未赋值");
        if (Instance.FormulaPrefab == null) Debug.LogWarning("FormulaPrefab/材料UI预制体未赋值");
        if (Instance.Formula == null) Debug.LogWarning("Formula/材料列表网格未赋值");
#endif
    }
}
