using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class VVHunter : MonoBehaviour {

	private SteamVR_PlayArea playArea;

	HmdQuad_t bound;

	private void Start()
    {
		playArea = GetComponent<SteamVR_PlayArea>();
    }

	// Update is called once per frame
	void Update () {
		bool flag = SteamVR_PlayArea.GetBounds(playArea.size, ref bound);
        if (!flag)
        {
			Debug.Log("获取边界失败");
        }
	}
}
