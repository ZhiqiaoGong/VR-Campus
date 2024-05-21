using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GraphData;
using GuiBook;
using LogicalGraph;

namespace GuiBook
{
	public class Paths : MonoBehaviour
	{

		//节点坐标数据
		//private GameObject Point;
		List<Node> nodes = new List<Node>();


		//数据库接口
		DaoOutput daoOutput;
		public List<PathData> pathDatas;

		//lg接口
		LgGraphOutput lgGraphOutput;

		//存储路径终点，方向
		List<newPathData> newPathDatas = new List<newPathData>();

		//
		private Slider SliderParent;
		public List<Slider> SliderChild;



		void Awake()
		{
			lgGraphOutput= GameObject.Find("LgGraph").GetComponent<LgGraphOutput>();
			//获取路径数据
			daoOutput = GameObject.Find("Dao").GetComponent<DaoOutput>();
			pathDatas = daoOutput.GetPathDatas();
			SliderChild = new List<Slider>();
			SliderParent = transform.Find("Slider").GetComponent<Slider>();
		}
		// Use this for initialization
		void Start()
		{
			//调用GuiBookbutton.cs中已经算好的点的坐标
			nodes = this.transform.parent.Find("Buttons").GetComponent<GuiBookButton>().point_pos;
            if (nodes==null)
            {
				nodes = GetComponent<GuiBookButton>().point_pos;
			}

			//pathdata->newpathdata
			foreach (var item in pathDatas)
			{
				newPathData n = new newPathData();
				n.start = item.start;
				n.end = item.end;
				n.length = item.length;
				n.vUrl = item.vUrl;
				//起点终点编号
				int a = item.start;
				int b = item.end;
				Vector3 start = new Vector3(nodes[a - 1].longitude, nodes[a - 1].latitude, 0);
				Vector3 end = new Vector3(nodes[b - 1].longitude, nodes[b - 1].latitude, 0);
				//计算中点坐标
				float x = (start.x + end.x) / 2.0f;
				float y = (start.y + end.y) / 2.0f;
				n.midPoint = new Vector3(x, y, 0);
				//计算角度
				Vector3 dir = new Vector3(end.x - start.x, end.y - start.y, 0);
				Vector3 R = new Vector3(1, 0, 0);
				n.angle = Vector3.Angle(dir, R);
				Vector3 normal = Vector3.Cross(dir, R);//叉乘求出法线向量
				n.angle *= Mathf.Sign(Vector3.Dot(normal, new Vector3(0, 0, 1)));  //求法线向量与物体上方向向量点乘，结果为1或-1，修正旋转方向


				//计算长度
				n.pathLength = (start - end).magnitude;
				//添加至newpathdata
				newPathDatas.Add(n);
			}
			


			//

			//设置根对象
			GameObject Slider_root = new GameObject();
			Slider_root.transform.SetParent(GameObject.Find("GuidBook/Map/Paths").transform);
			Slider_root.name = "GuiBookSliderRoot";

			//此处需要函数计算newPathDatas存储横纵坐标


			foreach (var item in newPathDatas)//newPathDatas node列表存储横纵坐标
			{
				Slider slider = Instantiate(SliderParent);
				slider.name = item.vUrl;
				slider.transform.SetParent(Slider_root.transform);
				//此处设置各滑动条对应位置、旋转设置
				slider.transform.position = item.midPoint;
				slider.transform.rotation = Quaternion.Euler(0, 0, -item.angle);
				slider.transform.localScale = new Vector3(item.pathLength / 90, 1, 1);
				slider.value = 0;
				//给每个slider增加监听器
				slider.onValueChanged.AddListener((float value) =>
				{
					changePath(value, slider.name);
				});

				SliderChild.Add(slider);
			}
		}

		public void changePath(float value,string name)
        {
			PathData pathdata = new PathData();
			foreach(var item in pathDatas)
            {
				if(name==item.vUrl)
                {
					pathdata = item;
					lgGraphOutput.ChangePosition(pathdata, value);
                }
            }
        }
	

		
	
		

	}
}








