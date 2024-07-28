//#define DEBUG_MODE

using System.Collections.Generic;
using System.IO;
using System.Linq;
using SQLite4Unity3d;
using UnityEngine;

/// <summary>
/// ���ݿ����ű��������������ݿ⡢��ȡȫ�����ݡ��������ݡ��������ݡ�ɾ�����ݹ���
/// </summary>
public class DatabaseManager : MonoBehaviour
{
    /// <summary>
    /// ���ݿ�����ʵ��
    /// </summary>
    private SQLiteConnection db;

    void Awake()
    {
        //�������ݿ�
        string dbPath = Path.Combine(Application.streamingAssetsPath, "item_db.db");
        db = new SQLiteConnection(dbPath,SQLiteOpenFlags.ReadWrite | SQLiteOpenFlags.Create);
#if DEBUG_MODE
        Debug.Log("�ɹ��������ݿ⣺ " + dbPath);
#endif
    }

    /// <summary>
    /// ��ȡ������Ʒ����
    /// </summary>
    /// <returns>�����ݱ�ÿ�����ݸ���ItemTable������һ���½��б���з���</returns>
    public List<ItemTable> GetAllItems()
    {
        return db.Table<ItemTable>().ToList();
    }

    /// <summary>
    /// ��������Ʒ����
    /// </summary>
    /// <param name="item">��Ҫ�����һ����Ʒ���ݣ����Բ�дRecord��Ϣ����Ϊ�������</param>
    public void InsertItem(ItemTable item)
    {
        db.Insert(item);
#if DEBUG_MODE
        Debug.Log("���ݳɹ��������ݱ�");
#endif
    }

    /// <summary>
    /// �޸���Ʒ����
    /// </summary>
    /// <param name="item">���޸ĵ�һ����Ʒ����</param>
    public void UpdateItem(ItemTable item)
    {
        db.Update(item);
#if DEBUG_MODE
        Debug.Log("�ɹ��޸����ݱ�����");
#endif
    }

    /// <summary>
    /// ɾ����Ʒ����
    /// </summary>
    /// <param name="item">��ɾ����һ����Ʒ����</param>
    public void DeleteItem(ItemTable item)
    {
        db.Delete(item);
#if DEBUG_MODE
        Debug.Log("�Ѵ����ݿ�ɾ����Ӧ����");
#endif
    }
}
