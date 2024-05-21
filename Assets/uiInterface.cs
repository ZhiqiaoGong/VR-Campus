using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using WebService;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO;
using System.Windows.Forms;

public class uiInterface : MonoBehaviour
{

    static string baseUrl;
    string token = "";
    List<Graph> g;
    List<Node> n;
    List<PathData> p;
    long id;
    int registerCode;
    int loginCode;


    public enum Url
    {
        localhost,
        tecent_cloud_name,
        tecent_cloud_ip
    }

    public Url url = 0;
    private string[] urlList = {
        "http://localhost:8080/" ,
        "http://lizhuodong.club:8080/",
        "http://1.116.155.101:8080/"
    };

    void Start()
    {
        baseUrl = urlList[(int)url];
        //StartCoroutine(Register("demouser", "demo"));
        //StartCoroutine(Login("3", "1232"));

        //StartCoroutine(ViewGraph());
        //openFile();
        openFile();
    }

    public string openFile()
    {
        //打开文件

        OpenFileDialog file = new OpenFileDialog();

        file.Filter = "TXT|*.txt";

        //file.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

        //file.Multiselect = false;

        if (file.ShowDialog() == DialogResult.Cancel)

            return null;

        var path = file.FileName;
        return path;//会返回全路径包含了文件名和后缀
    }





    public IEnumerator Register(string userName, string psw)
    {

        List<IMultipartFormSection> formData = new List<IMultipartFormSection>();
        string rqUrl = baseUrl + "user" + "/register?" + "userName=" + userName + "&password=" + psw;
        UnityWebRequest www = UnityWebRequest.Post(rqUrl, formData);
        yield return www.SendWebRequest();
        //
        if (www.responseCode >= 300)
        {
            Debug.Log(www.error);
        }
        else if (www.responseCode == 1)
        {
            Debug.Log("用户名已注册");
            registerCode = 1;
        }
        else
        {
            string res = www.downloadHandler.text;
            res = JsonManager.ValueParser(res, "data");
            id = long.Parse(JsonManager.ValueParser(res, "userId"));
            //返回并显示用户id
            registerCode = 200;
        }
    }


    public IEnumerator Login(string userId, string psw)
    {

        List<IMultipartFormSection> formData = new List<IMultipartFormSection>();
        string rqUrl = baseUrl + "user" + "/login?" + "userId=" + userId + "&password=" + psw;
        UnityWebRequest www = UnityWebRequest.Post(rqUrl, formData);
        yield return www.SendWebRequest();
        //
        if (www.responseCode >= 300)
        {
            Debug.Log(www.error);
        }
        else if (www.responseCode == 100)
        {
            Debug.Log("账号或密码错误");
            loginCode = 100;
        }
        else
        {
            string res = www.downloadHandler.text;
            res = JsonManager.ValueParser(res, "data");
            token = JsonManager.ValueParser(res, "token");
            //登录成功
            Debug.Log("登录成功");
            loginCode = 200;
        }
    }


    public IEnumerator CreatGraph(string graphName)
    {

        List<IMultipartFormSection> formData = new List<IMultipartFormSection>();
        string rqUrl = baseUrl + "GraphManager" + "/createGraph?" + "name=" + graphName;
        UnityWebRequest www = UnityWebRequest.Post(rqUrl, formData);
        www.SetRequestHeader("token", token);
        yield return www.SendWebRequest();
        //
        if (www.responseCode >= 300)
        {
            Debug.Log(www.error);
        }
        else
        {
            string res = www.downloadHandler.text;
            res = JsonManager.ValueParser(res, "message");
            Debug.Log(res);
            //创建成功
            Debug.Log("创建成功");
        }
    }


    public IEnumerator ViewGraph()
    {
        string url = "http://1.116.155.101:8080/GraphManager/viewMyGraphs";
        List<IMultipartFormSection> formData = new List<IMultipartFormSection>();
        UnityWebRequest www = UnityWebRequest.Get(url);

        JObject header = new JObject();
        //header.Add("token", "VXauOkMT5g+8LMWcs08oPVVzfd3NDWsZe/qE5cwHW8F4FbLvDGPXU0waZHy0siaV");
        //byte[] postBytes = System.Text.Encoding.Default.GetBytes(header.ToString());

        //www.uploadHandler = new UploadHandlerRaw(postBytes);
        //www.SetRequestHeader("token", "VXauOkMT5g+8LMWcs08oPVVzfd3NDWsZe/qE5cwHW8F4FbLvDGPXU0waZHy0siaV");
        www.SetRequestHeader("token", token);
        yield return www.SendWebRequest();
            

        if (www.responseCode >= 300)
        {
            Debug.Log(www.error);
        }
        string res = www.downloadHandler.text;
        Debug.Log(res);
        //res = JsonManager.ValueParser(res, "data");

        RootObject rb = JsonConvert.DeserializeObject<RootObject>(res);
        //用于保存所有的图信息
        List<Graph> graphs = new List<Graph>();
        foreach(Graph d in rb.data)
        {
            graphs.Add(d);
        }
        //显示所有图信息
        g = graphs;
    }


    public IEnumerator ChooseGraph(long id)
    {
        string url = "http://1.116.155.101:8080/GraphManager/choseGraph?"+"id="+id;
        List<IMultipartFormSection> formData = new List<IMultipartFormSection>();
        UnityWebRequest www = UnityWebRequest.Get(url);

        JObject header = new JObject();
        //header.Add("token", "VXauOkMT5g+8LMWcs08oPVVzfd3NDWsZe/qE5cwHW8F4FbLvDGPXU0waZHy0siaV");
        //byte[] postBytes = System.Text.Encoding.Default.GetBytes(header.ToString());

        //www.uploadHandler = new UploadHandlerRaw(postBytes);
        //www.SetRequestHeader("token", "VXauOkMT5g+8LMWcs08oPVVzfd3NDWsZe/qE5cwHW8F4FbLvDGPXU0waZHy0siaV");
        www.SetRequestHeader("token", token);
        yield return www.SendWebRequest();


        if (www.responseCode >= 300)
        {
            Debug.Log(www.error);
        }
        string res = www.downloadHandler.text;
        Debug.Log(res);
        res = JsonManager.ValueParser(res, "data");

        RootObject2 rb = JsonConvert.DeserializeObject<RootObject2>(res);
        Data2 dt = rb.data;

        //用于保存图中所有的节点
        List<Node> nodes = new List<Node>();
        //用于保存图中所有的路径
        List<PathData> paths = new List<PathData>();
        foreach (Node n in dt.nodes)
        {
            nodes.Add(n);
        }
        foreach (PathData p in dt.pathData)
        {
            paths.Add(p);
        }
        //显示所有节点和路径信息
        n = nodes;
        p = paths;
    }


    public IEnumerator AddNode(string nodeName, double longt, double latit)
    {

        List<IMultipartFormSection> formData = new List<IMultipartFormSection>();
        string rqUrl = baseUrl + "GraphEdit" + "/addNode?" + "Name=" + nodeName + "&longitude=" + longt + "&latitude=" + latit + "&nodeId=";
        UnityWebRequest www = UnityWebRequest.Post(rqUrl, formData);
        www.SetRequestHeader("token", token);
        yield return www.SendWebRequest();
        //
        if (www.responseCode >= 300)
        {
            Debug.Log(www.error);
        }
        else
        {
            string res = www.downloadHandler.text;
            res = JsonManager.ValueParser(res, "data");
            Debug.Log(res);
            Node node = new Node();
            node.id = int.Parse(JsonManager.ValueParser(res, "id"));
            node.name = JsonManager.ValueParser(res, "name");
            node.longitude = double.Parse(JsonManager.ValueParser(res, "longitude"));
            node.latitude = double.Parse(JsonManager.ValueParser(res, "latitude"));
        }
        //节点创建成功
    }
    public IEnumerator AddNode(string nodeName, double longt, double latit, int nodeid )
    {

        List<IMultipartFormSection> formData = new List<IMultipartFormSection>();
        string rqUrl = baseUrl + "GraphEdit" + "/addNode?" + "Name=" + nodeName + "&longitude=" + longt + "&latitude=" + latit + "&nodeId=" + nodeid;
        UnityWebRequest www = UnityWebRequest.Post(rqUrl, formData);
        www.SetRequestHeader("token", token);
        yield return www.SendWebRequest();
        //
        if (www.responseCode >= 300)
        {
            Debug.Log(www.error);
        }
        else
        {
            string res = www.downloadHandler.text;
            res = JsonManager.ValueParser(res, "data");
            Debug.Log(res);
            Node node = new Node();
            node.id = int.Parse(JsonManager.ValueParser(res, "id"));
            node.name = JsonManager.ValueParser(res, "name");
            node.longitude = double.Parse(JsonManager.ValueParser(res, "longitude"));
            node.latitude = double.Parse(JsonManager.ValueParser(res, "latitude"));
        }
        //节点修改成功
    }

    public void SureAddVideo()
    {
        //OpenFileDialog a = new OpenFileDialog(); //new一个方法
        //a.Filter = "(*.et;*.xls;*.xlsx)|*.et;*.xls;*.xlsx|all|*.*"; //删选、设定文件显示类型
        //a.ShowDialog(); //显示打开文件的窗口
        //string fileName = a.FileName; //获得选择的文件路径
        //Debug.Log(fileName);
    }

    public void addVideo(string url , int start, int end, int length)
    {
        FileInfo fileInfo = new FileInfo(url);
        var raw_data = File.ReadAllBytes(url);
        MultipartFormFileSection fileSection = new MultipartFormFileSection("file", raw_data);
        StartCoroutine(addVideo(fileSection, start, end, 10));
    }

    public IEnumerator addVideo(MultipartFormFileSection file, int start, int end, int length)
    {

        List<IMultipartFormSection> formData = new List<IMultipartFormSection>();
        formData.Add(file);
        string rqUrl = baseUrl + "/GraphEdit" + "/register?" + "start=" + start + "&end=" + end + "&length=" + length;
        UnityWebRequest www = UnityWebRequest.Post(rqUrl, formData);
        www.SetRequestHeader("token", token);
        yield return www.SendWebRequest();
        //
        if (www.responseCode >= 300)
        {
            Debug.Log(www.error);
        }
        else
        {
            string res = www.downloadHandler.text;
            res = JsonManager.ValueParser(res, "data");
            long id = long.Parse(JsonManager.ValueParser(res, "userId"));
        }
    }



    public List<Graph> GetGraphs()
    {
        return g;
    }
    public long GetId()
    {
        return id;
    }
    public List<Node> GetNodes()
    {
        return n;
    }
    public List<PathData> GetPathDatas()
    {
        return p;
    }
    public int getRegisterCode()
    {
        return registerCode;
    }
    public int getLoginCode()
    {
        return loginCode;
    }

}






public class RootObject
{
    public string code { get; set; }
    public string message { get; set; }
    public List<Graph> data { get; set; }
}


public class Data2
{
    public List<Node> nodes { get; set; }
    public List<PathData> pathData { get; set; }
}

public class RootObject2
{
    public string code { get; set; }
    public string message { get; set; }
    public Data2 data { get; set; }
}

