using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using GraphData;
using UnityEngine;
using UnityEngine.Networking;

public class VideoDownloader: MonoBehaviour {

    // 视频请求根路径
    string server_base_url;
    // 视频存储根路径
    string local_base_url;

    // 路径信息
    List<PathData> pathDatas;

    public bool[] status;




    // Use this for initialization
    void Start()
    {
        DaoOutput dao = GameObject.Find("Dao").GetComponent<DaoOutput>();
        if (dao.datasource == DaoOutput.Datasource.localSqlite)
        {
            return;
        }
        server_base_url = dao.baseDao.GetSourceUrl();
        var param_array = server_base_url.Split('/');
        local_base_url = dao.GetBaseUrl();
        pathDatas = dao.GetPathDatas();
        status = new bool[pathDatas.Count];
        for (int i = 0; i < pathDatas.Count; i++)
        {
            if (!File.Exists(local_base_url + pathDatas[i].vUrl))
                StartCoroutine(Download(server_base_url + pathDatas[i].vUrl, local_base_url + pathDatas[i].vUrl,i));
            else
            {
                //TODO 让用户交互地选择是否要替换视频文件
            }
        }
    }


    IEnumerator Download(string sourceUrl, string targetUrl,int index)
    {
        Debug.Log("开始下载:"+sourceUrl+"至"+targetUrl);
        string path = targetUrl;
        if (!System.IO.Directory.Exists(path))
        {
            var uwr = new UnityWebRequest(sourceUrl, UnityWebRequest.kHttpVerbGET);
            uwr.downloadHandler = new DownloadHandlerFile(path);
            yield return uwr.SendWebRequest();
            //>300说明请求失败
            if (uwr.responseCode >= 300)
            {
                //请求失败则删除写入文件
                if (File.Exists(targetUrl))
                {
                    File.Delete(targetUrl);//删除该文件
                }
                Debug.LogError(uwr.downloadHandler.text);
            }
            else
            {
                Debug.Log("File successfully downloaded and saved to " + path);
                status[index] = true;
            }
        }
    }
}
