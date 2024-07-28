//#define DEBUG_MODE

using UnityEngine;

/// <summary>
/// ͨ������B�����Ʊ���UI����ʾ��ر�
/// </summary>
public class BagOpenClose : MonoBehaviour
{
    /// <summary>
    /// ȫ��UI
    /// </summary>
    public GameObject UI;

    /// <summary>
    /// ����UI
    /// </summary>
    public GameObject Bag;

    /// <summary>
    /// ����UI
    /// </summary>
    public GameObject Make;

    /// <summary>
    /// ����UI���ұ�������UI
    /// </summary>
    public GameObject MakeUI;

    void Update()
    {
        //��ʱ����Ƿ�򿪱���
        OpenBag();
    }

    /// <summary>
    /// ͨ������B�����Ʊ����Ĵ�״̬
    /// </summary>
    private void OpenBag()
    {
        //��һ��B����ʾUI������UI״̬���ٰ�һ�ιر�UI
        if (Input.GetKeyDown(KeyCode.B))
        {
            //ȷ�����������ʾ���Ǳ�������
            UI.SetActive(!UI.activeSelf);
            Bag.SetActive(UI.activeSelf);
            //ȷ�����ú����߶�����ʾ�������ú���ͨ���ֶ�����������ʾ
            Make.SetActive(false);
            MakeUI.SetActive(false);
#if DEBUG_MODE
            Debug.Log("������ʾ״̬"+UI.activeSelf);
#endif
        }
    }
}
