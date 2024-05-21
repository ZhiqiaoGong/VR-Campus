using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class shoubingkongzhi : MonoBehaviour {

	///
	/// 手柄
	///
	SteamVR_TrackedObject tracked;

	ForkOutput forkOutput = null;

	void Awake()
	{
		//获取手柄
		tracked = GetComponent<SteamVR_TrackedObject>();
		forkOutput = GameObject.Find("Fork").GetComponent<ForkOutput>();
	}

	// Update is called once per frame
	void FixedUpdate()
	{
		var device = SteamVR_Controller.Input((int)tracked.index);
		
		

		if (device.GetPressDown(SteamVR_Controller.ButtonMask.Touchpad))
		{
			Debug.Log("按下圆盘");			
		}
		else if (device.GetPressDown(SteamVR_Controller.ButtonMask.Trigger))
		{
			Debug.Log("按下扳机键");
			forkOutput.ChangePath();
		}
		else if (device.GetPressDown(SteamVR_Controller.ButtonMask.Grip))
		{
			Debug.Log("按下手柄侧键");
		}
		else if (device.GetPressDown(SteamVR_Controller.ButtonMask.ApplicationMenu))
		{
			Debug.Log("按下手柄菜单键");
		}
		else if (device.GetPressDown(SteamVR_Controller.ButtonMask.ApplicationMenu))
		{
			Debug.Log("按下手柄菜单键");
		}

	}
}
