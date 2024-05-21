using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RDWmethod : MonoBehaviour
{
    private Vector3 userPosition;//获取用户位置
    private Vector3 lastPosition;
    public GameObject camFather;//vvhunter
    public AudioClip lert;//设置旋转提示音
    private Collider boundary;//设置边界
    private int count;//
    private bool flag;//给边界旋转值加锁
    private bool istrigger;//判断触发reset
    private float recorder;//记录边界值
    public float error;//设置旋转误差
    private bool isturn;//判断是否能旋转
    private Vector3 turnposition;
    //private GameObject quanjing_obj;

    // Start is called before the first frame update
    void Start()
    {
        count = 2;
        //lert.SetActive(true);
        isturn = true;
        flag = true;
        istrigger = false;
        userPosition = transform.position;
        lastPosition = transform.position;
        //MeshCollider 
        boundary = camFather.GetComponent<Collider>();
    }

    // Update is called once per frame
    void Update()
    {
        //quanjing_obj = change_test.user_Sphere;
        if ((!IsPointInCollider(boundary, transform.position)) && isturn)
        {
            Debug.Log("trigger");

            //rw = camer1.transform.rotation.w;
            //rx = camer1.transform.rotation.x;
            float ry = transform.rotation.eulerAngles.y;
            //ControllCam(camer2.transform, camer1.transform, camer1.transform.rotation.eulerAngles.y - 1, new Vector2(camer1.transform.position.x, camer1.transform.position.z));

            if (flag == true)
            {
                vedio_paly();
                recorder = ry;
                flag = false;
            }
            //rz = camer1.transform.rotation.z;

            istrigger = true;
            isturn = false;

        }
        if (istrigger && IsTurnarround(transform, recorder))
        {
            //lert.SetActive(false);
            //guidance.GetComponent<Player_Move>()._setposition = guidance.transform.localPosition;
            //turnclock = true;
            ControllCam(camFather.transform, transform, transform.rotation.eulerAngles.y - 180, new Vector2(transform.position.x, transform.position.z));
            //guidance.transform.rotation = Quaternion.LookRotation(camer1.transform.position);
            //camer2.transform.rotation=new Quaternion(-rw,-rx,-ry,-rz);
            istrigger = false;
            flag = true;
            turnposition = transform.position;
            count++;
        }
        if (istowards() && detect_dis(transform.position))
            isturn = true;
        //curvature_gain();
        switch (count % 2)
        {
            case 0:
                curvature_gain();
                break;
            default:
                curvature_gain_back();
                break;

        }
        //rotate_gain();
        
        //Debug.Log("localposition: " + transform.localPosition);
        //Debug.Log("localRotation: " + transform.localRotation);
        //Debug.Log("localRotation: " + transform.localRotation);
        //Debug.Log("forward: " + transform.forward);
    }
    private void LateUpdate()
    {
        userPosition = transform.position;
    }
    private bool detect_moving(Vector3 Userposition)
    {
        float dis = (userPosition - Userposition).magnitude;
        if (dis > 0.2 * Time.deltaTime)
            return true;
        else return false;
    }
    private bool detect_dis(Vector3 Userposition)
    {
        Vector2 Userpos = new Vector2(Userposition.x, Userposition.z);
        Vector2 turnpos = new Vector2(turnposition.x, turnposition.z);
        float dis = (turnpos - Userpos).magnitude;
        if (dis > 1 )
            return true;
        else
            return false;
    }
    /*
    private bool detect_rotating(Quaternion UserRotation)
    {
        float dis = userRotation.eulerAngles.y - UserRotation.eulerAngles.y;
        if (Mathf.Abs(dis) > 5)
            return true;
        else
            return false;
    }
    */
    private void ControllCam(Transform camFather, Transform cam, float targetAngle, Vector2 targetPositionXZ)
    {
        //quanjing_obj = change_test.user_Sphere;
        //camFather.transform.position = new Vector3(0, 0, 0);
        //camFather.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));

        Vector3 camLocalRot = cam.eulerAngles; 
        Vector3 camLocalPos = cam.position;
        Vector3 camLocalForward = cam.forward;

        float targetYAngle = -camLocalRot.y + targetAngle;
        Vector3 adjustedRot = new Vector3(0, targetYAngle, 0);
        camFather.transform.rotation = Quaternion.Euler(adjustedRot);
        transform.rotation = Quaternion.Euler(adjustedRot);
        //Quaternion adjustedRotQua = Quaternion.Euler(adjustedRot);
        //camFather.transform.rotation = Quaternion.Slerp(camFather.transform.rotation, adjustedRotQua, Time.deltaTime * 100);

        float targetYPosition = camLocalPos.y - 0.2f;
        Vector3 adjustedPos = Quaternion.AngleAxis(targetYAngle, new Vector3(0, 1, 0)) * (-camLocalPos);
        camFather.transform.position = adjustedPos + new Vector3(targetPositionXZ.x, targetYPosition, targetPositionXZ.y);
        transform.position = adjustedPos + new Vector3(targetPositionXZ.x, targetYPosition, targetPositionXZ.y);

        Vector3 xzForword = new Vector3(cam.forward.x, 0, cam.forward.z);
        xzForword.Normalize();
        //quanjing_obj = change_test.user_Sphere;
        //change_test.user_Sphere.transform.position = transform.position;
            ////    if (playProcessRate > 0.394)
            ////        quanjing_obj.rotation = Quaternion.Euler(new Vector3(quanjing_obj.rotation.eulerAngles.x, 90, quanjing_obj.rotation.eulerAngles.z));
            ////    quanjing_obj.forward = gp.curQuanjingFwd;
        //if (enableInsta)
        //{
        //    currentInstaSphere.localPosition = cam.localPosition;
        //    Vector3 angles = new Vector3(camLocalRot.x, 0, camLocalRot.z);
        //    currentInstaSphere.localEulerAngles = angles;
        //}

        // Debug.Log(xzForword);


    }
    private void ControllCam_rotate(Transform camFather, Transform cam, float targetAngle, Vector2 targetPositionXZ)
    {

        camFather.transform.position = new Vector3(0, 0, 0);
        camFather.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));

        Vector3 camLocalRot = cam.localEulerAngles;
        Vector3 camLocalPos = cam.localPosition;
        Vector3 camLocalForward = cam.forward;

        float targetYAngle = -camLocalRot.y + targetAngle;

        Vector3 adjustedRot = new Vector3(0, targetYAngle, 0);
        camFather.transform.rotation = Quaternion.Euler(adjustedRot);

        float targetYPosition = camLocalPos.y;
        Vector3 adjustedPos = Quaternion.AngleAxis(targetYAngle, new Vector3(0, 1, 0)) * (-camLocalPos);
        camFather.transform.position = adjustedPos + new Vector3(targetPositionXZ.x, targetYPosition, targetPositionXZ.y);

        Vector3 xzForword = new Vector3(cam.forward.x, 0, cam.forward.z);
        xzForword.Normalize();


    }
    private void curvature_gain()
    {
        Vector2 personPos = new Vector2(transform.position.x, transform.position.z);
        Vector2 currPos = new Vector2(lastPosition.x, lastPosition.z);
        if (detect_moving(transform.position) && istowards())
        {
            if ((personPos - currPos).magnitude >= 0.6)
            {
                ControllCam_rotate(camFather.transform, this.transform, this.transform.rotation.eulerAngles.y - 14, new Vector2(this.transform.position.x, this.transform.position.z));
                lastPosition = transform.position;

            }
        }
    }
    private void curvature_gain_back()
    {
        if (detect_moving(transform.position) && istowards())
        {
            if ((transform.position - lastPosition).magnitude >= 0.9)
            {
                ControllCam_rotate(camFather.transform, this.transform, this.transform.rotation.eulerAngles.y + 16, new Vector2(this.transform.position.x, this.transform.position.z));
                lastPosition = transform.position;

            }
        }
    }
    private bool istowards()
    {
        Vector3 towardsPos = transform.position - lastPosition;
        return Vector3.Dot(transform.forward, towardsPos) > 0 ? true : false;
    }
    public bool IsTurnarround(Transform camer, float ry)
    {
        if ((Mathf.Abs(camer.rotation.eulerAngles.y - ry) >= 180f - error && Mathf.Abs(camer.rotation.eulerAngles.y - ry) <= 180f + error) || ((360f - Mathf.Abs(camer.transform.rotation.eulerAngles.y - ry)) >= 180f - error && (360f - Mathf.Abs(camer.transform.rotation.eulerAngles.y - ry)) <= 180f + error))
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    public static bool IsPointInCollider(Collider bound, Vector3 point)
    {
        Bounds bds = bound.bounds;
        if (bds.Contains(point))
            return true;
        else
            return false;
    }
    private void vedio_paly()
    {
        AudioSource.PlayClipAtPoint(lert, transform.position);
    }
}
