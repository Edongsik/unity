//using UnityEngine;
//using System.Collections;
//using System.Collections.Generic;
//using System;
////XML용
//using System.Xml;
//using System.Xml.Serialization;

//public class EnemySpawn : MonoBehaviour
//{
   
//    public Transform EnemyPoolposition;

//    [Header("enemy List")]
//    public List<GameObject> poolenterEnemyList = new List<GameObject>();

//    //게임 오브젝트 풀 딕셔너리
//    Dictionary<string, GameObjectPool> gameObjPoolsDic = new Dictionary<string, GameObjectPool>();



//    //꺼내올 몬스터 게임 오브젝트 
//    List<GameObject> pullEnemyList= new List<GameObject>();   

//    //적 스폰할 위치 저장
//    [Header("enemy Spawn Position")] 
//    public List<Transform> spawnPositions = new List<Transform>();


//    // 포지션당 적 배치 갯수
//    int spawnEnemyObjsNum = 5;

//    //적 생성 XML 데이터 저장
//    public List<EnemyWaveData> enemyDatasList = new List<EnemyWaveData>();

//    public int totalEnemyCount;
 



//    public void Awake()
//    {
//        InitGameObjPools();

//    }
    

//    public void OnEnable()
//    {
//        //XML데이터 불러오기
//       LoadEnemyWaveDataFromXML();

//        //게임 오브젝트 풀 초기화 
       

//    }

//    #region Enemy 





//    // === 스폰 위치별로  5개씩 게임 오브젝트를 생성하여 게임 오브젝트 풀에 등록한다.

//    //==풀에 등록할 토탈 오브젝트수 
//    int makeObj =3;


//    void InitGameObjPools()
//    {
        
//        for (int i = 0; i < poolenterEnemyList.Count; i++)
//        {

//            //xml의 스폰포지션을 읽어와서  만들 오브젝트 , 갯수, 위치 
//            CreateGameObject(poolenterEnemyList[i], makeObj, EnemyPoolposition);

            
//        }

//        totalEnemyCount = poolenterEnemyList.Count * makeObj;
//        Debug.Log(totalEnemyCount);



//        //적 체력 표시 유저 인테페이스 생성 및 등록
//        // CreateGameObj(enemyHPbar, spawnEnemyObjsNum, enemyHPbarRoot,Vector3.one);

//        //적 캐릭터의 Dead애니메이션이 종료된후 나타날 아이템 
//        // CreateGameObj();
//    }






//    //=====풀에 게임 오브젝트 등록시키기 만들오브젝트, 양, 위치, 크기
//    void CreateGameObject(GameObject _gameObj, int _amunt, Transform _spwanPosition, Vector3 _scale = default(Vector3))
//    {
//        // 풀 만들기 
//        GameObjectPool tempGameObjectPool = new GameObjectPool(_spwanPosition, _gameObj);

//        //오브젝트 생성
//        for (int i = 0; i < _amunt; i++)
//        {

//            GameObject tempObj = Instantiate(_gameObj, _spwanPosition.position, Quaternion.identity) as GameObject;
//            tempObj.name = tempObj.name + i;
//            tempObj.transform.parent = _spwanPosition;

//            if (_scale != Vector3.zero)
//            {
//                tempObj.transform.localScale = _scale;

//            }

//            //풀에 등록
//            tempGameObjectPool.AddGameObject(tempObj);
//        }

//        gameObjPoolsDic.Add(_gameObj.name, tempGameObjectPool);

        
//    }

    

//    //=적 배치하기 시켜주기 --> XML에게 게임데이타 읽어온다. 
//    public void TakeOutEnemys(EnemyWaveData _enemyData)
//    {
       

//        //UI웨이브표시      
//        //  waveLB.text = _enemyData.waveNo.ToString();

//        //생성해야하는 숫자만큼 loop
//        for (int i = 0; i < _enemyData.amount; i++)
//        {
           

//            GameObject _currentSpawnGameObject;

         
//                _currentSpawnGameObject = Instantiate(gameObjPoolsDic[_enemyData.type].spawnObj,
//                                          spawnPositions[_enemyData.spawnPosition].transform.position,
//                                           Quaternion.identity) as GameObject;


//                _currentSpawnGameObject.transform.parent = spawnPositions[_enemyData.spawnPosition]; //위치지정
//                _currentSpawnGameObject.name = _enemyData.type + gameObjPoolsDic[_enemyData.type].lastIndex;//이름지정




//            //스폰된 적의 위치 
//            Debug.Log(_enemyData.spawnPosition);
//             _currentSpawnGameObject.transform.position = spawnPositions[_enemyData.spawnPosition].position;

//            //선택된 적 캐릭터를 초기화하여 작동시킨다.
//            _currentSpawnGameObject.tag = _enemyData.tagName; //태그

//            //새 오브젝트 만들기
//            Enemy currentEnemy = _currentSpawnGameObject.GetComponent<Enemy>();//컴포넌트추가
//            currentEnemy.InitEnemy(_enemyData.HP, _enemyData.AttackPower, _enemyData.MoveSpeed); //적 초기화

        

//            //=========게임오브젝트 풀에서 사용가능한 적 체력 표시 인터페이스가 있는지 체크
//            //GameObject _currentEnemyHPbar;
//            //if (!gameObjPools[enemyHPbar.name].NextGameObject(out _currentEnemyHPbar))
//            //{
//            //    //사용가능한 게임 오브젝트 없음 추가
//            //    _currentEnemyHPbar = Instantiate(enemyHPbar, gameObjPoolPosition.transform.position, Quaternion.identity) as GameObject;

//            //    _currentEnemyHPbar.transform.parent = enemyHPbarRoot;
//            //    _currentEnemyHPbar.transform.localScale = Vector3.one;
//            //    _currentEnemyHPbar.name = enemyHPbar.name + gameObjPools[enemyHPbar.name].lastIndex;
//            //    //풀에 등록하기
//            //    gameObjPools[enemyHPbar.name].AddGameObject(_currentEnemyHPbar);
//            //}


//            ////==========적 체력 표시 인터페이스 할당
//            //UISlider _tempEnemyHPbarSiler = _currentEnemyHPbar.GetComponent<UISlider>();
//            //currentEnemy.InitHPbar(_tempEnemyHPbarSiler, enemyHPBarPanel,enemyHPBarCam);//체력바 초기화 



//            if (_enemyData.tagName == "Boss")
//            {
//                //보스 등장
//            }
//        }
//    }



    

//    void CreateEnemyHPBar()
//    {

//    }

//    #endregion


//    #region XML
//    //XML을 읽어서 enemyWaveDatas에 저장한다.
//    void LoadEnemyWaveDataFromXML()
//    {
//        //이미 데이타를 로딩했으면 못하도록 예외처리
//        if (enemyDatasList != null && enemyDatasList.Count > 0)
//        {
//            Debug.Log("이미 데이타를 로딩");
//            return;
//        }
//        //XML파일읽기
//        TextAsset xmlText = Resources.Load("EnemyWaveData") as TextAsset;

//        //XML파일을 문서 객체 모델로 전환한다. 
//        XmlDocument xDoc = new XmlDocument();
//        xDoc.LoadXml(xmlText.text);
//        //XML파일 안에 EnemyWaveDAta란 xmlNode를 모두 읽어들인다. 
//        XmlNodeList nodeList = xDoc.DocumentElement.SelectNodes("EnemyWaveData");

//        XmlSerializer serializer = new XmlSerializer(typeof(EnemyWaveData));

//        //역직렬화를 통해 EnemyWaveData구조체로 변경하여 enemyWaveDatas맴버 필드에 저장한다. 
//        for (int i = 0; i < nodeList.Count; i++)
//        {
//            EnemyWaveData _enamyWaveData = (EnemyWaveData)serializer.Deserialize(new XmlNodeReader(nodeList[i]));
//            enemyDatasList.Add(_enamyWaveData);
//        }

//        Debug.Log("%%%%%%%" + enemyDatasList[0].waveNo);
//        Debug.Log("%%%%%%%" + enemyDatasList[0].type);
//    }

//    #endregion



//}
