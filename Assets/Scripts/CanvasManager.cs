using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;
using UnityEngine.UI;
using WebService;

///<summary>
///控制界面之间的切换
///</summary>

public class CanvasManager : MonoBehaviour
{

	public GameObject Welcom_Canvas;
	public GameObject Login_Canvas;
	public GameObject Register_Canvas;
	public GameObject Remind_Canvas;
	public GameObject User_Canvas;
	public GameObject Graph_Canvas;
	public GameObject ChoseGraph_Canvas;
	public GameObject AddNode_Canvas;
	public GameObject AddPath_Canvas;

	uiInterface inter;
	long userID;
	List<Graph> g;


	// Use this for initialization
	void Start()
	{
		Status = UIStatus.Welcom;
	}

	public enum UIStatus//定义枚举，列举界面切换的情况
	{
		Welcom,
		Login,
		Register,
		Remind,
		User,
		Graph,
		ChoseGraph,
		AddNode,
		AddPath,
	}
	private UIStatus uistatus;//创建枚举变量
	private UIStatus preuistatus;
	private UIStatus Status//定义属性给枚举变量赋值
	{
		get
		{
			return uistatus;
		}
		set
		{
			uistatus = value;
			UpdateUI();//在给枚举变量赋值后调用UI显示方法，控制UI的显示
		}
	}


	/// <summary>
	/// ui切换的方法
	/// </summary>
	public void UpdateUI()//定义UI显示的方法，通过枚举变量的值来判断，如果是该ui，则括号中的值为true,即激活该ui
	{
		Welcom_Canvas.SetActive(uistatus == UIStatus.Welcom);
		Login_Canvas.SetActive(uistatus == UIStatus.Login);
		Register_Canvas.SetActive(uistatus == UIStatus.Register);
		Remind_Canvas.SetActive(uistatus == UIStatus.Remind);
		User_Canvas.SetActive(uistatus == UIStatus.User);
		Graph_Canvas.SetActive(uistatus == UIStatus.Graph);
		ChoseGraph_Canvas.SetActive(uistatus == UIStatus.ChoseGraph);
		AddNode_Canvas.SetActive(uistatus == UIStatus.AddNode);
		AddPath_Canvas.SetActive(uistatus == UIStatus.AddPath);

	}
	public void Welcom()//显示欢迎界面的方法
	{
		Status = UIStatus.Welcom;//给属性Status赋值，赋值的同时调用了UpdateUI方法
	}
	public void Login()//显示登陆界面的方法
	{
		Status = UIStatus.Login;
	}
	public void SureLogin()
	{
		//获取id密码
		InputField id = transform.Find("InputField_Login_Username").gameObject.GetComponent<InputField>();
		InputField psw = transform.Find("InputField_Login_Userpsw").gameObject.GetComponent<InputField>();
		string sid = id.text;
		string spsw = psw.text;
		inter.Login(sid, spsw);

        if (inter.getLoginCode() == 100)
        {
			Remind("账号或密码错误，请重新输入");
			return;
        }
		else if(inter.getLoginCode() == 200)
        {
			User();
        }
	}

	public void Register()//显示注册界面的方法
	{
		Status = UIStatus.Register;
	}
	public void SureRegister()
	{
		preuistatus = Status;

		//获取id密码
		InputField id = transform.Find("InputField_Login_Username").gameObject.GetComponent<InputField>();
		InputField psw = transform.Find("InputField_Login_Userpsw").gameObject.GetComponent<InputField>();
		InputField psw2 = transform.Find("InputField_Login_Userpsw (1)").gameObject.GetComponent<InputField>();

		if (!psw.Equals(psw2))
		{
			Remind("两次输入密码不一致");
			return;
		}

		string sid = id.text;
		string spsw = psw.text;
		inter.Register(sid, spsw);

        if (inter.getRegisterCode() == 1)
        {
			Remind("用户名已存在，请重新输入");
			return;
        }

		userID = inter.GetId();
		//提示用户id
		int iid = (int)userID;
		Remind("您的帐号为：" + iid + " 请牢记！");
		preuistatus = UIStatus.Login;
	}

	public void Remind(string t)//显示提醒框界面的方法
	{
		Status = UIStatus.Remind;
		Text text = transform.Find("Text_Remind").gameObject.GetComponent<Text>();
		text.text = t;
	}
	public void SureRemind()
	{
		Status = preuistatus;
	}

	public void User()//显示用户界面的方法
	{
		Status = UIStatus.User;
		preuistatus = Status;
		Text tid = transform.Find("Text_Username (2)").gameObject.GetComponent<Text>();
		int iid = (int)userID;
		tid.text = iid + " ";
	}

	public void Graph()//显示展示图界面的方法
	{
		Status = UIStatus.Graph;
		preuistatus = Status;

		inter.ViewGraph();
		g = inter.GetGraphs();
		for (int i = 0; i < g.Count; i++)
		{
			string s = "Text_ShowMessage" + i;
			Text text = transform.Find(s).gameObject.GetComponent<Text>();
			int id = (int)g[i].graphId;
			text.text = "id:" + id + "图名称" + g[i].graphName;
		}
	}
	public void SureChoose()
    {
		for (int i = 0; i < g.Count; i++)
		{
			string t = "Toggle" + i;
			Toggle tog = transform.Find(t).gameObject.GetComponent<Toggle>();
			if (tog.isOn)
			{
				ChoseGraph(i);
			}
		}
	}

	public void ChoseGraph(int i)
    {
		Status = UIStatus.ChoseGraph;
		preuistatus = Status;

		inter.ChooseGraph(g[i].graphId);
		List<Node> nodes = inter.GetNodes();
		List<PathData> paths = inter.GetPathDatas();
		Text text = transform.Find("Text_ShowMessage2").gameObject.GetComponent<Text>();
		string t= "节点：\n";
        for (int j = 0; j < nodes.Count; j++)
        {
			t = t + "id："+nodes[j].id+"\n名字："+nodes[j].name + "\n经度："+nodes[j].longitude + "\n纬度：" + nodes[j].latitude+"\n";
        }
		for (int j = 0; j < paths.Count; j++)
		{
			t = t + "起点：" + paths[j].start + "\n终点：" + paths[j].end + "\n长度：" + paths[j].length + "\nv" +
				"Url：" + paths[j].vurl + "\n";
		}
		text.text = t;
	}

	public void AddNode()//显示增加节点界面的方法
	{
		Status = UIStatus.AddNode;
		preuistatus = Status;
	}
	public void SureAddnode()
    {
		InputField name = transform.Find("InputField_name").gameObject.GetComponent<InputField>();
		InputField longti = transform.Find("InputField_long").gameObject.GetComponent<InputField>();
		InputField lati = transform.Find("InputField_lati").gameObject.GetComponent<InputField>();
		InputField id = transform.Find("InputField_id").gameObject.GetComponent<InputField>();

		if (id.text == null)
		{
			inter.AddNode(name.text, double.Parse(longti.text), double.Parse(lati.text));
			name.text = null;
			longti.text = null;
			lati.text = null;
			id.text = null;
		}
		else
		{
			inter.AddNode(name.text, double.Parse(longti.text), double.Parse(lati.text), int.Parse(id.text));
			name.text = null;
			longti.text = null;
			lati.text = null;
			id.text = null;
		}
	}
	public void AddPath()//显示增加路径界面的方法
	{
		Status = UIStatus.AddPath;
		preuistatus = Status;
	}
}