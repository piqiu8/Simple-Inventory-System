//#define DEBUG_MODE

using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// ʹѡ��Ч���ڲ�ͬ��ť�н����л�
/// </summary>
public class ButtonSelectSwitch : MonoBehaviour
{
    /// <summary>
    /// ����ѡ��Ч����ȫ����ť
    /// </summary>
    public Button[] Buttons;

    /// <summary>
    /// ��ǰѡ�еİ�ť
    /// </summary>
    private Button CurrentSelectedButton;

    private void OnEnable()
    {
        //�������ã�ʹѡ��Ч����ʧ
        if (CurrentSelectedButton != null)
        {
            HideSelectionEffect(CurrentSelectedButton);
        }
#if DEBUG_MODE
        Debug.Log("ButtonSelectSwicth�ű���ʼ��");
#endif
    }

    void Start()
    {
        //��ʼ����ÿ����ť��ӵ���¼�
        foreach (Button button in Buttons)
        {
            button.onClick.AddListener(() => ButtonSelected(button));
        }
    }

    /// <summary>
    /// ʵ��ѡ��֮��Ļ���Ч��
    /// </summary>
    /// <param name="selectedButton">��ǰ��ѡ�еİ�ť</param>
    private void ButtonSelected(Button selectedButton)
    {
        //�����ǰ��ѡ�еİ�ť����������ѡ��Ч��
        if (CurrentSelectedButton != null)
        {
            HideSelectionEffect(CurrentSelectedButton);
        }
        //��ʾ��ѡ�а�ť��ѡ��Ч��
        ShowSelectionEffect(selectedButton);
        //���µ�ǰѡ�а�ť
        CurrentSelectedButton = selectedButton;
    }

    /// <summary>
    /// ��ʾ��ťѡ��Ч��
    /// </summary>
    /// <param name="button">��ǰѡ�а�ť</param>
    private void ShowSelectionEffect(Button button)
    {
        //ѡ��Ч���ǰ�ť�������壬����Ϊxuanzhong
        Transform selectionEffect = button.transform.Find("xuanzhong");
        if (selectionEffect != null)
        {
            selectionEffect.gameObject.SetActive(true);
        }
#if DEBUG_MODE
        else Debug.LogWarning("��ťδ���ѡ��Ч��");
#endif
    }

    /// <summary>
    /// ���ذ�ťѡ��Ч��
    /// </summary>
    /// <param name="button">��һ��ѡ�а�ť</param>
    private void HideSelectionEffect(Button button)
    {
        Transform selectionEffect = button.transform.Find("xuanzhong");
        if (selectionEffect != null)
        {
            selectionEffect.gameObject.SetActive(false);
        }
#if DEBUG_MODE
        else Debug.LogWarning("��ťδ���ѡ��Ч��");
#endif
    }
}