using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class VVHunterOutput : MonoBehaviour {
	//头盔相机对象
	public GameObject obj;
	//头盔相机位置
	public float pos_X;
	public float pos_Y;
	public float pos_Z;
	//头盔相机旋转角度
	public float rot_X;
	public float rot_Y;
	public float rot_Z;
	public float test_speed = 0.005f;

	public bool test = false;

	// Use this for initialization
	void Awake()
	{
		pos_X = 0;
		pos_Y = 0;
		pos_Z = 0;
		rot_X = 0;
		rot_Y = 0;
		rot_Z = 0;
	}

	// Update is called once per frame
	void Update()
	{
		/*
		if (!test)
		{
			pos_X = obj.transform.position.x;
			pos_Y = obj.transform.position.y;
			pos_Z = obj.transform.position.z;
		}
		//Debug.Log("pos_X:" + pos_X);
		//Debug.Log("pos_Y:" + pos_Y);
		//Debug.Log("pos_Z:" + pos_Z);

		rot_X = obj.transform.eulerAngles.x;
		if (rot_X > 180)
			rot_X = rot_X - 360;
		rot_Y = obj.transform.eulerAngles.y;
		if (rot_Y > 180)
			rot_Y = rot_Y - 360;
		rot_Z = obj.transform.eulerAngles.z;
		if (rot_Z > 180)
			rot_Z = rot_Z - 360;
		//Debug.Log("rot_X:" + rot_X);
		//Debug.Log("rot_Y:" + rot_Y);
		//Debug.Log("rot_Z:" + rot_Z);*/

		if (rot_X > 180)
			rot_X = rot_X - 360;
		if (rot_Y > 180)
			rot_Y = rot_Y - 360;
		if (rot_Z > 180)
			rot_Z = rot_Z - 360;

		if (test)
		{
			pos_X += test_speed;
			//pos_Y += 0.005f;
			pos_Z += test_speed;
		}


	}


}
