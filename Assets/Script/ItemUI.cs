using UnityEngine;

/// <summary>
/// ������Ʒ��Ӧ���Բ������ڱ��أ����а�����Ʒ�����ơ���ͼ�����������͡����ѵ���
/// </summary>
//��unity���������ɿɴ���NewItem�ļ���Bag�˵����ļ�Ĭ����ΪNewNewItem
[CreateAssetMenu(fileName = "NewItem",menuName = "Bag/NewItem")]
public class ItemUI : ScriptableObject
{
    /// <summary>
    /// ��Ʒ��Ӧid����ʼֵΪ-1
    /// </summary>
    public int ItemId = -1;

    /// <summary>
    /// ��Ʒ��Ӧ����
    /// </summary>
    public string ItemName = string.Empty;

    /// <summary>
    /// ��Ʒ��Ӧ��ͼ
    /// </summary>
    public Sprite ItemImage = null;

    /// <summary>
    /// ��Ʒ�Ĳ�ͬ����
    /// </summary>
    public enum ItemType{ ����,����,��ҩ,δ������ }

    /// <summary>
    /// ��Ʒ�Ķ�Ӧ����
    /// </summary>
    public ItemType Type = ItemType.δ������;

    /// <summary>
    /// ��Ʒ�����ѵ���
    /// </summary>
    public int ItemMax = 1;
}
