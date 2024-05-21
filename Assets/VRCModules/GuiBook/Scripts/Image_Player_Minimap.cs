using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GraphData;
using LogicalGraph;
using GuiBook;

public class Image_Player_Minimap : MonoBehaviour
{
	//接口
	GuiBookButton GBB;
	DaoOutput Dao;
	LgGraphOutput LG;

	//
	Position Player_Position;


	//储存已经算好的点的坐标
	public List<Node> Node_List = new List<Node>();
	List<Node> Node = new List<Node>();

	//储存起点终点的编号
	int Start_int;
	int End_int;
	Vector3 Start_Point;
	Vector3 End_Point;
	//储存进度

	float Player_Location;
	//储存方向
	Vector3 Direction;
	float Angle = 0;
	Vector3 N = new Vector3(0, 1, 0);
	//储存位置坐标
	public float Player_X;
	public float Player_Y;

	GameObject  GuidBook;



	void Awake()
	{
		//获取接口
		GuidBook = GameObject.Find("GuidBook").gameObject;
		
		GBB = GuidBook.transform.Find("Map/Buttons").GetComponent<GuiBookButton>();

		Dao = GameObject.Find("Dao").GetComponent<DaoOutput>();
		Node = Dao.GetNodes();


		LG = GameObject.Find("LgGraph").GetComponent<LgGraphOutput>();

		
	}

	// Use this for initialization
	void Start()
	{
		Node_List = GBB.Coordinate_Tran(Node);//坐标已换算完毕
		
		//Debug.Log("Image.gbb" + GBB.point_pos[1].name);
	}


	// Update is called once per frame
	void Update()
	{
		//更新位置信息
		Player_Position = LG.GetPosition();//获取位置

		Start_int = Player_Position.locatedPath.start;//起点终点编号
		End_int = Player_Position.locatedPath.end;

		Player_Location = Player_Position.location;//一段路走的进度



		Start_Point = new Vector3(Node_List[Start_int - 1].longitude, Node_List[Start_int - 1].latitude, 0);//起点终点坐标
		End_Point = new Vector3(Node_List[End_int - 1].longitude, Node_List[End_int - 1].latitude, 0);


		Player_X = Start_Point.x + Player_Location * (End_Point.x - Start_Point.x);//用户坐标
		Player_Y = Start_Point.y + Player_Location * (End_Point.y - Start_Point.y);

		Direction = new Vector3(Start_Point.x - End_Point.x, Start_Point.y - End_Point.y, 0);//路径方向
		Angle = Vector3.Angle(N, Direction);

		//更新朝向(基于路径方向）
		this.transform.rotation = Quaternion.Euler(0, 0, Angle - Player_Position.angle[1]);


		//更新位置


		this.transform.localPosition = new Vector3(Player_X * 0.32f, Player_Y * 0.32f, 0);


	}
}

