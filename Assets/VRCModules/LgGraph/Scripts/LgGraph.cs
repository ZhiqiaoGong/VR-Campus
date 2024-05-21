using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GraphData;
using GuiBook;

namespace LogicalGraph
{
	public class LgGraph : MonoBehaviour
	{

		DaoOutput d;
		List<PathData> pathdatas;
		List<Node> nodes;
		public Position pos;
		public float poslocation;
		public float anglex;
		public float angley;

		//private int prenext = 0;//保存二维地图上一次点击的节点id 用于判断点击
		private int nextnode = 0;//保存二维地图点击的节点id
		private int max = 0;
		private float[,] PathAdjacencyMatrix;//声明一个int类型的二维数组PathAdjacencyMatrix
		private Hashtable nodehash;
		private Hashtable pathhash;

		//PathData nowPath;//用户现在位于的路径（临时）
		PathData prePath;//用户上一次位于的路径（用于判断行进方向）
		private float rate = 7.5f;//用于转换速度与视频播放速度
		private float tx = 0, x = 0;//保存现在位置和前一帧位置
		private float preLocation = 0;//保存前一帧移动的距离
		[HideInInspector]
		public bool clickPath = false;//用户是否点击以更改路径
		[HideInInspector]
		public float changeRate = 0;//保存用户点击的路径的位置（0-1）
		private float[] move;

		private float NO_PATH = 9999999999;

		[HideInInspector]
		public float exrange = 0.1f;//用于切换路径 并判断路径的范围是否合法
		private float inrange = 0.2f;//用于切换路径
		private float delt = 0.01f;//切换路径的一点偏移量

		//private float move;//从vvhunter中接收到的用户位置的变化量
		//private Vector3 vec3;//从vvhunter中接收到的用户角度的变化量

		VVHunterOutput VV;
		GuiBookButton button;

		// Use this for initialization
		void Awake()
		{
			//初始化
			//获得VVHunter的Output
			VV = GameObject.Find("VVHunter").GetComponent<VVHunterOutput>();

			//获得Dao的Output
			d = GameObject.Find("Dao").GetComponent<DaoOutput>();
			d.testDao();



			//获得GuidBook的Output
			//button = GameObject.Find("Nodes").GetComponent<GuiBookButton>();


			pathdatas = d.GetPathDatas(); //所有路径信息
			nodes = d.GetNodes(); //所有节点信息
			foreach (var item in nodes)
			{
				if (max <= item.id)
				{
					max = item.id;
				}
			}



			pos = new Position();//初始化位置信息



			//建立id-node表
			nodehash = new Hashtable();
			for (int i = 0; i < nodes.Count; i++)
			{
				nodehash.Add(nodes[i].id, nodes[i]);
			}

			//建立id,id-pathdata表
			pathhash = new Hashtable();
			string s = "";
			for (int j = 0; j < pathdatas.Count; j++)
			{

				s = pathdatas[j].start + "," + pathdatas[j].end;
				if (!pathhash.ContainsKey(s))
					pathhash.Add(s, pathdatas[j]);
				s = pathdatas[j].end + "," + pathdatas[j].start;
				if (!pathhash.ContainsKey(s))
					pathhash.Add(s, pathdatas[j]);
			}


			//建立邻接矩阵
			InitPathGraph();
			CreatePathGraph();

			//初始化用户位置
			pos.locatedPath = pathdatas[0];
			pos.location = 0.0f;

			prePath = pos.locatedPath;
		}

		// Update is called once per frame
		void Update()
		{
			//从vvhunter接收一个距离和角度的变量
			//float move;
			//Vector3 vec3 = new Vector3();
			//move = VV.getPositinonVariableQuantity();
			//vec3=VV.getAngleVariableQuantity();
			//r[0] = vec3.x;
			//r[1] = vec3.y;
			//r[2] = vec3.z;

			move = new float[3];
			float[] r = new float[3];
			move[0] = VV.pos_X;
			move[1] = VV.pos_Y;
			move[2] = VV.pos_Z;
			r[0] = VV.rot_X;
			r[1] = VV.rot_Y;
			r[2] = VV.rot_Z;

			/*
			nextnode = button.button_output;
			if (nextnode > 0 && nextnode != prenext)
            {
				Debug.Log("click:"+nextnode);
				UpdatePos2D();
				prenext = nextnode;
            }*/
			UpdatePosAngle(move,r);

			poslocation = pos.location;

			anglex = pos.angle[0];
			angley = pos.angle[1];
		}


		void InitPathGraph()
		{
			PathAdjacencyMatrix = new float[max, max];
			for (int i = 0; i < max; i++)
			{
				for (int j = 0; j < max; j++)
				{
					PathAdjacencyMatrix[i, j] = NO_PATH;
				}
			}
		}



		void CreatePathGraph()
		{
			for (int i = 0; i < max; i++)
			{
				for (int j = 0; j < max; j++)
				{
					PathData p = GetPathbyIDs(i + 1, j + 1);
					if (p.length != NO_PATH)
					{
						PathAdjacencyMatrix[p.start - 1, p.end - 1] = p.length;
						PathAdjacencyMatrix[p.end - 1, p.start - 1] = p.length;
					}

				}
			}
		}

		//获得一个节点的邻接节点
		public List<Node> GetAdjNodes(int id)
		{
			List<Node> r = new List<Node>();
			for (int i = 0; i < max; i++)
			{
				if (PathAdjacencyMatrix[id - 1, i] != NO_PATH) r.Add(GetNodebyID(i + 1));
			}
			return r;
		}

		public Node GetNodebyID(int id)
		{
			foreach (var item in nodes)
			{
				if (id == item.id)
				{
					return item;
				}
			}
			return new Node();
		}

		//用两个节点搜索路径
		public PathData GetPathbyIDs(int id1, int id2)
		{
			string s1, s2;
			s1 = id1 + "," + id2;
			s2 = id2 + "," + id1;
			if (pathhash.Contains(s1)) return (PathData)pathhash[s1];
			else if (pathhash.Contains(s2)) return (PathData)pathhash[s2];
			else
			{
				PathData p = new PathData
				{
					length = NO_PATH //用路径长度为NO_PATH代表没有查找到两个id之间的路径
				};
				return p;
			}
		}

		//用一个节点搜索路径
		public PathData GetEndPathbyID(int id)
		{
			if (id < 1) Debug.Log("GetEndPathbyID id:" + id);
			for (int i = 0; i < pathdatas.Count; i++)
			{
				if (pathdatas[i].end == id)
					return pathdatas[i];
			}
			PathData p = new PathData
			{
				length = NO_PATH
			};
			return p;
		}

		public PathData GetStartPathbyID(int id)
		{
			if (id < 1) Debug.Log("GetStartPathbyID id:" + id);
			for (int i = 0; i < pathdatas.Count; i++)
			{
				if (pathdatas[i].start == id)
					return pathdatas[i];
			}
			PathData p = new PathData
			{
				length = NO_PATH
			};
			return p;
		}

		public void UpdatePos2D(int node)
		{
			nextnode = node;
			PathData tp;
			tp = GetEndPathbyID(nextnode);
			if (tp.length == NO_PATH)
			{
				tp = GetStartPathbyID(nextnode);
				if (tp.length == NO_PATH)
				{
					Debug.Log("missing video, moving failed");
					//nextnode = prenext;
					return;
				}
				pos.locatedPath = tp;
				pos.location = 0.0f;

				Debug.Log("go to the start of path:" + pos.locatedPath.start + "->" + pos.locatedPath.end);
			}
			else
			{
				pos.locatedPath = tp;
				pos.location = 1.0f;

				Debug.Log("go to the end of path:" + pos.locatedPath.start + "->" + pos.locatedPath.end);
			}
		}

		//public void UpdatePosAngle(float move, float[] angle)
		public void UpdatePosAngle(float[] move, float[] angle)
		{
			//2021.8.4修改 x是前一帧的位置
			//移动的距离则为接受到的float
			//x=move;
			x = (float)Math.Sqrt(move[0] * move[0] + move[2] * move[2]);//移动的距离

			//判断是否切换了路径
			if ((prePath.start != pos.locatedPath.start) || (prePath.end != pos.locatedPath.end))
			{
				//判断是否到达节点需 要转向，pos>0.8或者pos<0.2
				//位置设置在0.21-0.79之间,相当于放在了那条路径的开头上,不在节点,不用算转向


				if (clickPath)
				{
					preLocation = changeRate;
					clickPath = false;
					changeRate = 0;
				}
				else
				{
					//判断路径朝向并确定用户所在位置
					if ((prePath.end == pos.locatedPath.start) || (prePath.start == pos.locatedPath.start)) preLocation = inrange + delt;
					else preLocation = 1 - inrange - delt;
				}
				prePath = pos.locatedPath;
			}
			else
			{
				//没切换路径但用户点击了路径其他位置
				if (clickPath)
				{
					preLocation = changeRate;
					clickPath = false;
					changeRate = 0;
				}
			}
			pos.location = preLocation + rate * (x - tx) / pos.locatedPath.length;
			//poslocation = pos.location;
			if (pos.location > 1 + exrange) pos.location = 1 + exrange;
			if (pos.location < 0 - exrange) pos.location = 0 - exrange;
			/*if (pos.location > 1 || pos.location < 0) //到达分岔路口，判断移动方向
			{
				//if视线在某个角度范围内并保持一定秒数（待实现），修改路径
				float eyeangle = angle[1] + 180; //角度只与y轴有关

				//判断转向点是该路径的起点还是终点
				if (pos.location > 1)//终点
				{
					ChangePath(pos.locatedPath.end, true, eyeangle);

					//修改位置信息
					if (nowPath.start == pos.locatedPath.end)
						pos.location = pos.location - 1;
					else pos.location = 1 - pos.location;
				}
				else//起点
				{
					ChangePath(pos.locatedPath.start, false, eyeangle);

					//修改位置信息
					if (nowPath.start == pos.locatedPath.start)
						pos.location = -pos.location;
					else pos.location = pos.location + 1;
				}
			}*/
			pos.angle = angle;
			//pos.locatedPath = nowPath;
			preLocation = pos.location;
			tx = x;
		}


		/*
		void ChangePath(int n, bool isend, float ang)
		{
			//获得邻接节点
			List<Node> adjNodes = GetAdjacencyNodes(n);
			List<Nodeangle> adjNodesAngle = new List<Nodeangle>();
			for (int a = 0; a < adjNodes.Count; a++)
			{
				adjNodesAngle.Add(new Nodeangle(adjNodes[a]));
			}

			//节点用角度由小到大排序并保存
			Nodeangle change;
			for (int b = 0; b < adjNodesAngle.Count - 1; b++)//冒泡排序
			{
				for (int j = 0; j < adjNodesAngle.Count - 1 - b; j++)
				{
					if (NodesCompare(adjNodesAngle[j], adjNodesAngle[j + 1], isend) > 0)
					{
						change = adjNodesAngle[j];
						adjNodesAngle[j] = adjNodesAngle[j + 1];
						adjNodesAngle[j + 1] = change;
					}
				}
			}

			//判断视线朝向 选择路径
			if (ang > adjNodesAngle[0].angle / 2 && ang < (adjNodesAngle[1].angle - adjNodesAngle[0].angle) / 2 + adjNodesAngle[0].angle)
			{
				if (isend)
				{
					nowPath = GetPathbyIDs(pos.locatedPath.end, adjNodesAngle[0].node.id);
				}
				else
				{
					nowPath = GetPathbyIDs(pos.locatedPath.start, adjNodesAngle[0].node.id);
				}
				return;
			}
			int i = 1;
			for (; i < adjNodesAngle.Count - 1; i++)
			{
				if ((ang > (adjNodesAngle[i].angle - adjNodesAngle[i - 1].angle) / 2 + adjNodesAngle[i - 1].angle) &&
					(ang < (adjNodesAngle[i + 1].angle - adjNodesAngle[i].angle) / 2 + adjNodesAngle[i].angle))
				{
					if (isend)
					{
						nowPath = GetPathbyIDs(pos.locatedPath.end, adjNodesAngle[0].node.id);
					}
					else
					{
						nowPath = GetPathbyIDs(pos.locatedPath.start, adjNodesAngle[0].node.id);
					}
					break;
				}
			}
			//倒数第二个角度
			if ((ang > (adjNodesAngle[i].angle - adjNodesAngle[i - 1].angle) / 2 + adjNodesAngle[i - 1].angle) &&
					(ang < (360 - adjNodesAngle[i].angle) / 2 + adjNodesAngle[i].angle))
			{
				if (isend)
				{
					nowPath = GetPathbyIDs(pos.locatedPath.end, adjNodesAngle[0].node.id);
				}
				else
				{
					nowPath = GetPathbyIDs(pos.locatedPath.start, adjNodesAngle[0].node.id);
				}
				return;
			}
			//最后一个角度：原方向 - 转回去？
			if (ang < adjNodesAngle[0].angle / 2 || ang > (adjNodesAngle[adjNodesAngle.Count - 1].angle - adjNodesAngle[adjNodesAngle.Count - 2].angle) / 2 + adjNodesAngle[adjNodesAngle.Count - 2].angle)
				pos.angle[1] = -pos.angle[1];//?
		}

		//保存节点信息和角度信息 便于比较
		public class Nodeangle
		{
			public Node node;
			public float angle;
			public Nodeangle(Node n)
			{
				this.node = n;
				this.angle = 0;
			}
			public void NAchange(float a)
			{
				this.angle = a;
			}
		}

		//比较节点角度大小
		int NodesCompare(Nodeangle n1, Nodeangle n2, bool isend)
		{
			float a1, a2;
			if (isend)
			{
				a1 = GetAngle(LtoXY(GetNodebyID(pos.locatedPath.start).longitude, GetNodebyID(pos.locatedPath.start).latitude),
									LtoXY(n1.node.longitude, n1.node.latitude),
									LtoXY(GetNodebyID(pos.locatedPath.end).longitude, GetNodebyID(pos.locatedPath.end).latitude));
				a2 = GetAngle(LtoXY(GetNodebyID(pos.locatedPath.start).longitude, GetNodebyID(pos.locatedPath.start).latitude),
								LtoXY(n1.node.longitude, n2.node.latitude),
								LtoXY(GetNodebyID(pos.locatedPath.end).longitude, GetNodebyID(pos.locatedPath.end).latitude));
			}
			else
			{
				a1 = GetAngle(LtoXY(GetNodebyID(nowPath.end).longitude, GetNodebyID(nowPath.end).latitude),
					LtoXY(n1.node.longitude, n1.node.latitude),
					LtoXY(GetNodebyID(nowPath.start).longitude, GetNodebyID(nowPath.start).latitude));
				a2 = GetAngle(LtoXY(GetNodebyID(nowPath.end).longitude, GetNodebyID(nowPath.end).latitude),
					LtoXY(n2.node.longitude, n2.node.latitude),
					LtoXY(GetNodebyID(nowPath.start).longitude, GetNodebyID(nowPath.start).latitude));
			}
			//修改节点角度信息
			n1.NAchange(a1);
			n2.NAchange(a2);

			if (a1 > a2) return 1;
			else if (a1 < a2) return -1;
			else return 0;
		}

		//将经纬度信息转换成xy坐标(copy自csdn)
		float[] LtoXY(float longitude, float latidude)
		{
			double L = 6381372 * Math.PI * 2;//地球周长
			double W = L;// 平面展开后，x轴等于周长
			double H = L / 2;// y轴约等于周长一半
			double mill = 2.3;// 米勒投影中的一个常数，范围大约在正负2.3之间
			double x = longitude * Math.PI / 180;// 将经度从度数转换为弧度
			double y = latidude * Math.PI / 180;// 将纬度从度数转换为弧度
			y = 1.25 * Math.Log(Math.Tan(0.25 * Math.PI + 0.4 * y));// 米勒投影的转换
																	// 弧度转为实际距离
			x = (W / 2) + (W / (2 * Math.PI)) * x;
			y = (H / 2) - (H / (2 * mill)) * y;
			float[] r = new float[2];
			r[0] = (float)x;
			r[1] = (float)y;
			return r;
		}

		//用三个点算三角形的夹角
		float GetAngle(float[] p1, float[] p2, float[] p0)
		{
			float dx1 = p1[0] - p0[0];
			float dy1 = p1[1] - p0[1];
			float dx2 = p2[0] - p0[0];
			float dy2 = p2[1] - p0[1];


			float ratio;  //矩形长和宽平方的比
			ratio = (dx1 * dx1 + dy1 * dy1) / (dx2 * dx2 + dy2 * dy2);
			if (p1[1] < p0[1]) return (float)(Math.Acos(((dx1 * dx2 + dy1 * dy2) / Math.Sqrt((dx1 * dx1 + dy1 * dy1) * (dx2 * dx2 + dy2 * dy2) + 1e-10))) * 180.0 / Math.PI);
			else return 360 - (float)(Math.Acos(((dx1 * dx2 + dy1 * dy2) / Math.Sqrt((dx1 * dx1 + dy1 * dy1) * (dx2 * dx2 + dy2 * dy2) + 1e-10))) * 180.0 / Math.PI);
		}*/
	}
	public struct Position
	{
		public PathData locatedPath;
		public float[] angle;
		public float location;

		public Position(PathData lp, float lo)
		{
			this.locatedPath = lp;
			this.angle = new float[3];
			this.location = lo;
		}
		public Position(PathData lp, float[] a, float lo)
		{
			this.locatedPath = lp;
			this.angle = new float[3];
			this.angle[0] = a[0];
			this.angle[1] = a[1];
			this.angle[2] = a[2];
			this.location = lo;
		}
		public void SetAngle(float[] a)
		{
			this.angle[0] = a[0];
			this.angle[1] = a[1];
			this.angle[2] = a[2];
		}
	}


}