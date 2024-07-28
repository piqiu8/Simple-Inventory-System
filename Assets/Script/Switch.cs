//#define DEBUG_MODE

using UnityEngine;
using UnityEngine.UI;

public class Switch : MonoBehaviour
{
    /// <summary>
    /// ����UI�л���ť
    /// </summary>
    public Button Bag = null;

    /// <summary>
    /// ����UI�л���ť
    /// </summary>
    public Button make = null;

    /// <summary>
    /// ����UI��ť��ɫ
    /// </summary>
    private Color BagButtonColor = new Color(194f / 255f, 189f / 255f, 198f / 255f, 255f / 255f);

    /// <summary>
    /// ����UI��ť��ɫ
    /// </summary>
    private Color MakeButtonColor = new Color(195f / 255f, 190f / 255f, 198f / 255f, 255f / 255f);

    /// <summary>
    /// ������ť��ɫ
    /// </summary>
    private Color BackButtonColor = new Color(162f / 255f, 154f / 255f, 172f / 255f, 255f / 255f);

    private void OnEnable()
    {
        //������ɫ���ã�ȷ�������걳����ť��ɫ��������������ťΪ����ɫ
        Bag.image.color = BagButtonColor;
        make.image.color = BackButtonColor;
#if DEBUG_MODE
        Debug.Log("Switch�ű���ʼ��");
#endif
    }

    private void Start()
    {
#if DEBUG_MODE
        if (Bag == null) Debug.LogError("����UI�л���ťδ��ֵ");
        if (make == null) Debug.LogError("����UI�л���ťδ��ֵ");
#endif
    }

    /// <summary>
    /// ѡ�б�����ťʱ��ʹ���Ϊѡ����ɫ������������ť��Ϊ������ɫ
    /// </summary>
    public void BagSwitchButtonColor()
    {
        Bag.image.color = BagButtonColor;
        make.image.color = BackButtonColor;
    }

    /// <summary>
    /// ѡ��������ťʱ��ʹ���Ϊѡ����ɫ�����ñ�����ť��Ϊ������ɫ
    /// </summary>
    public void MakeSwitchButtonColor()
    {
        make.image.color = MakeButtonColor;
        Bag.image.color = BackButtonColor;
    }
}
