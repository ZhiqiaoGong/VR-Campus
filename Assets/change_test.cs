using LogicalGraph;
using RenderHeads.Media.AVProVideo;
using GraphData;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class change_test : MonoBehaviour
{
	//用于重定向获取当前的sphere对象；
	public static GameObject user_Sphere;
	
	//初始的ui控件
	[SerializeField]
	private GameObject primitive_sphere;

	//存放当前需要显示的sphere控件
	private List<GameObject> temp_paths;

	//由于所有的控件都必须是VRPlayer的子对象，为了分类，在VRPlayer下设一根对象，作为所有sphere组件的父对象
	private GameObject ForkSpheres;

	//调用位置逻辑图的接口
	private LgGraphOutput lg;
	private DaoOutput Dao;

	//当前用户的位置，将用于更新位置逻辑图
	private Position pos;
	//当前所接近的节点
	int current_id;
	float current_pos;
	int temp_id;

	//用户的当前路径
	//public string current_path;

	//是否已经绘制
	private bool if_showing = true;


	//public GameObject player1;
	//public GameObject player2;
	//Time time;
	bool flag=true;

	private string baseUrl;

	private void Awake()
	{
		temp_paths = new List<GameObject>();
		ForkSpheres = new GameObject();
		ForkSpheres.name = "ForkSpheres";
		ForkSpheres.transform.SetParent(GameObject.Find("VRPlayer").transform, false);
		lg = GameObject.Find("LgGraph").GetComponent<LgGraphOutput>();
		Dao = GameObject.Find("Dao").GetComponent<DaoOutput>();
	}

	private void Start()
	{
		baseUrl = Dao.GetBaseUrl();
		//flag = false;
		//user_Sphere=GameObject.Find("ForkSpheres").transform.Find("1_2.mp4").gameObject;
		//Debug.Log(user_Sphere.name);
	}

	// Update is called once per frame
	void Update () {
		
		//player1.SetActive(flag);
		//player2.SetActive(!flag);
		//flag = !flag;
		//Debug.Log(lg.GetPosition().locatedPath.vUrl);

		temp_id = current_id;
		pos = lg.GetPosition();
		current_pos = pos.location;
		//接近起点
		if (current_pos < 0.2f || current_pos > 0.8f)
		{
			//不论sphere是否被绘制都需要更新current_pos和current_id。
			//这项工作与LgGraph重复，未来再优化。
			if (current_pos < 0.2f)
			{
				if (current_pos < 0) current_pos = 0;
				current_id = pos.locatedPath.start;
			}
			else
			{
				if (current_pos > 1) current_pos = 1;
				current_id = pos.locatedPath.end;
			}

			//当交叉路口切换路径时，只保留用户选定路径的对象显示
			//处于节点
			if (current_pos <= 0 || current_pos >= 1)
			{
				//foreach (var item in temp_paths)
				//{
				//	//只保留新路径的sphere，其他路径的sphere全部销毁
				//	if (ForkOutput.user_New_Path.vUrl != item.name)
				//	{
				//		item.gameObject.SetActive(false);
				//		//temp_paths.Remove(item);
				//		//Destroy(item.gameObject);
				//	}
				//	else
				//	{
				//		user_Sphere = item.gameObject;
				//		Debug.Log(user_Sphere.name);
				//		//user_Sphere.SetActive(false);
				//	}

				//}
				for (int i = temp_paths.Count - 1; i >= 0; i--)
				{
					//只保留新路径的sphere，其他路径的sphere全部销毁
					if (ForkOutput.user_New_Path.vUrl != temp_paths[i].name)
					{
						temp_paths[i].gameObject.SetActive(false);
						DestroyImmediate(temp_paths[i].gameObject);
						temp_paths.Remove(temp_paths[i]);
					}
					else
					{
						user_Sphere = temp_paths[i].gameObject;
						Debug.Log(user_Sphere.name);
						//user_Sphere.SetActive(false);
					}

				}
			}

			//Debug.Log(current_id);
			//如果sphere已被绘制，返回
			//if (if_showing) return;
			List<Node> adj_nodes = lg.GetAdjacencyNodes(current_id);
			List<PathData> paths = new List<PathData>();
			foreach (var item in adj_nodes)
			{
				paths.Add(lg.GetPathbyIDs(current_id, item.id));
			}

			Show(paths);
			
		}

		

	}

	//动态加载几个sphere对象
	public void Show(List<PathData> paths)
	{
		//控制只加载一次
		if (temp_id != current_id)
			if_showing = false ;
	    else
			if_showing = true;
		if (if_showing) return;

		//对路径挨个遍历生成sphere球
		foreach (var item in paths)
		{
			var uiElement = Instantiate(primitive_sphere);
			uiElement.name = item.vUrl;
			uiElement.transform.SetParent(ForkSpheres.transform, false);
			
			//Debug.Log(baseUrl + item.vUrl);
			uiElement.GetComponent<MediaPlayer>().OpenVideoFromFile(MediaPlayer.FileLocation.RelativeToStreamingAssetsFolder, baseUrl + item.vUrl, true);
			temp_paths.Add(uiElement);
			if (flag)
			{
				user_Sphere = uiElement;
				flag = false;
				Debug.Log(user_Sphere.name);
				//user_Sphere.SetActive(false);
			}
		}
		
	}

	////销毁:需要保留一个
	//public void unShow()
	//{
	//	if (temp_paths != null)
	//	{
	//		foreach (var item in temp_paths)
	//		{
	//			Destroy(item.gameObject);
	//		}
	//	}
	//	temp_paths.Clear();
	//	if_showing = false;
	//}
}
