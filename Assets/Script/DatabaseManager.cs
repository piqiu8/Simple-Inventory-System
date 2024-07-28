//#define DEBUG_MODE

using System.Collections.Generic;
using System.IO;
using System.Linq;
using SQLite4Unity3d;
using UnityEngine;

/// <summary>
/// 数据库管理脚本，包含连接数据库、获取全部数据、插入数据、更新数据、删除数据功能
/// </summary>
public class DatabaseManager : MonoBehaviour
{
    /// <summary>
    /// 数据库连接实例
    /// </summary>
    private SQLiteConnection db;

    void Awake()
    {
        //连接数据库
        string dbPath = Path.Combine(Application.streamingAssetsPath, "item_db.db");
        db = new SQLiteConnection(dbPath,SQLiteOpenFlags.ReadWrite | SQLiteOpenFlags.Create);
#if DEBUG_MODE
        Debug.Log("成功连接数据库： " + dbPath);
#endif
    }

    /// <summary>
    /// 获取所有物品数据
    /// </summary>
    /// <returns>将数据表每行数据赋给ItemTable并插入一个新建列表进行返回</returns>
    public List<ItemTable> GetAllItems()
    {
        return db.Table<ItemTable>().ToList();
    }

    /// <summary>
    /// 插入新物品数据
    /// </summary>
    /// <param name="item">需要插入的一条物品数据，可以不写Record信息，因为其可自增</param>
    public void InsertItem(ItemTable item)
    {
        db.Insert(item);
#if DEBUG_MODE
        Debug.Log("数据成功插入数据表");
#endif
    }

    /// <summary>
    /// 修改物品数据
    /// </summary>
    /// <param name="item">需修改的一条物品数据</param>
    public void UpdateItem(ItemTable item)
    {
        db.Update(item);
#if DEBUG_MODE
        Debug.Log("成功修改数据表数据");
#endif
    }

    /// <summary>
    /// 删除物品数据
    /// </summary>
    /// <param name="item">需删除的一条物品数据</param>
    public void DeleteItem(ItemTable item)
    {
        db.Delete(item);
#if DEBUG_MODE
        Debug.Log("已从数据库删除对应数据");
#endif
    }
}
