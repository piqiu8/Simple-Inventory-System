using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ������Ʒ�Ķ�Ӧ�䷽�����а���������Ʒ���ơ�������Ʒ�����������������Ʒ��ͼ����������Ĳ����б�
/// </summary>
//��unity���������ɿɴ���NewFormula�ļ���Bag�˵����ļ�Ĭ����ΪNewFormula
[CreateAssetMenu(fileName ="NewFormula",menuName ="Bag/NewFormula")]
public class MakeFormulaUI : ScriptableObject
{
    /// <summary>
    /// ��ǰ������Ʒ��Ӧ��ID
    /// </summary>
    public int MakeId=-1;
    
    /// <summary>
    /// ��ǰ������Ʒ��Ӧ������
    /// </summary>
    public string MakeName = null;

    /// <summary>
    /// ��ǰ������Ʒ��Ӧ����ͼ
    /// </summary>
    public Sprite MakeImage = null;

    /// <summary>
    /// ��ǰ������Ʒ��Ӧ������
    /// </summary>
    [TextArea]
    public string MakeInfo = string.Empty;

    /// <summary>
    /// �������������Ʒ���������Ʒ�װΪ�ṹ�壬ʹ�䱣��һ�·���������������л�ʹ���ڽ�������ʾ
    /// </summary>
    [System.Serializable]
    public struct FormulaElem
    {
        /// <summary>
        /// �������϶�Ӧ��ID
        /// </summary>
        public int NeedId;

        /// <summary>
        /// �������϶�Ӧ����ͼ
        /// </summary>
        public Sprite NeedImage;

        /// <summary>
        /// �������϶�Ӧ������
        /// </summary>
        public int NeedNum;

        /// <summary>
        /// �������϶�Ӧ������
        /// </summary>
        public string NeedName;
        public FormulaElem(int needId, Sprite needImage, int needNum, string needName)
        {
            NeedId = needId;
            NeedImage = needImage;
            NeedNum = needNum;
            NeedName = needName;
        }
    }

    /// <summary>
    /// ���������б�
    /// </summary>
    public List<FormulaElem> FormulaElemList = new List<FormulaElem>();
}
