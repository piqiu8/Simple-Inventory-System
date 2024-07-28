using SQLite4Unity3d;

/// <summary>
/// 用来存储数据表的一行数据
/// </summary>
//与item_table数据表进行关联
[Table("item_table")]
public class ItemTable
{
    /// <summary>
    /// 设为主键用于唯一性记录，且自增
    /// </summary>
    [PrimaryKey, AutoIncrement]
    public int RecordId { get; set; }

    /// <summary>
    /// 记录是什么物品
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// 物品数量
    /// </summary>
    public int ItemNum { get; set; }

    /// <summary>
    /// 物品类型
    /// </summary>
    public string ItemType { get; set; }
}
