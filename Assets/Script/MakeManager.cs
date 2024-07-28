//#define DEBUG_MODE

using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// ������������Ĺ���
/// </summary>
public class MakeManager : MonoBehaviour
{
    /// <summary>
    /// �������浥��
    /// </summary>
    static MakeManager Instance;

    /// <summary>
    /// ���������Ҳ�������������UI�����а�����Ҫ�滻�Ĳ���
    /// </summary>
    public MakeUIControl MakeUI = null;

    /// <summary>
    /// ������������б���ĵ�������UIģ�棬��Ϊ�Ƚϼ򵥣�����ֱ��ʹ��Ԥ����
    /// </summary>
    public GameObject FormulaPrefab = null;

    /// <summary>
    /// ��������б��������Ҫ��Ϊ�˿������ı�����б�������������������������ʹ������
    /// </summary>
    public GameObject Formula = null;

    /// <summary>
    /// ��������䷽�������Ϣ���б�����0��λ��������������䷽��1��λ��������������䷽��2��λ�����ë���������䷽
    /// </summary>
    public List<MakeFormulaUI> MakeFormulaUIlist = new List<MakeFormulaUI>();

    /// <summary>
    /// �����ܷ�������Ʒ
    /// </summary>
    private bool IfMake = true;

    /// <summary>
    /// ���ڱ�ǵ�ǰ������Ϊ��һ����Ʒ��0����������1����������2������ë����Ĭ��Ϊ-1
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
        Debug.Log("MakeManger�ű�ִ��Awake()");
#endif
    }

    private void OnEnable()
    {
        MakeDeBug();
        //��������UI
        Instance.transform.Find("UI/Make/MakeUI").gameObject.SetActive(false);
#if DEBUG_MODE
        Debug.Log("MakeManger�ű�ִ��OnEnable()");
#endif

    }

    /// <summary>
    /// ͨ����ť��������ֵ����ʾ��ͬ��Ʒ����������
    /// ���˼·��ÿ��������Ʒ��ť��Ӧһ��������Ʒ�������ťʱ��MakeTag�޸�Ϊ������Ʒ��ID���Լ����������ʲô��Ʒ��Ȼ����ʾ���Ҳ�������UI��������UIˢ��
    /// </summary>
    /// <param name="MakeId">��Ӧ��Ʒ��ID</param>
    public static void ChooseMakeUI(int MakeId)
    {
        //��¼��ǰ��������Ʒ
        Instance.MakeTag = MakeId;
#if DEBUG_MODE
        Debug.Log("��ǰ������ƷΪ��" + Instance.MakeFormulaUIlist[Instance.MakeTag].MakeName + " ��ӦMakeTagΪ��" + Instance.MakeTag);
#endif
        //ˢ��������������
        Instance.transform.Find("UI/Make/MakeUI").gameObject.SetActive(true);
        RefreshMakeInfo(Instance.MakeFormulaUIlist[Instance.MakeTag]);
        RefreshFormulaList(1);
    }

    /// <summary>
    /// ˢ���Ҳ��ϱߵ�������Ʒ��Ϣ
    /// ���˼·��ͨ��MakeTag�������ʲô��Ʒ��Ȼ���ȡ��Ʒ�䷽������MakeUIģ�棬�滻��Ӧ��������ʾ����ͬ������Ʒ��������UI
    /// </summary>
    /// <param name="makeFormulaUI">��Ӧ�������䷽��Ϣ</param>
    private static void RefreshMakeInfo(MakeFormulaUI makeFormulaUI)
    {
        //���Ҳ�������������UI��Ӧ�����滻Ϊʵ��UI�����ಿ�ֱ��ֲ���
        //�����Ӧ�����䷽������֮����չʹ��
        Instance.MakeUI.MakeFormula = makeFormulaUI;
        Instance.MakeUI.MakeImage.sprite = makeFormulaUI.MakeImage;
        Instance.MakeUI.MakeName.text = makeFormulaUI.MakeName;
        Instance.MakeUI.ItemNum.text = BagManager.GetItemSum(makeFormulaUI.MakeId).ToString();
        Instance.MakeUI.Makeinfo.text = makeFormulaUI.MakeInfo;
    }

    /// <summary>
    /// �ڵ������ɶ�Ӧ��Ʒ�������ز��б�
    /// ���˼·������MakeTag���ǰ��������Ʒ��Ȼ���ȡ��Ʒ�䷽�����������б�֮����Formula���������ɵ�������UIģ�棬���滻��Ӧ����ʹ��ת��Ϊ��Ӧ����UI��
    /// �ٳ��Ե�ǰ����������ʾ�����ղ���������Ŀ����󽫱�����Ʒ��Ŀ�����������Ŀ���бȽϣ���С�������������ı���첢���IfMakeΪfalse�������޷���������Ʒ��
    /// ֮��ѭ���ò���ֱ���������UIȫ�����ɡ�
    /// </summary>
    /// <param name="makeFormulaUI">��Ӧ��������Ʒ</param>
    private static void ShowFormulaListUI(MakeFormulaUI makeFormulaUI)
    {
        //��ȡ��������
        int makeNum = GetMakeNum();
        //��ʼ��Ϊtrue
        Instance.IfMake = true;
        //ͨ��ѭ����̬�����������Ĳ����б������������ڼ�ʹֻ��Ҫ2�ֻ�1�ֲ���ʱҲ���Զ�������ʾ��������Ϊ�޸�
        for (int i = 0; i < makeFormulaUI.FormulaElemList.Count; i++)
        {
            //����������б�������ģ��������
            GameObject FormulaPrefabElem = Instantiate(Instance.FormulaPrefab, Instance.Formula.transform);
            //��ģ����ͼ�滻Ϊʵ�ʲ�����ͼ
            FormulaPrefabElem.transform.Find("FormulaImage/ItemImage").GetComponent<Image>().sprite = makeFormulaUI.FormulaElemList[i].NeedImage;
            int ItemSum = BagManager.GetItemSum(makeFormulaUI.FormulaElemList[i].NeedId);
            //��ģ���ı�ͨ���ַ�����ֵ�ķ�ʽ���滻Ϊʵ�ʶ�Ӧ�ı�
            TMP_Text PrefabText = FormulaPrefabElem.transform.Find("FormulaNum").GetComponent<TextMeshProUGUI>();
            PrefabText.text = $"({ItemSum} / {makeFormulaUI.FormulaElemList[i].NeedNum * makeNum}) {makeFormulaUI.FormulaElemList[i].NeedName}";
            //�������������������������������ı���Ϊ��ɫ
            if (ItemSum < makeFormulaUI.FormulaElemList[i].NeedNum * makeNum)
            {
                PrefabText.color = new Color(165f / 255f, 0f / 255f, 0f / 255f, 255f / 255f);
                //ֻҪ��һ���������������޷�����
                Instance.IfMake = false;
            }
        }
    }

    /// <summary>
    /// ����������Ʒ�������������뱳����Ʒ��Ӳ��ɳ��������������
    /// ���˼·��ͨ��������Ӱ�ť�������������������������жϣ�����������᲻�ᳬ�������������������Ļ�����޸�����������ˢ�µ��µ���������б�ʹ��
    /// ���ݵ�ǰ������������ʾ
    /// </summary>
    public static void AddMakeNum()
    {
        int addMakeNum = GetMakeNum() + 1;
        if (!IfMakeOut(addMakeNum)) RefreshFormulaList(addMakeNum);
#if DEBUG_MODE
        else Debug.Log("��������������������Ʒ����");
#endif
    }

    /// <summary>
    /// ����������Ʒ����������С��1
    /// ���˼·��ͨ��������ٰ�ť����������������������С��1�������������޸�����������ˢ�µ��µ���������б�ʹ����ݵ�ǰ������������ʾ
    /// </summary>
    public static void SubMakeNum()
    {
        int subMakeNum = GetMakeNum() - 1;
        if (subMakeNum > 0) RefreshFormulaList(subMakeNum);
#if DEBUG_MODE
        else Debug.Log("������Ʒ��������С��1");
#endif
    }

    /// <summary>
    /// ���������Ĳ������ĺ���Ʒ�Ļ��
    /// ���˼·��ͨ��IfMake�жϵ�ǰ�Ƿ���������������ԵĻ������������ť��ͨ��MakeTag��ȡ��������б��ٳ�����������ͨ��ѭ�����δӱ����۳���Ӧ����������
    /// ������ٻ�ö�Ӧ����������������Ʒ���������ˢ�µײ���������б������������ص���1
    /// </summary>
    public static void MakeButton()
    {
        if (Instance.IfMake)
        {
            //��ֹ�ڱ����������������Ȼ������һ����Ʒ����ΪĬ����ʾ��������һ������һ����Ʒ�����������������
            if (BagManager.GetBagNowCapa() < BagManager.GetBagMax())
            {
                foreach (var formulaElem in Instance.MakeFormulaUIlist[Instance.MakeTag].FormulaElemList)
                {
                    //���Ķ�Ӧ����
                    BagManager.SubItemsOnTable(BagManager.GetItemUIList()[formulaElem.NeedId], formulaElem.NeedNum * GetMakeNum());
                }
                //��ȡ��Ӧ��Ʒ
                BagManager.AddItemsOnTable(BagManager.GetItemUIList()[Instance.MakeTag], GetMakeNum());
#if DEBUG_MODE
                Debug.Log("�ɹ�����" + Instance.MakeFormulaUIlist[Instance.MakeTag].MakeName + GetMakeNum() + "��");
#endif
                RefreshFormulaList(1);
                //ˢ�¶�Ӧ��Ʒ������
                Instance.MakeUI.ItemNum.text = BagManager.GetItemSum(Instance.MakeTag).ToString();
            }
#if DEBUG_MODE
            else Debug.Log("��������");
#endif
        }
#if DEBUG_MODE
        else Debug.Log("δ������������");
#endif
    }

    /// <summary>
    /// ���ƼӼ���ť͸����
    /// ���˼·�����ڼ���ť����������Ϊ1����ʹ���Ϊ��͸������֮������ʾ�����ڼӰ�ť���������������ϼ�һ�����ж��Ƿ�ᳬ�����������������Ļ�ʹ���Ϊ��͸����
    /// ������������ʾ
    /// </summary>
    private static void ButtonTranControl()
    {
        //��������Ϊ1�������ť͸������֮�ָ�ԭ��
        if (GetMakeNum() == 1) ButtonTran(Instance.MakeUI.SubButton);
        else ButtonUnTran(Instance.MakeUI.SubButton);
        //����ǰ������+1�ᳬ������������ʹ�Ӱ�ť͸������֮�ָ�ԭ��
        if (IfMakeOut(GetMakeNum() + 1)) ButtonTran(Instance.MakeUI.AddButton);
        else ButtonUnTran(Instance.MakeUI.AddButton);
    }

    /// <summary>
    /// ˢ�µײ��Ĳ����б�����������ť״̬
    /// ���˼·����ˢ��������������ֵ�����Ƴ����������б�Ȼ��������������ˢ�²�����ʾ��������ж���������ˢ�°�ť״̬
    /// </summary>
    /// <param name="MakeNum">������</param>
    private static void RefreshFormulaList(int MakeNum)
    {
        //ˢ��������
        Instance.MakeUI.MakeNum.text = $"����x{MakeNum}";
        //�Ƴ��ײ����ϱ�
        RemoveFormulaListUI();
        //�������ɵײ����ϱ��Դ�ˢ������
        ShowFormulaListUI(Instance.MakeFormulaUIlist[Instance.MakeTag]);
        //ˢ�°�ť״̬
        ButtonTranControl();
    }

    /// <summary>
    /// ɾ�����²����б�
    /// </summary>
    private static void RemoveFormulaListUI()
    {
        //����������������б����������UIȫ��ɾ��
        for (int i = 0; i < Instance.Formula.transform.childCount; i++)
        {
            //ɾ����ǰ��Ʒ
            Destroy(Instance.Formula.transform.GetChild(i).gameObject);
        }
    }

    /// <summary>
    /// ��ȡҪ��������Ʒ����
    /// </summary>
    /// <returns>���ض�Ӧ����</returns>
    private static int GetMakeNum()
    {
        //ͨ��������ʽ��ȡ��ǰҪ��������Ʒ�����ı�������ȡx����Ķ������
        string MakeNum = Regex.Match(Instance.MakeUI.MakeNum.text, @"x(\d+)").Groups[1].Value;
        return int.Parse(MakeNum);
    }

    /// <summary>
    /// �жϵ�ǰ�������᲻�ᳬ�������������
    /// </summary>
    /// <param name="addMakeNum">��ǰ������</param>
    /// <returns>�����򷵻�true��δ�����򷵻�false</returns>
    private static bool IfMakeOut(int addMakeNum)
    {
        if (Instance.MakeTag != -1)
        {
            if (Instance.MakeTag == 0 || Instance.MakeTag == 1)
            {
                //��Ϊ����Ϊһ�����Ʒ
                if (BagManager.GetBagNowCapa() + addMakeNum > BagManager.GetBagMax()) return true;
                else return false;
            }
            else
            {
                //��Ϊ���Ϊһ�����Ʒ��������Ʒ����+��������/����Ʒ���ѵ���������ȡ�������������Ʒ��ռ�õĸ��������ټ�ȥδ����ǰ��Ʒռ�õĸ��������Դ˻������������Ʒ��Ҫ�ĸ��������ٺ����ø�������ӽ����ж�
                if ((int)Math.Ceiling(((double)(BagManager.GetItemSum(Instance.MakeTag) + addMakeNum)) / BagManager.GetItemUIList()[Instance.MakeTag].ItemMax) - BagManager.GetItemGridSum(Instance.MakeTag) + BagManager.GetBagNowCapa() > BagManager.GetBagMax())
                    return true;
                else return false;
            }
        }
        else
        {
#if DEBUG_MODE
            Debug.LogWarning("������ǩδ���и�ֵ");
#endif
            //Ĭ�Ϸ���false
            return false; 
        }
    }

    /// <summary>
    /// ����ť��Ϊ͸��
    /// </summary>
    /// <param name="button">Ҫ����İ�ť</param>
    private static void ButtonTran(GameObject button)
    {
        button.transform.GetComponent<Image>().color = new Color(1, 1, 1, 120f / 255f);
        button.transform.GetChild(0).GetComponent<TextMeshProUGUI>().color = new Color(1, 1, 1, 120f / 255f);
    }

    /// <summary>
    /// ����ť�ָ�ԭ��
    /// </summary>
    /// <param name="button">Ҫ����İ�ť</param>
    private static void ButtonUnTran(GameObject button)
    {
        button.transform.GetComponent<Image>().color = Color.white;
        button.transform.GetChild(0).GetComponent<TextMeshProUGUI>().color = Color.white;
    }

    /// <summary>
    /// ������Ϣ
    /// </summary>
    private static void MakeDeBug()
    {
#if DEBUG_MODE
        if (Instance.MakeUI == null) Debug.LogWarning("MakeUI/������UIδ��ֵ");
        if (Instance.FormulaPrefab == null) Debug.LogWarning("FormulaPrefab/����UIԤ����δ��ֵ");
        if (Instance.Formula == null) Debug.LogWarning("Formula/�����б�����δ��ֵ");
#endif
    }
}
