using LogicalGraph;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class controlRotition : MonoBehaviour {
	public GameObject camer;
	public GameObject camer_father;
	private float rot_y;

	LgGraphOutput lg;
	Position pos;
	//可转向的范围
	private static float area = 0.2f;
	// Use this for initialization
	void Start () {
		lg = GameObject.Find("LgGraph").GetComponent<LgGraphOutput>();
	}
	
	// Update is called once per frame
	void Update () {
		pos = lg.GetPosition();
		if (pos.location < area || pos.location > 1 - area)
		{
			if (pos.location < area)
			{
				if (pos.location < 0) pos.location = 0;

			}
			else
			{
				if (pos.location > 1) pos.location = 1;
			}
			//处于节点
			if (pos.location <= 0 || pos.location >= 1)
			{
				buton_click();
			}
		}
	}
	public void buton_click()
	{
		rot_y = camer.transform.localEulerAngles.y;
		if (rot_y > 180)
			rot_y = rot_y - 360;
		//rot_y = camer.transform.rotation.eulerAngles.y;
		Debug.Log(rot_y);
		camer_father.transform.rotation = Quaternion.Euler(0, -rot_y + 180, 0);
	}
}
