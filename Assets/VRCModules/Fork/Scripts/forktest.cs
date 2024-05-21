using System.Collections;
using System.Collections.Generic;
using GraphData;
using UnityEngine;

public class forktest : MonoBehaviour {

	public int count = 6;

    void OnEnable()
    {
		List<Node> nodes = new List<Node>();
		List<float> vs = new List<float>();
		
		for (int i = 0; i < count; i++)
        {
			Node n = new Node();
			n.name = "test" + i.ToString();
			nodes.Add(n);
			vs.Add(i * 360 / count);
		}
		
		ShowNodeInf s = GetComponent<ShowNodeInf>();
		s.show(nodes, vs);


	}

	void OnDisable()
    {
		ShowNodeInf s = GetComponent<ShowNodeInf>();
		s.unShow();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
