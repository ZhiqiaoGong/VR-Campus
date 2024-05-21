using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 用于转化、保存来自web等处的实体
/// </summary>
namespace WebService
{


	[Serializable]
	public struct Graph
	{
		public long graphId;
		public string graphName;
		public static Graph CreateFromJSON(string jsonString)
		{
			return JsonUtility.FromJson<Graph>(jsonString);
		}
	}


	[Serializable]
	public struct PathData
    {
		public long start;
		public long end;
		public string vurl;
		public double length;

		//public static PathData CreateFromJSON(string jsonString)
		//{
		//	return JsonUtility.FromJson<PathData>(jsonString);
		//}
	}

	[Serializable]
	public struct Node
    {
		public long id;
		public string name;
		public double longitude;
		public double latitude;
		//public static Node CreateFromJSON(string jsonString)
		//{
		//	return JsonUtility.FromJson<Node>(jsonString);
		//}
	}

	[Serializable]
	public struct User
    {
		public long userId;
		public string userName;
		//public static User CreateFromJSON(string jsonString)
		//{
		//	return JsonUtility.FromJson<User>(jsonString);
		//}
	}

	public class JsonManager
    {
		public static object CreateFromJSON(string jsonString)
		{
			return JsonUtility.FromJson<object>(jsonString);
		}

		public static List<T> JsonArray2List<T>(string jsonArray)
		{
			//JsonConvert能自动将Json数组装配到迭代器中，但需要先确保这是一个Json数组
            if (jsonArray.Contains("[")&&jsonArray.Contains("]"))
            {
				return JsonConvert.DeserializeObject<List<T>>(jsonArray);
            }
            else
            {
				var res = JsonConvert.DeserializeObject<T>(jsonArray);
				List<T> list = new List<T>();
				list.Add(res);
				return list;
            }
		}


		public static string ValueParser(string source,string key)
        {
			JObject keyValuePairs = JObject.Parse(source);
			return keyValuePairs[key].ToString();
        }

		public static string toJsonString(object entity)
        {
			return JsonConvert.SerializeObject(entity);
        }
			

	}











}

