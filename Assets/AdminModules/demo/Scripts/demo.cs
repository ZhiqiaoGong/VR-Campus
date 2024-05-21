using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using WebService;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Admin
{
    public class demo : MonoBehaviour
    {

        static string baseUrl;

        public enum Url
        {
            localhost,
            tecent_cloud_name,
            tecent_cloud_ip
        }

        public Url url = 0;
        private string[] urlList = {
            "http://localhost:8080/" ,
            "http://lizhuodong.club:8080/",
            "http://1.116.155.101:8080/"
        };

        void Start()
        {
            baseUrl = urlList[(int)url];
            StartCoroutine(Register("demouser", "demo"));
            StartCoroutine(ViewGraph());
        }




        public IEnumerator Register(string userName, string psw)
        {

            List<IMultipartFormSection> formData = new List<IMultipartFormSection>();
            string rqUrl = baseUrl + "user" + "/register?" + "userName=" + userName + "&password=" + psw;
            UnityWebRequest www = UnityWebRequest.Post(rqUrl, formData);
            yield return www.SendWebRequest();
            //
            if (www.responseCode >= 300)
            {
                Debug.Log(www.error);
            }
            else
            {
                string res = www.downloadHandler.text;
                res = JsonManager.ValueParser(res, "data");
                long id = long.Parse(JsonManager.ValueParser(res, "userId"));
            }
        }


        public IEnumerator ViewGraph()
        {
            string url = "http://lizhuodong.club:8080/GraphManager/viewMyGraphs";
            List<IMultipartFormSection> formData = new List<IMultipartFormSection>();
            UnityWebRequest www = UnityWebRequest.Get(url);
            
            JObject header = new JObject();
            //header.Add("token", "VXauOkMT5g+8LMWcs08oPVVzfd3NDWsZe/qE5cwHW8F4FbLvDGPXU0waZHy0siaV");
            //byte[] postBytes = System.Text.Encoding.Default.GetBytes(header.ToString());

            //www.uploadHandler = new UploadHandlerRaw(postBytes);
            www.SetRequestHeader("token", "VXauOkMT5g+8LMWcs08oPVVzfd3NDWsZe/qE5cwHW8F4FbLvDGPXU0waZHy0siaV");
            yield return www.SendWebRequest();


            if (www.responseCode >= 300)
            {
                Debug.Log(www.error);
            }
            string res = www.downloadHandler.text;
            Debug.Log(res);


        }





    }
}
