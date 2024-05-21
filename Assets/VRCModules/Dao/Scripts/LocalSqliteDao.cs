using Mono.Data.Sqlite;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace GraphData
{
    public class LocalSqliteDao : MonoBehaviour,DataInterface
    {
        public List<Node> GetAllNodes()
        {
            SqliteDataReader s = ExecuteQuery("select * from Node");
            return NodeMapping(s);
        }

        public string GetLocalStorageUrl()
        {
            return Environment.CurrentDirectory.Replace("\\", "/") + "/video/localUser/软件园校区/";
        }

        public List<PathData> GetAllPathdatas()
        {
            SqliteDataReader s = ExecuteQuery("select * from \"PathData\"");
            return PathDataMapping(s);
        }

        private static LocalSqliteDao _dbInstance = null;
        public static LocalSqliteDao _DBInstance()
        {
            return _dbInstance;
        }

        private string dbName = "VRCampusData";     //数据库名称

        //建立数据库连接
        SqliteConnection connection;
        //数据库命令
        SqliteCommand command;
        //数据库阅读器
        SqliteDataReader reader;

        private void Awake()
        {
            //连接数据库
            OpenConnect();
        }

        private void OnDestroy()
        {
            //断开数据库连接
            CloseDB();
        }

        // 执行SQL命令
        public SqliteDataReader ExecuteQuery(string queryString)
        {
            command = connection.CreateCommand();
            command.CommandText = queryString;
            reader = command.ExecuteReader();
            return reader;
        }

        public void OpenConnect()
        {
            try
            {
                //数据库存放在 Asset/StreamingAssets
                string path = Application.streamingAssetsPath + "/" + "database/" + dbName + ".sqlite";
                //新建数据库连接
                connection = new SqliteConnection(@"Data Source = " + path);
                //打开数据库
                connection.Open();
                Debug.Log("打开数据库");
            }
            catch (Exception ex)
            {
                Debug.Log(ex.ToString());
            }
        }

        //关闭数据库
        public void CloseDB()
        {
            if (command != null)
            {
                command.Cancel();
            }
            command = null;

            if (reader != null)
            {
                reader.Close();
            }
            reader = null;

            if (connection != null)
            {
                //connection.Close();
            }
            connection = null;

            Debug.Log("关闭数据库");
        }


        public List<Node> NodeMapping(SqliteDataReader sqlite)
        {
            //string[] column_name =new string[] {"id","name","longitude","latidude"};
            List<Node> nodes = new List<Node>();
            while (sqlite.Read())
            {
                Node node = new Node();
                try
                {
                    node.id = sqlite.GetInt32(sqlite.GetOrdinal("id"));
                    node.name = sqlite.GetString(sqlite.GetOrdinal("name"));
                    node.latitude = sqlite.GetFloat(sqlite.GetOrdinal("latitude"));
                    node.longitude = sqlite.GetFloat(sqlite.GetOrdinal("longitude"));
                    nodes.Add(node);
                }
                catch (Exception)
                {

                    Debug.Log("Node记录映射错误");
                }
            }
            return nodes;
        }

        public List<PathData> PathDataMapping(SqliteDataReader sqlite)
        {
            List<PathData> pathDatas = new List<PathData>();
            while (sqlite.Read())
            {
                PathData path = new PathData();
                try
                {
                    path.start = sqlite.GetInt32(sqlite.GetOrdinal("start"));
                    path.end = sqlite.GetInt32(sqlite.GetOrdinal("end"));
                    path.vUrl = sqlite.GetString(sqlite.GetOrdinal("vUrl"));
                    path.length = sqlite.GetFloat(sqlite.GetOrdinal("length"));
                }
                catch (Exception)
                {

                    Debug.Log("Pathdata映射出错");
                }
                pathDatas.Add(path);
            }

            return pathDatas;
        }

        public string GetSourceUrl()
        {
            throw new NotImplementedException();
        }



        ///// <summary>
        ///// CS转化为DB类别
        ///// </summary>
        ///// <param name="type">c#中字段的类别</param>
        ///// <returns></returns>
        //string CS2DB(Type type)
        //{
        //    string result = "Text";
        //    if (type == typeof(Int32))
        //    {
        //        result = "Int";
        //    }
        //    else if (type == typeof(String))
        //    {
        //        result = "Text";
        //    }
        //    else if (type == typeof(Single))
        //    {
        //        result = "FLOAT";
        //    }
        //    else if (type == typeof(Boolean))
        //    {
        //        result = "Bool";
        //    }
        //    return result;
        //}

        ///// <summary>
        ///// 向指定数据表中插入数据
        ///// </summary>
        ///// <param name="tableName"></param>
        ///// <param name="values"></param>
        ///// <returns></returns>
        //public SqliteDataReader InsertValues(string tableName, string[] values)
        //{
        //    string sql = "INSERT INTO " + tableName + " values (";
        //    foreach (var item in values)
        //    {
        //        sql += "'" + item + "',";
        //    }
        //    sql = sql.TrimEnd(',') + ")";

        //    Debug.Log("插入成功");
        //    return ExecuteQuery(sql);
        //}

        ///// <summary>
        ///// 插入数据
        ///// </summary>
        ///// <typeparam name="T"></typeparam>
        ///// <param name="t"></param>
        ///// <returns></returns>
        //public SqliteDataReader Insert<T>(T t)
        //{
        //    var type = typeof(T);
        //    var fields = type.GetFields();
        //    string sql = "INSERT INTO " + type.Name + " values (";

        //    foreach (var field in fields)
        //    {
        //        //通过反射得到对象的值
        //        sql += "'" + type.GetField(field.Name).GetValue(t) + "',";
        //    }
        //    sql = sql.TrimEnd(',') + ");";

        //    Debug.Log("插入成功");
        //    return ExecuteQuery(sql);
        //}


        ///// <summary>
        ///// 更新数据
        ///// </summary>
        ///// <param name="tableName"></param>
        ///// <param name="values">需要修改的数据</param>
        ///// <param name="conditions">修改的条件</param>
        ///// <returns></returns>
        //public SqliteDataReader UpdataData(string tableName, string[] values, string[] conditions)
        //{
        //    string sql = "update " + tableName + " set ";
        //    for (int i = 0; i < values.Length - 1; i += 2)
        //    {
        //        sql += values[i] + "='" + values[i + 1] + "',";
        //    }
        //    sql = sql.TrimEnd(',') + " where (";
        //    for (int i = 0; i < conditions.Length - 1; i += 2)
        //    {
        //        sql += conditions[i] + "='" + conditions[i + 1] + "' and ";
        //    }
        //    sql = sql.Substring(0, sql.Length - 4) + ");";
        //    Debug.Log("更新成功");
        //    return ExecuteQuery(sql);
        //}


        ///// <summary>
        ///// 删除数据
        ///// </summary>
        ///// <param name="tableName"></param>
        ///// <param name="conditions">查询条件</param>
        ///// <returns></returns>
        //public SqliteDataReader DeleteValues(string tableName, string[] conditions)
        //{
        //    string sql = "delete from " + tableName + " where (";
        //    for (int i = 0; i < conditions.Length - 1; i += 2)
        //    {
        //        sql += conditions[i] + "='" + conditions[i + 1] + "' and ";
        //    }
        //    sql = sql.Substring(0, sql.Length - 4) + ");";
        //    return ExecuteQuery(sql);
        //}

        //打开数据库

    }
    public struct PathData
    {
        public float length;
        public int start;
        public int end;
        public string vUrl;
    }

    public struct Node
    {
        public int id;
        public string name;
        public float longitude;
        public float latitude;
    }

}
