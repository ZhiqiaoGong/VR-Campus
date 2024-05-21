using UnityEngine;


/// <summary>
/// 下载视频/查看下载进度
/// </summary>
public class VideoDownloaderOutput : MonoBehaviour {

    VideoDownloader videoDownloader;
    void Start()
    {
        videoDownloader = GetComponent<VideoDownloader>();
    }

}
