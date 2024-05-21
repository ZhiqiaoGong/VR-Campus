using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour {
	[SerializeField]
	private GameObject head_camera=null;
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
        if (head_camera!=null)
        {
			Vector3 pos = head_camera.transform.position;
			gameObject.transform.position.Set(pos.x,2, pos.z);
		}
	}
}
