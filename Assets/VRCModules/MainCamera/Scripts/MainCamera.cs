using System.Collections;
using System.Collections.Generic;
using LogicalGraph;
using UnityEngine;
using UnityEngine.XR;

public class MainCamera : MonoBehaviour {

	LgGraphOutput lg;
	Position pos;


	private void Awake()
	{
		InputTracking.disablePositionalTracking = true;//这个禁用头盔
		XRDevice.DisableAutoXRCameraTracking(gameObject.GetComponent<Camera>(), true);//这个禁用旋转，第一个参数把头盔自带的相机放进去就行了
	}

	void Start () {
		lg = GameObject.Find("LgGraph").GetComponent<LgGraphOutput>();
	}
	
	// Update is called once per frame
	void Update () {
		pos = lg.GetPosition();
		this.transform.eulerAngles = new Vector3(pos.angle[0],pos.angle[1],pos.angle[2]);
	}
}
