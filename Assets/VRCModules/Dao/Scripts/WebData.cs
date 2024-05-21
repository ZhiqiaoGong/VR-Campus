using System;
using System.Collections;
using System.Collections.Generic;
using GraphData;
using UnityEngine;

public class WebData : MonoBehaviour,DataInterface {

    //来自login场景的遗产
    InitOutput Init;

    List<Node> nodes;
    List<PathData> pathDatas;

    long userId;
    long graphId;
   

    // Use this for initialization
    void Awake()
    {
        Init = GameObject.Find("Init").GetComponent<InitOutput>();
        if (Init == null)
        {
            return;
        }
     
        

        //web数据
        List<WebService.PathData> webPathDatas = Init.pathDatas;
        List<WebService.Node> webNodes = Init.nodes;
        this.userId = Init.userId;
        this.graphId = Init.graphId;

        //本地数据
        nodes = new List<Node>();
        pathDatas = new List<PathData>();


        foreach (var item in webPathDatas)
        {
            pathDatas.Add(web2local(item));
        }

        foreach (var item in webNodes)
        {
            nodes.Add(web2local(item));
        }
        Destroy(Init.gameObject);

        
    }

    List<Node> DataInterface.GetAllNodes()
    {
        return nodes;
    }

    List<PathData> DataInterface.GetAllPathdatas()
    {
        return pathDatas;
    }

    public string GetLocalStorageUrl()
    {
        return Environment.CurrentDirectory.Replace("\\", "/") + "/video/" + userId + "/" + graphId + "/";
    }
     public string GetSourceUrl()
    {
        return "http://1.116.155.101//file/video/" + userId + "/" + graphId + "/";
    }

    PathData web2local(WebService.PathData webData)
    {
        var res = new PathData();
        res.start = (int)webData.start;
        res.end = (int)webData.end;
        res.vUrl = webData.vurl;
        res.length = (float)webData.length;
        return res;
    }

    Node web2local(WebService.Node webData)
    {
        var res = new Node();
        res.id = (int)webData.id;
        res.latitude = (float)webData.latitude;
        res.longitude = (float)webData.longitude;
        res.name = webData.name;
        return res;
    }

   
}
