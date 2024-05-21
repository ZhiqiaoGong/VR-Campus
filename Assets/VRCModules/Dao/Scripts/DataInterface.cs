using System.Collections;
using System.Collections.Generic;
using GraphData;
using UnityEngine;

public interface DataInterface{

    List<Node> GetAllNodes();

    List<PathData> GetAllPathdatas();
    
   /// <summary>
   /// 返回视频的本地存储路径
   /// </summary>
   /// <returns></returns>
   string GetLocalStorageUrl();

    /// <summary>
    /// 返回视频源路径
    /// </summary>
    /// <returns></returns>
   string GetSourceUrl();


}
