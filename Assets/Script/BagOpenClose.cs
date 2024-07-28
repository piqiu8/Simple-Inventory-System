//#define DEBUG_MODE

using UnityEngine;

/// <summary>
/// 通过按键B，控制背包UI的显示与关闭
/// </summary>
public class BagOpenClose : MonoBehaviour
{
    /// <summary>
    /// 全部UI
    /// </summary>
    public GameObject UI;

    /// <summary>
    /// 背包UI
    /// </summary>
    public GameObject Bag;

    /// <summary>
    /// 制作UI
    /// </summary>
    public GameObject Make;

    /// <summary>
    /// 制作UI中右边制作表UI
    /// </summary>
    public GameObject MakeUI;

    void Update()
    {
        //随时检测是否打开背包
        OpenBag();
    }

    /// <summary>
    /// 通过按键B，控制背包的打开状态
    /// </summary>
    private void OpenBag()
    {
        //按一次B键显示UI并重置UI状态，再按一次关闭UI
        if (Input.GetKeyDown(KeyCode.B))
        {
            //确保重置完后显示的是背包界面
            UI.SetActive(!UI.activeSelf);
            Bag.SetActive(UI.activeSelf);
            //确保重置后两者都不显示出来，让后续通过手动操作进行显示
            Make.SetActive(false);
            MakeUI.SetActive(false);
#if DEBUG_MODE
            Debug.Log("背包显示状态"+UI.activeSelf);
#endif
        }
    }
}
