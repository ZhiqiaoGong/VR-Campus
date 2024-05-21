using System.Collections;
using System.Collections.Generic;
using LogicalGraph;
using UnityEngine;

public class DebugOutput : MonoBehaviour {
	LgGraphOutput lg;
	public GameObject vv_cam_eye;
	public GameObject vv_hunter;

	public Position pos;



	public float location;
	public string vUrl;
	public float vv_rot_y;
	public float vv_cam_rot_y;
	public float lg_rot_y;
	


	// Use this for initialization
	void Start () {
		lg = GameObject.Find("LgGraph").GetComponent<LgGraphOutput>();
	}
	
	// Update is called once per frame
	void Update () {
		vv_rot_y = vv_hunter.transform.eulerAngles.y;
		vv_cam_rot_y = vv_cam_eye.transform.eulerAngles.y;
		pos = lg.GetPosition();
		location = pos.location;
		vUrl = pos.locatedPath.vUrl;
		lg_rot_y = pos.angle[1];
	
	}
}
