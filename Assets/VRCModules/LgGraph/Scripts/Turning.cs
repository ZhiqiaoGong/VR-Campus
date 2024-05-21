using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LogicalGraph;
using GraphData;

public class Turning : MonoBehaviour
{

	public float walkSpeed = 0.01f;
	public float turnSpeed = 1f;

	LgGraphOutput lg;
	VVHunterOutput vv;
	private Position pos;
	private PathData path;
	private float loc;

	// Use this for initialization
	void Awake()
	{
		lg = GameObject.Find("LgGraph").GetComponent<LgGraphOutput>();
		vv= GameObject.Find("VVHunter").GetComponent<VVHunterOutput>();
		pos = lg.GetPosition();
		path = pos.locatedPath;
		loc = pos.location;
	}

	// Update is called once per frame
	void Update()
	{
		pos = lg.GetPosition();
		if ((path.start != pos.locatedPath.start) || (path.end != pos.locatedPath.end))
		{
			//已切换路径，判断前进方向
			if ((path.start == pos.locatedPath.end) || (path.end == pos.locatedPath.end))
				walkSpeed = -Mathf.Abs(walkSpeed);
			else
				walkSpeed = Mathf.Abs(walkSpeed);
			path = pos.locatedPath;
		}

		//行走
		if (Input.GetKey(KeyCode.W))
		{
			vv.pos_X += walkSpeed;
			vv.pos_Z += walkSpeed;
			Debug.Log("w");
		}
		else if (Input.GetKey(KeyCode.S))
		{
			vv.pos_X -= walkSpeed;
			vv.pos_Z -= walkSpeed;
			Debug.Log("s");
		}

		//x转向
		if (Input.GetKey(KeyCode.A))
		{
			vv.rot_Y -= turnSpeed;
			Debug.Log("a"+ vv.rot_Y);
		}
		else if (Input.GetKey(KeyCode.D))
		{
			vv.rot_Y += turnSpeed;
			Debug.Log("d " + vv.rot_Y);
		}

		//y转向
		if (Input.GetAxis("Mouse ScrollWheel") < 0)
		{
			vv.rot_X -= turnSpeed;
			Debug.Log("Scroll down " + vv.rot_X);
		}
		else if (Input.GetAxis("Mouse ScrollWheel") > 0)
		{
			vv.rot_X += turnSpeed;
			Debug.Log("Scroll up " + vv.rot_X);
		}
	}
}