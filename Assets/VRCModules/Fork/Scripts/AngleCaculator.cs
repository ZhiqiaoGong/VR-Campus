using System.Collections;
using System.Collections.Generic;
using GuiBook;
using LogicalGraph;
using GraphData;
using UnityEngine;

public class AngleCaculator{

	CoordinateTranformer coordinate = null;
	Node root_node;


    public AngleCaculator(Node root_node)
    {
		this.root_node = root_node;
		coordinate = new CoordinateTranformer(root_node);
    }

	
	/// <summary>
	/// 获取一系列节点与本实例的根节点连线与正东方向的夹角。传入参数中的所有节点的经纬度属性为初值
	/// </summary>
	/// <param name="nodes">目标节点列表</param>
	/// <param name="root_node">根节点</param>
	/// <returns>度数制的夹角列表</returns>
	public List<float> getAngleList(List<Node> nodes)
	{
		List<float> res = new List<float>();
		Vector2 rn = coordinate.transform(root_node);
		foreach (var item in nodes)
		{
            try
            {
				res.Add(getAngle(rn, coordinate.transform(item)));
			}
            catch (System.NullReferenceException)
            {
				Debug.Log("未初始化坐标系。将以列表中首节点为坐标系");
				return getAngleListAutolly(nodes);
			}
		}
		return res;
	}

	public List<float> getAngleListAutolly(List<Node> nodes,Node root_node = new Node())
    {
		List<float> res = new List<float>();
		if (root_node.name == null)
        {
			root_node = nodes[0];
        }
		CoordinateTranformer coordinate = new CoordinateTranformer(root_node);
		Vector2 rn = coordinate.transform(root_node);
		foreach (var item in nodes)
		{
			res.Add(getAngle(rn, coordinate.transform(item)));
		}
		return res;
	}

	public float getAngle(Vector2 root,Vector2 aim)
    {
		return getAngle(root.x, root.y, aim.x, aim.y);
    }

	/// <summary>
	/// 根据两个节点返回两个节点的夹角
	/// </summary>
	/// <param name="root"></param>
	/// <param name="aim"></param>
	/// <returns></returns>
	public float getAngle(Node root,Node aim)
    {
		return getAngle(coordinate.transform(root), coordinate.transform(aim));
    }



	/// <summary>
	/// 根据root和aim的平面坐标，返回由root到aim的射线与x轴的夹角
	/// </summary>
	/// <returns>度数制夹角值</returns>
	private float getAngle(float root_x, float root_y, float aim_x, float aim_y)
	{
		float dRotateAngle = Mathf.Atan2(Mathf.Abs(root_y - aim_y), Mathf.Abs(root_x - aim_x));
		if (aim_x >= root_x)
		{
			if (aim_y >= root_y)
			{
				//第一象限 不变	
			}
			else
			{
				//第四象限 
				dRotateAngle = 2*Mathf.PI - dRotateAngle;
			}
		}
		else
		{
			if (aim_y >= root_y)
			{
				//第二象限
				dRotateAngle = Mathf.PI - dRotateAngle;
			}
			else
			{
				//第三象限
				dRotateAngle = Mathf.PI + dRotateAngle;
			}
		}
		dRotateAngle = dRotateAngle * 180 / Mathf.PI;
		dRotateAngle = angleNormalization(dRotateAngle);
		return dRotateAngle;
	}

	public static float angleNormalization(float angle)
    {
		while (angle > 360) angle -= 360;
		while (angle < 0) angle += 360;
		return angle;
    }


	/// <summary>
	/// 两角度之差。会自动标准化为平面角
	/// </summary>
	/// <param name="minuend"></param>
	/// <param name="subtraction"></param>
	/// <returns></returns>
	public static float angle_gap(float minuend,float subtraction)
    {
		float angle = minuend - subtraction;
		angle = angleNormalization(angle);
		return angle>180?(360-angle):angle;

    }
	


}
