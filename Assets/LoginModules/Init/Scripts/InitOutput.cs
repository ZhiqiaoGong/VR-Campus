using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using WebService;

public class InitOutput : MonoBehaviour {

    public List<PathData> pathDatas;
    public List<Node> nodes;
    public long userId;
    public long graphId;



	public void Init(List<Node> nodes,List<PathData> pathDatas,long userId,long graphId)
    {
        this.userId = userId;
        this.graphId = graphId;

        DontDestroyOnLoad(gameObject);
        //TODO 加载下一场景
        this.pathDatas = pathDatas;
        this.nodes = nodes;
        SceneManager.LoadSceneAsync("VRCampus");
    }
}
