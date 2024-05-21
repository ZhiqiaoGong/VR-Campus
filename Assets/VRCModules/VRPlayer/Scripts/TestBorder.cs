using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;
using System;

public class TestBorder : MonoBehaviour
{
    public GameObject camer1;
    public GameObject camer2;
    public GameObject txt1;
    public GameObject Alert;
    public float rw;
    public float rx;
    public float ry;
    public float rz;
    public bool istrigger;
    public float border;
    public float wucha;
    public float Movespeed;
    private int count_time;
    private bool flag;
    private float recorder;
    private Vector3 currentPos;
    private float error = 0.001f;
    // Start is called before the first frame update
    void Start()
    {
        //txt1.SetActive(false);
        Alert.SetActive(false);
        istrigger = false;
        flag = true;
        count_time = 0;
        currentPos = camer1.transform.position;
        
    }

    // Update is called once per frame
    void Update()
    {
        count_time++;
        //ControllSpeed(camer2.transform, camer1.transform);
        //camer2.transform.Translate(Vector3.forward * Time.deltaTime * Movespeed);
        //camer1.transform.Translate(Vector3.forward * Time.deltaTime * Movespeed);
        if (Input.GetKey(KeyCode.W))
        {
            Debug.Log("用户需要转身");
            rw = camer1.transform.rotation.w;
            rx = camer1.transform.rotation.x;
            ry = camer1.transform.rotation.y;
            rz = camer1.transform.rotation.z;
            //Debug.Log("rw:" + rw + " rx:" + rx + " ry:" + ry + " rz:" + rz);
        }
       
        
        //Debug.Log("Angle" + camer1.transform.eulerAngles.y);
        //Debug.Log("1:"+camer1.transform.position.x);
        //Debug.Log("2:" + camer1.transform.localPosition.x);
        /************************************************************PV锁***********************************************************************************/
        if (camer1.transform.localPosition.x >= border || camer1.transform.localPosition.x <= -border || camer1.transform.localPosition.z >= border || camer1.transform.localPosition.z <= -border)
        {
            Alert.SetActive(true);

            //txt1.SetActive(true);
            //rw = camer1.transform.rotation.w;
            //rx = camer1.transform.rotation.x;
            ry = camer1.transform.rotation.eulerAngles.y;
            //ControllCam(camer2.transform, camer1.transform, camer1.transform.rotation.eulerAngles.y - 1, new Vector2(camer1.transform.position.x, camer1.transform.position.z));
            //Debug.Log("ry:" + ry);

            if (flag == true)
            {
                recorder = ry;
                flag = false;
            }
            //rz = camer1.transform.rotation.z;

            istrigger = true;
        }
        else if (istrigger && IsTurnarround(camer1, recorder))
        {
            Alert.SetActive(false);
            //txt1.SetActive(false);
            Debug.Log("eulerAngles" + camer1.transform.rotation.eulerAngles.y);
            Debug.Log("localEulerAngles" + camer1.transform.localEulerAngles.y);
            ControllCam(camer2.transform, camer1.transform, camer1.transform.rotation.eulerAngles.y - 180, new Vector2(camer1.transform.position.x, camer1.transform.position.z));
            //camer2.transform.rotation=new Quaternion(-rw,-rx,-ry,-rz);
            istrigger = false;
            flag = true;
        }
        currentPos = camer1.transform.position;
      //  Debug.Log("pos:" + currentPos.x + " , " + currentPos.y + " , " + currentPos.z);

    }
    /*
        private void ControllSpeed(Transform cam, float Movespeed) 
        {
            Vector3 camLocation = cam.localPosition;
            transform.Translate(Vector3.forward * Movespeed * Time.deltaTime, cam);

        }
    */
    private void ControllSpeed(Transform camFather, Transform cam) 
    {
        Vector3 camLocalForward = cam.forward; 
        Vector3 xzForword = new Vector3(cam.forward.x, 0, cam.forward.z);
        cam.localPosition += xzForword * 2;
        //Debug.Log(cam.forward.x + " " + cam.forward.y + " " + cam.forward.z);
    }
    private void ControllCam(Transform camFather, Transform cam, float targetAngle, Vector2 targetPositionXZ)
    {
        //camFather.transform.position = new Vector3(0, 0, 0);
        //camFather.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));

        Vector3 camLocalRot = cam.localEulerAngles;
        Vector3 camLocalPos = cam.localPosition;
        Vector3 camLocalForward = cam.forward;

        float targetYAngle = -camLocalRot.y + targetAngle;
        Vector3 adjustedRot = new Vector3(0, targetYAngle, 0);
        camFather.transform.rotation = Quaternion.Euler(adjustedRot); 
        //Quaternion adjustedRotQua = Quaternion.Euler(adjustedRot);
        //camFather.transform.rotation = Quaternion.Slerp(camFather.transform.rotation, adjustedRotQua, Time.deltaTime * 100);

        
        float targetYPosition = camLocalPos.y - 0.2f;
        Vector3 adjustedPos = Quaternion.AngleAxis(targetYAngle, new Vector3(0, 1, 0)) * (-camLocalPos);
        camFather.transform.position = adjustedPos + new Vector3(targetPositionXZ.x, targetYPosition, targetPositionXZ.y);
        
        //Debug.Log(camFather.position.x + ", " + camFather.position.y + ", " + camFather.position.z);



        //Vector3 xzForword = new Vector3(cam.forward.x, 0, cam.forward.z);
        //Debug.Log(cam.forward.x+" "+ cam.forward.y+" " + cam.forward.z);
        //xzForword.Normalize();


        Vector3 xzRight = new Vector3(cam.right.x, 0, cam.right.z);
        //Debug.Log(cam.right.x + " " + cam.right.y + " " + cam.right.z);
        xzRight.Normalize();

        //if (enableInsta)
        //{
        //    currentInstaSphere.localPosition = cam.localPosition;
        //    Vector3 angles = new Vector3(camLocalRot.x, 0, camLocalRot.z);
        //    currentInstaSphere.localEulerAngles = angles;
        //}

        // Debug.Log(xzForword);

    }

    private void gainCam(Transform camFather, Transform cam, float targetAngle, Vector2 targetPositionXZ)
    {
        //camFather.transform.position = new Vector3(0, 0, 0);
        //camFather.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));

        Vector3 camLocalRot = cam.localEulerAngles;
        Vector3 camLocalPos = cam.localPosition;
        Vector3 camLocalForward = cam.forward;

        float targetYAngle = -camLocalRot.y + targetAngle;
        Vector3 adjustedRot = new Vector3(0, targetYAngle, 0);
        camFather.transform.rotation = Quaternion.Euler(adjustedRot);
        Quaternion adjustedRotQua = Quaternion.Euler(adjustedRot);
        camFather.transform.rotation = Quaternion.Slerp(camFather.transform.rotation, adjustedRotQua, Time.deltaTime * 100);
    }



        private bool IsMoving(GameObject camera)
    {
        if (currentPos.x < ( camera.transform.position.x - error ) || currentPos.x > ( camera.transform.position.x + error))
            return true;
        else
            return false;
    }
    public bool IsTurnarround(GameObject camer1, float ry)
    {
        if ((Math.Abs(camer1.transform.rotation.eulerAngles.y - ry) >= 180f - wucha && Math.Abs(camer1.transform.rotation.eulerAngles.y - ry) <= 180f + wucha) || ((360f - Math.Abs(camer1.transform.rotation.eulerAngles.y - ry)) >= 180f - wucha && (360f - Math.Abs(camer1.transform.rotation.eulerAngles.y - ry)) <= 180f + wucha))
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}

