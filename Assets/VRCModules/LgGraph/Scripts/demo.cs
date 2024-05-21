using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class demo : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
        

    }

    // Update is called once per frame
    void Update()
    {
        bool flag = false;
        if (!flag)
        {
            DaoOutput daoOutput = GameObject.Find("Dao").GetComponent<DaoOutput>();
            //daoOutput.testDao();
            flag = true;
        }

    }

     



}