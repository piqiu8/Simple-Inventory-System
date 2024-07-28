using SQLite4Unity3d;

/// <summary>
/// �����洢���ݱ��һ������
/// </summary>
//��item_table���ݱ���й���
[Table("item_table")]
public class ItemTable
{
    /// <summary>
    /// ��Ϊ��������Ψһ�Լ�¼��������
    /// </summary>
    [PrimaryKey, AutoIncrement]
    public int RecordId { get; set; }

    /// <summary>
    /// ��¼��ʲô��Ʒ
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// ��Ʒ����
    /// </summary>
    public int ItemNum { get; set; }

    /// <summary>
    /// ��Ʒ����
    /// </summary>
    public string ItemType { get; set; }
}
