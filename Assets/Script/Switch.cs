//#define DEBUG_MODE

using UnityEngine;
using UnityEngine.UI;

public class Switch : MonoBehaviour
{
    /// <summary>
    /// 背包UI切换按钮
    /// </summary>
    public Button Bag = null;

    /// <summary>
    /// 制作UI切换按钮
    /// </summary>
    public Button make = null;

    /// <summary>
    /// 背包UI按钮颜色
    /// </summary>
    private Color BagButtonColor = new Color(194f / 255f, 189f / 255f, 198f / 255f, 255f / 255f);

    /// <summary>
    /// 制作UI按钮颜色
    /// </summary>
    private Color MakeButtonColor = new Color(195f / 255f, 190f / 255f, 198f / 255f, 255f / 255f);

    /// <summary>
    /// 背景按钮颜色
    /// </summary>
    private Color BackButtonColor = new Color(162f / 255f, 154f / 255f, 172f / 255f, 255f / 255f);

    private void OnEnable()
    {
        //进行颜色重置，确保重置完背包按钮颜色正常，而制作按钮为背景色
        Bag.image.color = BagButtonColor;
        make.image.color = BackButtonColor;
#if DEBUG_MODE
        Debug.Log("Switch脚本初始化");
#endif
    }

    private void Start()
    {
#if DEBUG_MODE
        if (Bag == null) Debug.LogError("背包UI切换按钮未赋值");
        if (make == null) Debug.LogError("制作UI切换按钮未赋值");
#endif
    }

    /// <summary>
    /// 选中背包按钮时，使其变为选中颜色，并让制作按钮变为背景颜色
    /// </summary>
    public void BagSwitchButtonColor()
    {
        Bag.image.color = BagButtonColor;
        make.image.color = BackButtonColor;
    }

    /// <summary>
    /// 选中制作按钮时，使其变为选中颜色，并让背包按钮变为背景颜色
    /// </summary>
    public void MakeSwitchButtonColor()
    {
        make.image.color = MakeButtonColor;
        Bag.image.color = BackButtonColor;
    }
}
