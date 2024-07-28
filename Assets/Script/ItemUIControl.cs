//#define DEBUG_MODE

using UnityEngine.UI;
using TMPro;
using UnityEngine;

/// <summary>
/// ����Ԥ����ģ������滻���ݣ��Ա���ټ����滻Ԥ����ģ������滻��������ʾ����ͬ��ƷUI
/// </summary>
public class ItemUIControl : MonoBehaviour
{
    /// <summary>
    /// ��ƷUI�Ķ�Ӧ��Ʒ����ʱ�ò��ϣ�����֮����չ
    /// </summary>
    public ItemUI Item = null;

    /// <summary>
    /// ��ƷUI�Ķ�Ӧ��Ʒ����
    /// </summary>
    public TMP_Text ItemName = null;

    /// <summary>
    /// ��ƷUI�Ķ�Ӧ��Ʒ��ͼ
    /// </summary>
    public Image ItemImage = null;

    /// <summary>
    /// ��ƷUI�Ķ�Ӧ�����ı�
    /// </summary>
    public TMP_Text ItemNum = null;

    void Start()
    {
#if DEBUG_MODE
        //���г�ʼ�����
        if (Item == null) Debug.LogWarning("Item/��Ʒδ���г�ʼ��");
        if (ItemName == null) Debug.LogWarning("ItemName/��Ʒ����δ���г�ʼ��");
        if (ItemImage == null) Debug.LogWarning("ItemImage/��Ʒ��ͼδ���г�ʼ��");
        if (ItemNum == null) Debug.LogWarning("ItemNum/��Ʒ�����ı�δ���г�ʼ��");
#endif
    }
}
