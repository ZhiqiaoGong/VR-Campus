using System.Collections;
using System.Collections.Generic;
using LogicalGraph;
using GraphData;
using UnityEngine;
using UnityEngine.UI;



/// <summary>
/// 根据当前所在节点，绘制可达节点
/// </summary>
public class ShowNodeInf : MonoBehaviour {


	//初始的ui控件
	[SerializeField]
	private Button primitive_node;

	//Text距离眼睛的深度
	private float depth = 1;
	//Text距离中心的距离

	private  float length= 1;
	//按钮追踪的相机
	[SerializeField]
	private GameObject camera;

	//存放当前需要显示的ui控件
	private List<Button> temp_nodes;

	//由于所有的控件都必须是Canvas的子对象，为了分类，在Canvas下设一根对象，作为Fork模块所有组件的父对象
	private GameObject ForkButtons;

	//调用位置逻辑图的接口
	private LgGraphOutput lg;

	//当前用户的位置，将用于更新位置逻辑图
	private Position pos;
	//当前所接近的节点
	int current_id;
	float current_pos;


	//是否已经绘制
	private bool if_showing = false;


	private void Awake()
    {
		temp_nodes = new List<Button>();
		ForkButtons = new GameObject();
		ForkButtons.name = "ForkButtons";
		ForkButtons.transform.SetParent(GameObject.Find("Canvas").transform, false);
		lg = GameObject.Find("LgGraph").GetComponent<LgGraphOutput>();
	
	}

	private void Update()
    {
		
		pos = lg.GetPosition();
		current_pos = pos.location;
		//接近起点
		if (current_pos<0.2f||current_pos > 0.8f)
        {
			//不论按钮是否被绘制都需要更新current_pos和current_id。
			//这项工作与LgGraph重复，未来再优化。
			if (current_pos<0.2f)
            {
				if (current_pos < 0) current_pos = 0;
				current_id = pos.locatedPath.start;
            }
            else
            {
				if (current_pos > 1) current_pos = 1;

				current_id = pos.locatedPath.end;
            }

			//如果按钮已被绘制，返回
			if (if_showing) return;
			List<Node> ajNodes = lg.GetAdjacencyNodes(current_id);
			Node current_node = lg.getNodeById(current_id);
			//获得当前节点与周围顶点连线与正东方向的夹角
			AngleCaculator angleCaculator = new AngleCaculator(current_node);
			List<float> vs = angleCaculator.getAngleList(ajNodes);
			show(ajNodes, vs);
        }
        else
        {
			unShow();
        }

	}

	/*
	 * @param nodes:用户当前可达节点
	 * @param vs:可达节点在场景中对应的角度
	 */
	public void show(List<Node> nodes,List<float> vs)
    {

		if (if_showing) return;
		using (var e1 = nodes.GetEnumerator())
		using (var e2 = vs.GetEnumerator())
        {
            while (e1.MoveNext()&&e2.MoveNext())
            {
				
				var node = e1.Current;
				var angle = e2.Current;
				var uiElement = Instantiate(primitive_node);
				
				uiElement.transform.SetParent(ForkButtons.transform,false);

				//botton名即其对应的node.id,botton上显示节点名字
				uiElement.name = node.id.ToString()+" "+angle.ToString();
				uiElement.transform.Find("Text").GetComponent<Text>().text = node.name;


				//变化按钮位置
				//uiElement.transform.position += new Vector3(length * Mathf.Cos(angle), 0,length * Mathf.Sin(angle));
				uiElement.transform.position = camera.transform.position;
				uiElement.transform.position += new Vector3(length*Mathf.Cos(angle),length * Mathf.Sin(angle), depth);

                //设置按钮触发事件
                uiElement.onClick.AddListener(()=> {bottonListener(node.id); });
				temp_nodes.Add(uiElement);

			}
        }
		if_showing = true;
    }

	public void unShow()
    {
        if (temp_nodes!=null)
        {
			foreach (var item in temp_nodes)
			{
				Destroy(item.gameObject);
			}
		}
		temp_nodes.Clear();
		if_showing = false;
    }



	private void bottonListener(int aim)
    {
		PathData new_path = lg.GetPathbyIDs(aim, current_id);
        if (new_path.vUrl==null||new_path.length<=0)
        {
			Debug.Log("路径不存在");
			return;
        }
		pos.locatedPath = new_path;
		pos.location = current_pos;
		lg.ChangePosition(pos);
    }




}
