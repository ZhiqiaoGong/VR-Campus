using System.Collections;
using System.Collections.Generic;
using LogicalGraph;
using GraphData;
using UnityEngine;

public class ForkOutput : MonoBehaviour {
	//调用位置逻辑图的接口
	private LgGraphOutput lg;

	//当前用户的位置，将用于更新位置逻辑图
	private Position pos;

	//当接近节点时，将利用接近的节点初始化该坐标系，同时初始化各节点对应的偏转角
	//未接近节点时，需要置为null
	private AngleCaculator angleCaculator = null;
	private List<Node> neibor_nodes = null;
	private List<float> angles = null;
	Node current_node;

	public static PathData user_New_Path;

	//可转向的范围
	private static float area = 0.2f;

	private void Awake()
    {
		lg = GameObject.Find("LgGraph").GetComponent<LgGraphOutput>();
	}
	private void Start()
    {
		pos = lg.GetPosition();
    }


	private void Update()
    {
		pos = lg.GetPosition();
		//更新邻近节点
		if (pos.location < area || pos.location > 1-area)
		{
			if (pos.location < area)
			{
				if (pos.location < 0) pos.location = 0;
				current_node = lg.getNodeById(pos.locatedPath.start);
			}
			else
			{
				if (pos.location > 1) pos.location = 1;
				current_node = lg.getNodeById(pos.locatedPath.end);
			}
		}
        else
        {
			current_node = new Node();
			neibor_nodes = null;
			angles = null;
        }
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
			ChangePath();
        }

        if (pos.location<=0||pos.location>=1)
        {
			ChangePath();
        }
	}


	

	public void ChangePath()
    {
		//处于邻近节点
        if (current_node.name==null&&current_node.id==0)
        {
			Debug.Log("失败,用户未站在节点处");
        }
        else
        {
			angleCaculator = new AngleCaculator(current_node);
			neibor_nodes = lg.GetAdjacencyNodes(current_node.id);
			angles = angleCaculator.getAngleList(neibor_nodes);
            if (neibor_nodes.Count==0)
            {
				Debug.Log("无可达路径");
				return;
			}
			//当前正方向与正东方向的夹角
			float deviation = angleCaculator.getAngle(lg.getNodeById(pos.locatedPath.end),lg.getNodeById(pos.locatedPath.start));
			//当前用户方向与正东方向的夹角
			float current_angle =AngleCaculator.angleNormalization(deviation - pos.angle[1]);
			Debug.Log("用户当前视角与正东方向夹角为："+current_angle);

			float nearest_angle = AngleCaculator.angle_gap(angles[0],current_angle);
			Node best_node = neibor_nodes[0];
			for (int i = 0; i < neibor_nodes.Count; i++)
            {
				float offset = AngleCaculator.angle_gap(angles[i],current_angle);
				Debug.Log("当前视角与" + neibor_nodes[i].name + "方向的角度为：" + offset);
				if ( offset < nearest_angle){
					nearest_angle = offset;
					best_node = neibor_nodes[i];
				}
			}

			PathData new_path = lg.GetPathbyIDs(best_node.id, current_node.id);
			if (new_path.vUrl == null || new_path.length <= 0)
			{
				Debug.Log("路径不存在");
				return;
			}
			user_New_Path = new_path;
			pos.locatedPath = new_path;
			pos.location = new_path.start==current_node.id?0:1;
			//偏转角重置
			//pos.angle[1] = current_node.id==pos.locatedPath.start?180:0; //好像重置不了
			lg.ChangePosition(pos);
		}
    }

	




}
