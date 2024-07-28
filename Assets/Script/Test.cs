//#define DEBUG_MODE

using UnityEngine;

/// <summary>
/// ���Ա��������Ƿ��������ýű�
/// </summary>
public class Test:MonoBehaviour
{
    /// <summary>
    /// ��Ӧ��������
    /// </summary>
    public ItemUI Item = null;
    public int AddNum = 0, SubNum = 0;

    private void Start()
    {
#if DEBUG_MODE
        if (Item == null) Debug.LogWarning("Item/��Ӧ��Ʒδ��ֵ");
#endif
    }

    /// <summary>
    /// �򱳰��������Ʒ
    /// </summary>
    public void AddItem()
    {
        BagManager.AddItemsOnTable(Item, AddNum);
    }

    /// <summary>
    /// ���ٱ�����Ʒ������
    /// </summary>
    public void SubItem()
    {
        BagManager.SubItemsOnTable(Item, SubNum);
    }
}
