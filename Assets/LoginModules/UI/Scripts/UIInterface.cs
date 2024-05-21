using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using WebService;

public class UIInterface : MonoBehaviour {

    //只有其中一个处于激活状态
    GameObject activeUI;
    GameObject UserButtonRoot;
    GameObject GraphButtonRoot;



    GameObject Canvas;
    WebServiceOutput webService;

    [SerializeField]
    GameObject pri_button;
    
    void Awake()
    {
        GraphButtonRoot = new GameObject("GraphButtonRoot");
        UserButtonRoot = new GameObject("UserButtonRoot");
        Canvas = GameObject.Find("Canvas");

        UserButtonRoot.transform.SetParent(Canvas.transform,false);
        GraphButtonRoot.transform.SetParent(Canvas.transform,false);

        buttonsLayout(UserButtonRoot);
        buttonsLayout(GraphButtonRoot);
        
        if (pri_button==null)
        {
            Debug.Log(gameObject.name+":组件：UIInterface：未初始化原始控件");
            gameObject.SetActive(false);
        }
        
        webService = GameObject.Find("WebService").GetComponent<WebServiceOutput>();
        
    }


    void Update()
    {
        //性能开销不高，但起始也可以选择只在接口中调用此方法
        choseActive();
    }


	public void ShowUsersList(List<User> users)
    {
        activeUI = UserButtonRoot;
        //销毁原来的子对象。不考虑性能地、每次执行执行一次销毁、重建，可以确保数据可以即时更新
        killChilds(UserButtonRoot);

        foreach (var user in users)
        {
            GameObject user_button = Instantiate(pri_button);
            
            var button = user_button.GetComponent<Button>();
            button.GetComponentInChildren<Text>().text = user.userName;

            user_button.name = "<user>"+user.userName;

            user_button.transform.SetParent(UserButtonRoot.transform,false);
            user_button.GetComponent<Button>().onClick.AddListener(() => { webService.ChoseUser(user.userId); });
        }    
    }

    public void ShowGraphsList(List<Graph> graphs)
    {
        activeUI = GraphButtonRoot;
        killChilds(GraphButtonRoot);

        foreach (var graph in graphs)
        {
            GameObject graphButton = Instantiate(pri_button);

            var button = graphButton.GetComponent<Button>();
            button.GetComponentInChildren<Text>().text = graph.graphName;

            graphButton.name = "<graph>" + graph.graphName;
            graphButton.transform.SetParent(GraphButtonRoot.transform, false);
            graphButton.GetComponent<Button>().onClick.AddListener(() => { webService.ChoseGraph(graph.graphId); });
        }

    }

    /// <summary>
    /// 为一组ui控件设置布局策略
    /// </summary>
    /// <param name="ui_root"></param>
    void buttonsLayout(GameObject ui_root)
    {
        GridLayoutGroup layout;
        if (ui_root.GetComponent<GridLayoutGroup>()==null)
        {
            //使用自动布局
            layout = ui_root.AddComponent<GridLayoutGroup>(); 
        }
    }

    /// <summary>
    /// 检查当前应该激活的ui
    /// </summary>
    void choseActive()
    {
        if (UserButtonRoot == activeUI)
            UserButtonRoot.SetActive(true);
        else UserButtonRoot.SetActive(false);

        if (GraphButtonRoot == activeUI)
            GraphButtonRoot.SetActive(true);
        else GraphButtonRoot.SetActive(false);
    }

    /// <summary>
    /// 清理子对象
    /// </summary>
    void killChilds(GameObject gameObject)
    {
        for (int i = 0; i < gameObject.transform.childCount; i++) gameObject.transform.GetChild(i).gameObject.SetActive(false);
    }




}



