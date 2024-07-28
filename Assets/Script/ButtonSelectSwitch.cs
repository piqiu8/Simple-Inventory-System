//#define DEBUG_MODE

using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 使选中效果在不同按钮中进行切换
/// </summary>
public class ButtonSelectSwitch : MonoBehaviour
{
    /// <summary>
    /// 含有选中效果的全部按钮
    /// </summary>
    public Button[] Buttons;

    /// <summary>
    /// 当前选中的按钮
    /// </summary>
    private Button CurrentSelectedButton;

    private void OnEnable()
    {
        //进行重置，使选中效果消失
        if (CurrentSelectedButton != null)
        {
            HideSelectionEffect(CurrentSelectedButton);
        }
#if DEBUG_MODE
        Debug.Log("ButtonSelectSwicth脚本初始化");
#endif
    }

    void Start()
    {
        //初始化，每个按钮添加点击事件
        foreach (Button button in Buttons)
        {
            button.onClick.AddListener(() => ButtonSelected(button));
        }
    }

    /// <summary>
    /// 实现选项之间的互斥效果
    /// </summary>
    /// <param name="selectedButton">当前被选中的按钮</param>
    private void ButtonSelected(Button selectedButton)
    {
        //如果当前有选中的按钮，隐藏它的选中效果
        if (CurrentSelectedButton != null)
        {
            HideSelectionEffect(CurrentSelectedButton);
        }
        //显示新选中按钮的选中效果
        ShowSelectionEffect(selectedButton);
        //更新当前选中按钮
        CurrentSelectedButton = selectedButton;
    }

    /// <summary>
    /// 显示按钮选中效果
    /// </summary>
    /// <param name="button">当前选中按钮</param>
    private void ShowSelectionEffect(Button button)
    {
        //选中效果是按钮的子物体，名称为xuanzhong
        Transform selectionEffect = button.transform.Find("xuanzhong");
        if (selectionEffect != null)
        {
            selectionEffect.gameObject.SetActive(true);
        }
#if DEBUG_MODE
        else Debug.LogWarning("按钮未添加选中效果");
#endif
    }

    /// <summary>
    /// 隐藏按钮选中效果
    /// </summary>
    /// <param name="button">上一次选中按钮</param>
    private void HideSelectionEffect(Button button)
    {
        Transform selectionEffect = button.transform.Find("xuanzhong");
        if (selectionEffect != null)
        {
            selectionEffect.gameObject.SetActive(false);
        }
#if DEBUG_MODE
        else Debug.LogWarning("按钮未添加选中效果");
#endif
    }
}