using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GraphData;

public class DaoOutput : MonoBehaviour
{
    public enum Datasource
    {
        localSqlite,
        web
    }

    private List<PathData> pathDatas;
    private List<Node> nodes;
    public DataInterface baseDao;
    public Datasource datasource = Datasource.localSqlite;

    void Awake()
    {
        switch (datasource)
        {
            case Datasource.localSqlite:
                baseDao = transform.GetComponentInChildren<LocalSqliteDao>(); 
                break;
            case Datasource.web:
                baseDao = transform.GetComponentInChildren<WebData>();
                break;
            default:
                break;
        }
        pathDatas = baseDao.GetAllPathdatas();
        nodes = baseDao.GetAllNodes();
    }

    void Start()
    {

    }

    public List<PathData> GetPathDatas()
    {
        return pathDatas;
    }

    public List<Node> GetNodes()
    {
        return nodes;
    }

    /// <summary>
    /// 获取本地视频存储根路径
    /// </summary>
    /// <returns></returns>
    public string GetBaseUrl()
    {
        return baseDao.GetLocalStorageUrl();
    }


    /// <summary>
    /// 获取邻接节点
    /// </summary>
    /// <param name="node">搜索的节点</param>
    /// <returns>一个列表 参数节点邻接的节点的列表</returns>
    public List<Node> GetAdjacencyNode(Node node)
    {
        List<Node> res = new List<Node>();
        int id = node.id;
        foreach (var item in pathDatas)
        {
            if (item.start == id || item.end == id)
            {
                //另一端的id
                int adj_id = item.start == id ? item.start : item.end;
                
                bool if_exist = false;
                //确保该id不在res中，因为未来可能会有双向的路径
                foreach (var re in res) if (re.id == adj_id) if_exist = true;
                if (!if_exist) foreach (var n in nodes) if (n.id == adj_id) res.Add(n);
            }
        }
        return res;
    }



    public void testDao()
    {
        Debug.Log("==================================开始测试数据库接口：================================");
        foreach (var node in nodes)
        {
            
            Debug.Log("节点序号" + node.name);
        }
        foreach (var path in pathDatas)
        {
            Debug.Log("vUrl：" + path.vUrl);
        }
        Debug.Log("==================================数据库接口测试结束！===================================");
    }

}



