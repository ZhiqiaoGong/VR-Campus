using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LogicalGraph;
using GraphData;
using GuiBook;

public class LgGraphOutput : MonoBehaviour
{

	private Position position;
	public LgGraph lggraph;
	public string path;
	public float v_pos;

	void Awake()
	{
		position = lggraph.pos;
		path = position.locatedPath.vUrl;
		v_pos = position.location;
	}
	private void Update()
	{
		position = lggraph.pos;
		path = position.locatedPath.vUrl;
		v_pos = position.location;
	}
	public Position GetPosition()
	{
		if (lggraph.pos.location > 1 + lggraph.exrange) lggraph.pos.location = 1 + lggraph.exrange;
		if (lggraph.pos.location < 0 - lggraph.exrange) lggraph.pos.location = 0 - lggraph.exrange;
		position = lggraph.pos;
		return position;
	}

	public List<Node> GetAdjacencyNodes(int id)
	{
		return lggraph.GetAdjNodes(id);
	}

	public void ChangePosition(Position p)
	{
		if (p.location > 1 + lggraph.exrange) p.location = 1 + lggraph.exrange;
		if (p.location < 0 - lggraph.exrange) p.location = 0 - lggraph.exrange;
		lggraph.pos = p;
	}

	public void ChangePosition(int nodeToGo)
	{
		lggraph.UpdatePos2D(nodeToGo);
	}

	public void ChangePosition(PathData pathToGo, float rate)
	{
		lggraph.pos.locatedPath = pathToGo;
		lggraph.clickPath = true;
		lggraph.changeRate = rate;
		//lggraph.pos.location = rate;
		position = lggraph.pos;
		Debug.Log("go th the path" + position.locatedPath.vUrl + " rate:" + lggraph.pos.location);
		//angle?
	}

	public void ChangePosition(float rate)
    {
		lggraph.clickPath = true;
		lggraph.changeRate = rate;
		position = lggraph.pos;
		Debug.Log("at path" + position.locatedPath.vUrl + " rate:" + lggraph.pos.location);
	}

	public void LGTest()
	{
		Debug.Log("调用test");
	}

	public Node getNodeById(int id)
	{
		return lggraph.GetNodebyID(id);
	}

	public PathData GetPathbyIDs(int id1, int id2)
	{
		return lggraph.GetPathbyIDs(id1, id2);

	}


}
