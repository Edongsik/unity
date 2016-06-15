using UnityEngine;
using System.Collections;

public class FarmerTouchControl : MonoBehaviour
{

    public Camera mainCamera;

    public GameObject fireObj;

    public Transform firePoint;

    //새총 발사방향
    Bullet bullet;
    Vector3 fireDierction;


    public float fireSpeed = 5;

    bool isEnableAttack = true;

    Vector3 lastInputPosition;

    Vector3 tempVecter3;

    Vector2 tempVecter2 = new Vector2();

    //새총발사 오브젝트 처리
    GameObject tempObj;

    Animator animator;

    public void Awake()
    {
        animator = GetComponent<Animator>();
        bullet = fireObj.GetComponent<Bullet>();
    }
    #region 
    public void Update()
    {
        if (Input.GetMouseButton(0))
        {
            //마우스 입력 위치를 저장
            lastInputPosition = Input.mousePosition;
           // animator.SetBool("isAttack", false);
            //공격 가능 여부 판단

            if (isEnableAttack)
            {
                //공격애니메이션 전환
                animator.SetTrigger("Fire");
                

            }

        }

       // animator.SetBool("isAttack",true);

    }

    //public void OnDrawGizmos()
    //{
    //    Gizmos.color = Color.blue;
    //    Gizmos.DrawLine(firePoint.position, tempVecter3);
    //}
    #endregion

    
    void Fire(Vector3 _inputPosition)
    {
        //입력위치를 월드 좌표로 변환
        tempVecter3 = mainCamera.ScreenToWorldPoint(_inputPosition);
        tempVecter3.z = 0;

        fireDierction = tempVecter3 - firePoint.position;
        fireDierction = fireDierction.normalized;

        //발사
        tempObj = Instantiate(fireObj, firePoint.position, Quaternion.identity) as GameObject;

        //발사한 오브젝트 속도 계산
        tempVecter2.Set(fireDierction.x, fireDierction.y);
        tempVecter2 = tempVecter2 * fireSpeed;

        //속도 적용
        tempObj.GetComponent<Rigidbody2D>().velocity = tempVecter2;
        //공격력 전달
        bullet.InitShotObj(2.0f);
        

    }



    #region 애니메이션 이벤트 
    void FireTrigger()
    {
        //애니메이션 진행중엔 항상 공격
        Fire(lastInputPosition);
    }

    void FireEnd()
    {
        //애니메이션이 종료될때 공격가능
        isEnableAttack = true;
    }

    #endregion




}
