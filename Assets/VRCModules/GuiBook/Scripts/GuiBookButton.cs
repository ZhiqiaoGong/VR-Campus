using GraphData;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GuiBook
{
    public class GuiBookButton : MonoBehaviour
    {
        //结点数据
        
        public int button_output;
       

        //cr
        private Button button_parent;
        public List<Button> buttonmin;
        
        //路径数据
        //public Image line;
        
        private List<PathData> pathDatas;


        //数据库接口
        DaoOutput daoOutput;
        LgGraphOutput lg;

        //存储数据库节点信息
        List<Node> nodes;
        public List<Node> point_pos = new List<Node>();

        //屏幕数据
        int offset = (int)(Screen.width / 400);
       


        void Awake()
        {
            daoOutput = GameObject.Find("Dao").GetComponent<DaoOutput>();
            lg = GameObject.Find("LgGraph").GetComponent<LgGraphOutput>();

            nodes = daoOutput.GetNodes();
            pathDatas = daoOutput.GetPathDatas();

            button_parent = transform.Find("Button").GetComponent<Button>();

            //cr
            buttonmin = new List<Button>();
            point_pos = Coordinate_Tran(nodes);
        }

        // Use this for initialization
        void Start()
        {
            //设置节点根对象
            GameObject buttons_root = new GameObject();
            buttons_root.transform.SetParent(GameObject.Find("GuidBook/Map/Buttons").transform);
            buttons_root.name = "GuiBookButtonRoot";


            

            foreach (var item in point_pos)
            {
 
                Button button = Instantiate(button_parent);
                button.name = item.name;
                button.transform.SetParent(buttons_root.transform);
                button.transform.position = new Vector3((float)item.longitude, (float)item.latitude, 0);

                button.onClick.AddListener(() =>
                {
                    Debug.Log("button.name" + item.id);
                    ChangePos(item.id);
                }
                );
                buttonmin.Add(button);

            }
            

        }

        

        public void ChangePos(int id)
        {
            lg.ChangePosition(id);
        }

        //List<Node> coord_cast_by_li(List<Node> pri_nodes)
        //{
        //    float base_x = range / 6;
        //    float base_y = range / 6;

        //    List<Node> res = new List<Node>();
        //    float min_x, min_y, max_x = 0, max_y = 0;
        //    min_x = pri_nodes[0].longitude;
        //    min_y = pri_nodes[0].latitude;
        //    foreach (var item in pri_nodes)
        //    {
        //        max_x = max_x < item.longitude ? item.longitude : max_x;
        //        max_y = max_y < item.latitude ? item.latitude : max_y;
        //        min_x = min_x >= item.longitude ? item.longitude : min_x;
        //        min_y = min_y >= item.latitude ? item.latitude : min_y;
        //    }
        //    float x_range = max_x - min_x;
        //    float y_range = max_y - min_y;

        //    float rate = Mathf.Cos(min_y * Mathf.PI / 180);

        //    foreach (var item in pri_nodes)
        //    {
        //        Node n = item;
        //        n.longitude = ((item.longitude - min_x) / x_range * range)+base_x;

        //        n.latitude = (((item.latitude - min_y) / y_range * range) + base_y)*rate;
        //        res.Add(n);
        //    }
        //    return res;

        //}


        public List<Node> Coordinate_Tran(List<Node> pri_nodes)
        {
            List<Node> res = new List<Node>();
            CoordinateTranformer coordinate = new CoordinateTranformer(pri_nodes[0]);
            foreach (var item in pri_nodes)
            {
                Node ress = item;
                Vector2 vec = coordinate.transform(item.longitude, item.latitude);
                ress.longitude = vec.x;
                ress.latitude = vec.y;
                res.Add(ress);//res.Add(coordinate.transform(item));
            }
            return res;
        }

    }

    public class CoordinateTranformer
    {
        double root_longitude;
        double root_laitude;
        private float MACRO_AXIS = 6378137; // 赤道圆的平均半径
        private float MINOR_AXIS = 6356752; // 半短轴的长度，地球两极距离的一半
                                            //半径的平方
        private double a;
        private double b;
        private double c0;
        private double d0;
        private double x;
        private double y;

        //chenran
        public float offX = 30;
        public float offY = 22;
        public float mulX = 1.175f;
        public float mulY = 1.13f;



        public CoordinateTranformer(Node node)
        {
            root_longitude = 117.137;
            root_laitude = 36.666;
            a = Mathf.Pow(MACRO_AXIS, (float)2.0);
            b = Mathf.Pow(MINOR_AXIS, (float)2.0);
            c0 = Mathf.Pow(Mathf.Tan((float)(2 * root_laitude * Mathf.PI / 360)), (float)2.0);
            d0 = Mathf.Pow(1 / Mathf.Tan((float)(2 * root_laitude * Mathf.PI / 360)), (float)2.0);
            x = a / Mathf.Sqrt((float)(a + b * c0));
            y = b / Mathf.Sqrt((float)(b + a * d0));
        }

        /// <summary>
        /// 输入经纬度，返回以root_longitude、root_latitude为原点的平面坐标
        /// </summary>
        /// <param name="longitude">数据库点经度，将作为x值</param>
        /// <param name="latitude">数据库点纬度，将作为y值</param>
        /// <returns></returns>
        public Vector2 transform(float longitude, float latitude)
        {
            Vector2 vec;
            //Node res = node;
            float c = Mathf.Pow(Mathf.Tan((float)(2 * latitude * Mathf.PI / 360)), (float)2.0);//数据库点纬度
            float d = Mathf.Pow(1 / Mathf.Tan((float)(2 * latitude * Mathf.PI / 360)), (float)2.0);//数据库点纬度
            double m = a / Mathf.Sqrt((float)(a + b * c));
            double n = b / Mathf.Sqrt((float)(b + a * d));

            vec.x = (float)(x * (2 * longitude * Mathf.PI / 360 - 2 * root_longitude * Mathf.PI / 360)) * (float)mulX + offX;
            vec.y = Mathf.Sqrt((float)((m - x) * (m - x) + (n - y) * (n - y))) * (float)mulY + offY;

            //res.latitude = Mathf.Sqrt((float)((m - x) * (m - x) + (n - y) * (n - y))) + 50;
            //res.longitude = (float)(x * (2 * node.longitude * Mathf.PI / 360 - 2 * root_longitude * Mathf.PI / 360)) + 50;//数据库经度
            return vec;
        }
        
        public Vector2 transform(Node node)
        {
            return transform(node.longitude, node.latitude);
        }
    }

    public class newPathData
    {
        public int start;
        public int end;
        public float length;
        public string vUrl;

        public Vector3 midPoint;
        public float angle;
        public float pathLength;

        public float advance;

    }

    public class pathDataToLg
    {
        public PathData pathData;
        public float advance;
    }
}
