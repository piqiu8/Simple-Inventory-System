//#define DEBUG_MODE

using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// �����������UI��ʾ
/// </summary>
public class MakeUIControl : MonoBehaviour
{
    /// <summary>
    /// �����䷽����ʱ�ò��ϣ�����֮����չ
    /// </summary>
    public MakeFormulaUI MakeFormula = null;
    
    /// <summary>
    /// ��ǰ������Ʒ��Ӧ����ͼ
    /// </summary>
    public Image MakeImage = null;
    
    /// <summary>
    /// ��ǰ������Ʒ��Ӧ������
    /// </summary>
    public TMP_Text MakeName = null;
    
    /// <summary>
    /// ���е�ǰ������Ʒ������
    /// </summary>
    public TMP_Text ItemNum = null;

    /// <summary>
    /// ��ǰ������Ʒ��Ӧ����Ϣ
    /// </summary>
    public TMP_Text Makeinfo = null;

    /// <summary>
    /// Ҫ������Ʒ����
    /// </summary>
    public TMP_Text MakeNum = null;

    /// <summary>
    /// ����������Ʒ�İ�ť
    /// </summary>
    public GameObject AddButton = null;

    /// <summary>
    /// ����������Ʒ�İ�ť
    /// </summary>
    public GameObject SubButton = null;

    void Start()
    {
        //���г�ʼ�����
#if DEBUG_MODE
        if (MakeFormula == null) Debug.LogWarning("MakeFormula/�����䷽δ���г�ʼ��");
        if (MakeImage == null) Debug.LogWarning("MakeImage/������Ʒ��ͼδ���г�ʼ��");
        if (MakeName == null) Debug.LogWarning("MakeName/������Ʒ����δ���г�ʼ��");
        if (ItemNum == null) Debug.LogWarning("ItemNum/������Ʒ����δ���г�ʼ��");
        if (Makeinfo == null) Debug.LogWarning("Makeinfo/������Ʒ��Ϣδ���г�ʼ��");
        if (MakeNum == null) Debug.LogWarning("MakeNum/������Ʒ����δ���г�ʼ��");
        if (AddButton == null) Debug.LogWarning("AddButton/���Ӱ�ťδ���г�ʼ��");
        if (SubButton == null) Debug.LogWarning("SubButton/���ٰ�ťδ���г�ʼ��");
#endif
    }
}
