using UnityEngine;
using WebService;


public class WebServiceOutput : MonoBehaviour {

    HttpPoster httpPoster;
    void Awake()
    {
        httpPoster = GetComponent<HttpPoster>();
    }

    void Start()
    {
        //TODO 改成其他出发形式？
        ShowUsers();
    }
    public void ShowUsers()
    {
        StartCoroutine(httpPoster.GetUsersList());
    }

    public void ChoseUser(long userId)
    {
        StartCoroutine(httpPoster.GetGraphsList(userId));
    }

    public void ChoseGraph(long id)
    {
        StartCoroutine(httpPoster.ChoseGraph(id));
    }


}
