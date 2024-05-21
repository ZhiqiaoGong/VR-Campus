using HTC.UnityPlugin.Vive;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class viewpoint_Adjust : MonoBehaviour {
	public GameObject camer;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (ViveInput.GetPress(HandRole.RightHand, ControllerButton.Pad)|| ViveInput.GetPress(HandRole.LeftHand, ControllerButton.Pad))
		{
			Debug.Log(ViveInput.GetPadTouchAxis(HandRole.RightHand).y);
			if (ViveInput.GetPadTouchAxis(HandRole.RightHand).y >= 0)
			{
				camer.transform.position = new Vector3(camer.transform.position.x, camer.transform.position.y + (float)0.01, camer.transform.position.z);
			}
			else
			{
				camer.transform.position = new Vector3(camer.transform.position.x, camer.transform.position.y - (float)0.01, camer.transform.position.z);
			}
		}
	}
}
