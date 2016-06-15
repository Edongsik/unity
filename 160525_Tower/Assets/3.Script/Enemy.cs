using UnityEngine;
using System.Collections;
using System;

public enum EnemyState
{
    NONE,
    MOVE,
    ATTACK,
    DAMAGE,
    DEAD
}

public class Enemy : MonoBehaviour, IDamageable
{
    //몬스터 상태
    public EnemyState currentState = EnemyState.NONE;
    EnemyWaveData enemyWaveData;

    // LineCast에 사용될 위치.
    public Transform frontPosition;
    protected RaycastHit2D isObstacle;

    //이동속도
    public float moveSpeed = 1.0f;

    float destoryTime = 3.0f;
    float destroyTimer = 0;

    //체력
    protected float currentHp = 10;
    protected float maxHp = 10;

    //공격가능 여부 
    protected bool isEnableAttack = true; //가능
    protected float attackPower = 10; //공격력

    protected float damagedPower;//방어력?

    //컨퍼넌트
    protected Animator animator;
    protected Rigidbody2D rig2D;

    //체력바 관련 
  // UISlider hpBarSlider;
    // GameObject hpBarObj;

     UIPanel hpBarPanel;
    public Camera uiCam;


    Vector3 hpBarCalVec3;

   //public GameObject dropitem;

    #region MonoBehaviour

    public void Awake()
    {
        animator = gameObject.GetComponentInChildren<Animator>();
        rig2D = GetComponent<Rigidbody2D>();
        //아이템 감추기
      //  dropitem.SetActive(false);

    }

    void OnEnable()
    {
  


    }



    public void FixedUpdate()
    {

        switch (currentState)
        {
            case EnemyState.NONE:
                rig2D.velocity = Vector2.zero;
                break;


            case EnemyState.MOVE:

                Move();
                
                if (uiCam != null)
                {
                   
                    RepositionHPbar();
                }

                break;


            case EnemyState.ATTACK:
                rig2D.velocity = Vector2.zero;
                break;


            case EnemyState.DAMAGE:
                rig2D.velocity = Vector2.zero;
                animator.SetTrigger("DMG");
                break;


            case EnemyState.DEAD:
                rig2D.velocity = Vector2.zero;
                animator.SetTrigger("DEAD");
               // Destroy(this.gameObject);
                break;

        }

       // Debug.Log(destroyTimer);
    }



    #endregion

    #region 움직임 
    //초기화
    public void InitEnemy(float _MaxHP, float _AttackPower, float _MoveSpeed)
    {
        //IDEL 애니메이션 재생?
       //  animator.SetTrigger("isAlive");
        //HP등 설정
        maxHp = _MaxHP;
        currentHp = _MaxHP;
        attackPower = _AttackPower;
        moveSpeed = _MoveSpeed;

        //상태를 이동으로 바꿔줌
        currentState = EnemyState.MOVE;
        // isAlive 트리거를 초기화해서 dead 애니메이션 종료 후 walk 애니메이션 바로 전환되는 것을 방지.
      //  animator.ResetTrigger("isAlive");
    }
    #endregion


    #region HPBAR

    public void InitHPbar(UISlider _targetHPBar, UIPanel _targetPanel, Camera _targetCam)
    {
        Debug.Log("@@@@@@"+_targetPanel);
        //멤버필드할당
       // hpBarSlider = _targetHPBar;
      //  hpBarObj = hpBarSlider.gameObject;
        hpBarPanel = _targetPanel;
        uiCam = _targetCam;

        //오브젝트풀에서 제외되도록 초깃값 임시 수정
      //  hpBarObj.transform.localPosition = Vector3.left * 1000;

        //hpbar키기
        TurnOnOffHPBar(true);
    }

    protected void RepositionHPbar()
    {
       
        //적위치가 카메라 상에서 어느 위치인지 계산 
        hpBarCalVec3 = uiCam.WorldToScreenPoint(transform.position);

       
        hpBarCalVec3.z = 0;

        //위치조정
       
      //  hpBarObj.transform.localPosition = hpBarCalVec3;
        if (GameData.Instance.uiWidth == 0)
        {
           // GameData.Instance.uiWidth = hpBarPanel.width * (GameData.Instance.uiHeight / hpBarPanel.height);
            // Debug.Log("**************"+GameData.Instance.uiWidth);
        }

        hpBarCalVec3.x = (hpBarCalVec3.x / Screen.width) * GameData.Instance.uiWidth;
        hpBarCalVec3.y = (hpBarCalVec3.y / Screen.height) * GameData.Instance.uiHeight;
    }


    public void TurnOnOffHPBar(bool _isTurnOn = false)
    {
       // hpBarObj.SetActive(_isTurnOn);
    }



    #endregion


    //이동
    public void Move()
    {
        //장애물 검출
        isObstacle = Physics2D.Linecast(transform.position, frontPosition.position,
            1 << LayerMask.NameToLayer("Obstacle"));


        if (isObstacle)
        {
            //장애물 만나면 공격 애니메이션으로 전환
            if (isEnableAttack)
            {
                currentState = EnemyState.ATTACK;
                animator.SetTrigger("ATTACK");
                
            }


        }
        else
        {
            rig2D.velocity = new Vector2(-moveSpeed, rig2D.velocity.y);
        }
    }
    int count;

    public void AniloofCount()
    {
       count++;
       
        if (count >=3)
        {
            currentState = EnemyState.DEAD;
          
        }
    }

    //죽기 
 
        public void DeadEnemy()
    {
        Destroy(this.gameObject);
    }

  

    //공격
    public void Attack()
    {
        //농장에 피해를 가한다. 
        RaycastHit2D findObstacle = Physics2D.Linecast(
            transform.position, frontPosition.position,
            1 << LayerMask.NameToLayer("Obstacle"));



        if (findObstacle)
        {
            IDamageable damageTarget =
                (IDamageable)findObstacle.transform.GetComponent(typeof(IDamageable));
            damageTarget.ReceivedDamage(attackPower);
        }

    }
    



    //데미지 받기
    public void ReceivedDamage(float _damageTaken)
    {
        if (currentState == EnemyState.DEAD || currentState == EnemyState.NONE)
        {
            if (IsInvoking("ChangeStateToMove"))
            {
                CancelInvoke("ChangeStateToMove");
            }
            return;

        }
        //충돌 시 일정 시간 동안 이동정지
        currentState = EnemyState.DAMAGE;

        //움직임 상태면 멈추기 취소
        if (IsInvoking("ChangeStateToMove"))
        {
            CancelInvoke("ChangeStateToMove");
        }

        //이동상태로 바꿔주기
        Invoke("ChangeStateToMove", 0.3f);


        //체력을 감소 시킨다. 
        currentHp -= _damageTaken;
        // UI넣어주기


        //현재체력이 0 이하면
        if (currentHp <= 0)
        {
            currentState = EnemyState.DEAD;
            currentHp = 0;
            isEnableAttack = false;//공격못함           
          //  dropitem.SetActive(true);//아이템 보여주기
            //움직이기 멈추기
            if (IsInvoking("ChangeStateToMove"))
            {
                CancelInvoke("ChangeStateToMove");
            }

            //TODO 점수 증가 
            GameData.Instance.gm.AddSore(10);

            //오브젝트가 보스면 다시 적 생성
            if (gameObject.tag == "boss")
            {

                GameData.Instance.gm.SetUpGameStateToIdle();//게임상태를 다시 IDLE로 바꿔준다. 

            }
           // GetComponentInChildren<SpriteRenderer>().gameObject.SetActive(false);
            


        }
        else
        {
            animator.SetTrigger("DMG");
        }




    }




    #region 상태변경

    //이동시 상태 변화
   public void ChangeStateToMove()
    {
        //충돌에 의한 경직 상태에서 이동 상태로 변경
        currentState = EnemyState.MOVE;
    }



    #endregion

    #region  애니메이션 제어

    void AttackAniEnd()
    {
        if (currentState == EnemyState.ATTACK)
        {
            currentState = EnemyState.MOVE;
        }
    }

    #endregion



    #region UI



    #endregion
}
