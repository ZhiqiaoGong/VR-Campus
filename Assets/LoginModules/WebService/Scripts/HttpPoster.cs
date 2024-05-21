using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using System;
using System.Collections.Generic;

namespace WebService
{
    
    public enum Url
    {
        localhost,
        tecent_cloud_name,
        tecent_cloud_ip
    }





    [Serializable]
    public class HttpPoster:MonoBehaviour
    {

        static string baseUrl;

        public Url url = 0;
        private string[] urlList = { 
            "http://localhost:8080/player/" , 
            "http://lizhuodong.club:8080/player/",
            "http://1.116.155.101:8080/player/"
        };


        UIInterface UICallBack;
        InitOutput initOutput;

        long userId;
        long graphId;


        void Awake()
        {
            UICallBack = GameObject.Find("UI").GetComponent<UIInterface>();
            initOutput = GameObject.Find("Init").GetComponent<InitOutput>();
            baseUrl = urlList[(int)url];
        }

        public IEnumerator GetUsersList()
        {
            //UnityWebRequess对象应该是不能复用的
            UnityWebRequest www = UnityWebRequest.Get(baseUrl+"getUsersList");
            //yield return SendWebRequest()会让协程暂停直至请求出错或完成
            yield return www.SendWebRequest();
            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
            }
            else
            {
                string res = www.downloadHandler.text;
                res = JsonManager.ValueParser(res, "data");
                // 将结果显示为文本
                //Debug.Log(res);
                List<User> userList = JsonManager.JsonArray2List<User>(res);
                Debug.Log("UserList:"+JsonManager.toJsonString(userList));
                
                //调用ui
                UICallBack.ShowUsersList(userList);
            }
        }

        public IEnumerator GetGraphsList(long userId)
        {
            this.userId = userId;

            UnityWebRequest www = UnityWebRequest.Get(baseUrl+"viewGraphs?userId="+userId.ToString());
            //yield return SendWebRequest()会让协程暂停直至请求出错或完成
            yield return www.SendWebRequest();
            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
            }
            else
            {
                string res = www.downloadHandler.text;
                res = JsonManager.ValueParser(res, "data");
                // 将结果显示为文本
                //Debug.Log(res);
                List<Graph> graphs = JsonManager.JsonArray2List<Graph>(res);
                Debug.Log("user "+userId+"'s GraphList:"+JsonManager.toJsonString(graphs));

                UICallBack.ShowGraphsList(graphs);
            }
        }

        public IEnumerator ChoseGraph(long graphId)
        {

            this.graphId = graphId;

            UnityWebRequest www = UnityWebRequest.Get(baseUrl + "choseGraph?id=" + graphId.ToString());
            //yield return SendWebRequest()会让协程暂停直至请求出错或完成
            yield return www.SendWebRequest();
            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
            }
            else
            {
                string res = www.downloadHandler.text;
                res = JsonManager.ValueParser(res, "data");
                // 将结果显示为文本
                //Debug.Log(res);
                string pathData = JsonManager.ValueParser(res, "pathData");
                string nodes = JsonManager.ValueParser(res, "nodes");
                List<PathData> PathDataList = JsonManager.JsonArray2List<PathData>(pathData);
                List<Node> nodesList = JsonManager.JsonArray2List<Node>(nodes);
                Debug.Log("Graph " + graphId + "'s pathdata:" + JsonManager.toJsonString(PathDataList));
                Debug.Log("Graph " + graphId + "'s nodes:" + JsonManager.toJsonString(nodesList));

                if (this.graphId == 0 || this.userId == 0)
                {
                    Debug.Log("未选择公司或图");
                }
                else
                {
                    //加载下一个场景
                    initOutput.Init(nodesList, PathDataList, this.userId, this.graphId);
                }
            }
        }


    }

}

