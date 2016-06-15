using UnityEngine;
using System.Collections;
using System.Collections.Generic;


//XML용
using System.Xml;
using System.Xml.Serialization;


public enum GameState
{
    READY, IDLE, GAMEOVER, WAIT,
}




public class GameManager : MonoBehaviour, IDamageable
{


    //게임 상황판단
    public GameState currentGameState = GameState.READY;

    EnemyState enemyState = EnemyState.NONE;

    Enemy enemy;

    //게임시작후 경과시간
    float timeStartGame = 0;
    
    float timeElapsed =0;
    public float timeOver = 10.0f;

    //획득한 점수 저장
    int score = 0;
    int seed = 0;
    int enemyScore = 0;
    int level = 0;
    int enemyCount = 0;

    //농장 HP
    float farmCurrentHP = 300;
    float farmMAxHP = 300;


    [Header("Farm UI")]
    public UILabel scoreLB;//스코어
    public UILabel playerLB;//플레이어레벨
    public UISlider farmHpSlider; //HP슬라이더
    public UILabel farmMaxHpNum;
    public UILabel farmNowHpNum;

    [Header("Center UI")]
    public UILabel waveLB;
    public UISlider gameTimer;
  

    public UILabel seedNowNum;
    public UILabel seedMaxNum;

    public UILabel enemyNowNum;
    public UILabel enemyMaxNum;


   // public EnemySpawn EnemySpawn;
  //  List<EnemyWaveData> enemyWaveDatas = new List<EnemyWaveData>();
    public int currentEnemyWaveDataIndexNo = 0;
    
    

    //적 체력 표시 유저 인터페이스 생성에 사용한다. 
    [Header("Enemy UI")]
    public GameObject enemyHPbar;
    public Transform enemyHPbarRoot;
    public UIPanel enemyHPBarPanel;
    public Camera enemyHPBarCam;

    // 코인 프리팹을 등록하는데 사용.
    public GameObject coinObj;
    public UILabel coinLb;

    [Header("result Window")]
    // 결과창 게임 오브젝트.
    public GameObject resultWindow;
    // 결과창에 사용되는 UILabel
    public UILabel resultHighScoreLb, 
        resultNowScoreLb,
        resultWaveLb,
        resultDeadEnemysLb, 
        resultGetCoinsLb;



    #region MonoBehavier


    public void Awake()
    {
        //스크립트 연결
        GameData.Instance.gm = this;
    

        //enemytotalSpownNum =EnemySpawn.spawnEnemyObjsList.Count; //총 스폰될 적 숫자 
       // enemyMaxNum.text = EnemySpawn.totalEnemyCount.ToString();

        farmNowHpNum.text = farmCurrentHP.ToString();
        farmMaxHpNum.text = farmMAxHP.ToString();

        InitGameObjPools();

    }


    public void OnDestroy()
    {//스크립트 연결 해제 
        GameData.Instance.gm = null;

    }

  

    public void Update()
    {
        //Debug.Log(enemyWaveDatas[0].type);
        //농장 현재 체력
        farmNowHpNum.text = farmCurrentHP.ToString();

        switch (currentGameState)
        {
            case GameState.READY:

                //게임이 시작되면 3초간 사용자에게 준비시간을줌 
                timeElapsed += Time.deltaTime;
                if (timeElapsed >= 2.0f)
                {
                    timeElapsed = 0;

                    //게임시작
                   SetUpGameStateToIdle();
                }
                break;

            case GameState.IDLE:
                //게임 시간 경과 
               // 타이머 작동
                timeElapsed += Time.deltaTime;
                gameTimer.value = (timeOver-timeElapsed )/ timeOver;
               

                if (timeElapsed >= timeOver)
                {
                    timeElapsed = timeOver;
                    GameOver();
                }

                
                break;

            case GameState.WAIT:

                timeElapsed += Time.deltaTime;

                break;

            case GameState.GAMEOVER:

               
                break;

        }


        
    }


    #endregion


    #region 농장관련

    //농장 데미지 받기 
    public void ReceivedDamage(float _damageTaken)
    {
        if (currentGameState == GameState.GAMEOVER)
        {
            return;
        }

        farmCurrentHP -= _damageTaken;
        farmHpSlider.value = farmCurrentHP/farmMAxHP;

#if UNITY_EDITOR
        //Debug.Log(farmCurrentHP / farmMAxHP);
       // Debug.Log("농장HP"+farmCurrentHP);
#endif

        if (farmCurrentHP <= 0)
        {
            //즉시부활아이템이 있을때 여기 넣어주기

            farmCurrentHP = 0;
            currentGameState = GameState.GAMEOVER;

            //결과창 표시
        }
    }

    //점수 

    public void AddSore(int _addScore)
    {
        if (currentGameState == GameState.READY || currentGameState == GameState.GAMEOVER)
        {
            return;
        }

        score += _addScore;
        enemyCount++;



        enemyNowNum.text = enemyCount.ToString();

#if UNITY_EDITOR
        Debug.Log(score);
#endif


    }
    

    #endregion

    #region 게임상태

    //==게임시작
    public void SetUpGameStateToIdle()
    {
        //게임 상태를 IDLE로 바꿔줌
        currentGameState = GameState.IDLE;
        enemyState = EnemyState.MOVE;
        //해제되지 못한 Invoke가 있음 해제하고 
        if (IsInvoking("CheckSpawnEnemy"))
        {
            CancelInvoke("CheckSpawnEnemy");
        }

        //새롭게 설정
        InvokeRepeating("CheckSpawnEnemy",0.5f,2.0f);
    }



    //적 생성 상태 체크 
    void CheckSpawnEnemy()
    {
      Debug.Log("CheckSpawnEnemy");
        enemyState = EnemyState.MOVE;
        // idle 상태가 아니라면 더이상 진행되지 못하도록 에러처리.
        if (currentGameState != GameState.IDLE)
        {
            Debug.Log("idle 상태가 아니라면 더이상 진행되지 못하도록 에러처리.");
            return;
        }

        Debug.Log("enemyDatasList.Count" + enemyDatasList.Count);
        //모든 적들이 생성되었다면
        if (currentEnemyWaveDataIndexNo >= enemyDatasList.Count)
        {
            Debug.Log("모든 적들이 생성");
            //게임종료
            //  currentGameState = GameState.GAMEOVER;
            CancelInvoke("CheckSpawnEnemy");
            //결과창 표시
            //OpenResult();

            return;

        }

        //적을 배치한다.  
        TakeOutEnemys(enemyDatasList[currentEnemyWaveDataIndexNo]);
        //SpawnEnemy(enemyDatasList[currentEnemyWaveDataIndexNo]);


        ////생성된 적이 Boss면 
        //if (EnemySpawn.enemyDatasList[currentEnemyWaveDataIndexNo].tagName == "boss")
        //{//생성 중지
        //    Debug.Log("생성된 적이 Boss면 " + enemyDatasList[currentEnemyWaveDataIndexNo].tagName);
        //    currentGameState = GameState.WAIT;
        //    CancelInvoke("CheckSpawnEnemy");

        //}

       currentEnemyWaveDataIndexNo++ ;
    }

    private void GameOver()
    {
        currentGameState = GameState.GAMEOVER;
    }
    #endregion

    #region 게임 동작 


    void Damage(float _damageTaken)
    {

        //농장체력 --> 처음 설정한 농장의 전체 체력으로 현재 농장체력을 나눠서 0~1사이로 만들어줌
        farmHpSlider.value = farmCurrentHP / farmMAxHP;
    }

    #endregion


    //==============================================================================================
    
    [Header("enemy List")]
    public Transform EnemyPoolposition=null;
    public List<GameObject> poolEnterEnemyList = new List<GameObject>();

    //게임 오브젝트 풀 딕셔너리
    Dictionary<string, GameObjectPool> gameObjPoolsDic = new Dictionary<string, GameObjectPool>();



    //꺼내올 몬스터 게임 오브젝트 
    List<GameObject> pullEnemyList = new List<GameObject>();

    //적 스폰할 위치 저장
    [Header("enemy Spawn Position")]
    public List<Transform> spawnPositions = new List<Transform>();


  

    //적 생성 XML 데이터 저장
    public List<EnemyWaveData> enemyDatasList = new List<EnemyWaveData>();

    public int totalEnemyCount;






    public void OnEnable()
    {
        //XML데이터 불러오기
        LoadEnemyWaveDataFromXML();

        //게임 오브젝트 풀 초기화 


    }

    #region Enemy 





    // === 스폰 위치별로  5개씩 게임 오브젝트를 생성하여 게임 오브젝트 풀에 등록한다.

    //==풀에 등록할 토탈 오브젝트수 
    int makeObj = 1;


    void InitGameObjPools()
    {

        for (int i = 0; i < poolEnterEnemyList.Count; i++)
        {

            //xml의 스폰포지션을 읽어와서  만들 오브젝트 , 갯수, 위치 
            CreateGameObject(poolEnterEnemyList[i], makeObj, EnemyPoolposition);


        }

        totalEnemyCount = poolEnterEnemyList.Count * makeObj;
        enemyMaxNum.text = totalEnemyCount.ToString();
    
        Debug.Log("totalEnemyCount" + totalEnemyCount);



        //적 체력 표시 유저 인테페이스 생성 및 등록
        // CreateGameObj(enemyHPbar, spawnEnemyObjsNum, enemyHPbarRoot,Vector3.one);

        //적 캐릭터의 Dead애니메이션이 종료된후 나타날 아이템 
        // CreateGameObj();
    }






    //=====풀에 게임 오브젝트 등록시키기 만들오브젝트, 양, 위치, 크기
    void CreateGameObject(GameObject _gameObj, int _amunt, Transform _spwanPosition, Vector3 _scale = default(Vector3))
    {
        // 풀 만들기 
        GameObjectPool tempGameObjectPool = new GameObjectPool(_spwanPosition, _gameObj);

        //오브젝트 생성
        for (int i = 0; i < _amunt; i++)
        {

            GameObject tempObj = Instantiate(_gameObj, _spwanPosition.position, Quaternion.identity) as GameObject;
            tempObj.name = _gameObj.name;
         
            tempObj.transform.parent = spawnPositions[Random.Range(0,spawnPositions.Count)];
            tempObj.transform.position = tempObj.transform.parent.position;

            if (_scale != Vector3.zero)
            {
                tempObj.transform.localScale = _scale;

            }

            //풀에 등록
            tempGameObjectPool.AddGameObject(tempObj);
        }

        Debug.Log(_gameObj.name);
        gameObjPoolsDic.Add(_gameObj.name, tempGameObjectPool);


    }



    //=적 배치하기 시켜주기 --> XML에게 게임데이타 읽어온다. 
    public void TakeOutEnemys(EnemyWaveData _enemyData)
    {


        //UI웨이브표시      
        waveLB.text = _enemyData.waveNo.ToString();

        //생성해야하는 숫자만큼 loop
        for (int i = 0; i < _enemyData.amount; i++)
        {
            GameObject _currentSpawnGameObject;
            if (!gameObjPoolsDic[_enemyData.type].stayGameObj(out _currentSpawnGameObject))
            {

                _currentSpawnGameObject = Instantiate(gameObjPoolsDic[_enemyData.type].spawnObj,
                                          spawnPositions[_enemyData.spawnPosition].transform.position,
                                           Quaternion.identity) as GameObject;


                _currentSpawnGameObject.transform.parent = spawnPositions[_enemyData.spawnPosition]; //위치지정
                _currentSpawnGameObject.name = _enemyData.type + gameObjPoolsDic[_enemyData.type].lastIndex;//이름지정

                gameObjPoolsDic[_enemyData.type].AddGameObject(_currentSpawnGameObject);
            }

        



            //스폰된 적의 위치 
            Debug.Log(_enemyData.spawnPosition);
            _currentSpawnGameObject.transform.position = spawnPositions[_enemyData.spawnPosition].position;

            //선택된 적 캐릭터를 초기화하여 작동시킨다.
            _currentSpawnGameObject.tag = _enemyData.tagName; //태그
            Enemy currentEnemy = _currentSpawnGameObject.GetComponent<Enemy>();//컴포넌트선택
            currentEnemy.InitEnemy(_enemyData.HP, _enemyData.AttackPower, _enemyData.MoveSpeed); //적 초기화



            //=========게임오브젝트 풀에서 사용가능한 적 체력 표시 인터페이스가 있는지 체크
            //GameObject _currentEnemyHPbar;
            //if (!gameObjPools[enemyHPbar.name].NextGameObject(out _currentEnemyHPbar))
            //{
            //    //사용가능한 게임 오브젝트 없음 추가
            //    _currentEnemyHPbar = Instantiate(enemyHPbar, gameObjPoolPosition.transform.position, Quaternion.identity) as GameObject;

            //    _currentEnemyHPbar.transform.parent = enemyHPbarRoot;
            //    _currentEnemyHPbar.transform.localScale = Vector3.one;
            //    _currentEnemyHPbar.name = enemyHPbar.name + gameObjPools[enemyHPbar.name].lastIndex;
            //    //풀에 등록하기
            //    gameObjPools[enemyHPbar.name].AddGameObject(_currentEnemyHPbar);
            //}


            ////==========적 체력 표시 인터페이스 할당
            //UISlider _tempEnemyHPbarSiler = _currentEnemyHPbar.GetComponent<UISlider>();
            //currentEnemy.InitHPbar(_tempEnemyHPbarSiler, enemyHPBarPanel,enemyHPBarCam);//체력바 초기화 



            if (_enemyData.tagName == "Boss")
            {
                //보스 등장
            }
        }
    }





    void CreateEnemyHPBar()
    {

    }

    #endregion


    #region XML
    //XML을 읽어서 enemyWaveDatas에 저장한다.
    void LoadEnemyWaveDataFromXML()
    {
        //이미 데이타를 로딩했으면 못하도록 예외처리
        if (enemyDatasList != null && enemyDatasList.Count > 0)
        {
            Debug.Log("이미 데이타를 로딩");
            return;
        }
        //XML파일읽기
        TextAsset xmlText = Resources.Load("EnemyWaveData") as TextAsset;

        //XML파일을 문서 객체 모델로 전환한다. 
        XmlDocument xDoc = new XmlDocument();
        xDoc.LoadXml(xmlText.text);
        //XML파일 안에 EnemyWaveDAta란 xmlNode를 모두 읽어들인다. 
        XmlNodeList nodeList = xDoc.DocumentElement.SelectNodes("EnemyWaveData");

        XmlSerializer serializer = new XmlSerializer(typeof(EnemyWaveData));

        //역직렬화를 통해 EnemyWaveData구조체로 변경하여 enemyWaveDatas맴버 필드에 저장한다. 
        for (int i = 0; i < nodeList.Count; i++)
        {
            EnemyWaveData _enamyWaveData = (EnemyWaveData)serializer.Deserialize(new XmlNodeReader(nodeList[i]));
            enemyDatasList.Add(_enamyWaveData);
        }

        Debug.Log("%%%%%%%" + enemyDatasList[0].waveNo);
        Debug.Log("%%%%%%%" + enemyDatasList[0].type);
    }

    #endregion



}






