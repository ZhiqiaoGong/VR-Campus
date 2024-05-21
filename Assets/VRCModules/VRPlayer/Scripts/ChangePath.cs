using RenderHeads.Media.AVProVideo;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangePath : MonoBehaviour {

	private LgGraphOutput lg;
	[SerializeField]
	private MediaPlayer _mediaPlayer = null;
	private string m_VideoPath;
	private string baseUrl;
	public string current_path;
	DaoOutput dao;


	public void LoadVideo(string filePath)
	{
		
		_mediaPlayer.OpenVideoFromFile(MediaPlayer.FileLocation.RelativeToStreamingAssetsFolder, filePath, true);
	}
	// Use this for initialization
	void Start()
	{
		lg = GameObject.Find("LgGraph").GetComponent<LgGraphOutput>();
        if (_mediaPlayer==null)
        {
			_mediaPlayer = GetComponent<MediaPlayer>();
        }
		dao = GameObject.Find("Dao").GetComponent<DaoOutput>();
		baseUrl = dao.GetBaseUrl();
	}
	
	// Update is called once per frame
	void Update () {
		Debug.Log(current_path);
        if (!lg.GetPosition().locatedPath.vUrl.Equals(current_path))	
        {
			current_path = lg.GetPosition().locatedPath.vUrl;
			m_VideoPath = baseUrl + current_path;//路径对应的视频的路径
			LoadVideo(m_VideoPath);
		}


	}
}
