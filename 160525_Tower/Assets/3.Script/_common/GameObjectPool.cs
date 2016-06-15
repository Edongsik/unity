using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

//XML용
using System.Xml;
using System.Xml.Serialization;


public class GameObjectPool {


    int poolNowIndex = 0;
    int count = 0;
    float spawnPositionX = 0;
    Transform spawnPosition;
    public GameObject spawnObj;
    List<GameObject> pool = new List<GameObject>();



    //생성자
    public GameObjectPool(Transform _position, GameObject _initSpawnObj)
    {
        spawnPosition = _position;
        spawnObj = _initSpawnObj;

    }

    //게임 오브젝트를 풀에 추가한다. 
    public void AddGameObject(GameObject _addGameObject)
    {
        pool.Add(_addGameObject);
        count++; 
    }



    //사용중이지 않은 게임 오브젝트를 선택한다.     
    //out 키워드는 인수를 참조로 전달하는 데 사용됩니다
    public bool stayGameObj(out GameObject _returnObject)//out 인수가 가지고 있는 값을 가지고옴
    {
        int startIndexNo = poolNowIndex;

        if (lastIndex==0)
        {
            _returnObject = default(GameObject);
            return false;
        }

        //처음 만들어진 위치보다 왼쪽에 위치하면 이미 사용된걸로 판단.
        while (pool[poolNowIndex].transform.position.x < spawnPositionX)
        {
            poolNowIndex ++;
            poolNowIndex = (poolNowIndex >= count) ? 0 : poolNowIndex;

            //사용가능한 게임 오브젝트가 없을때
            if (startIndexNo ==poolNowIndex)
            {
                _returnObject = default(GameObject);
                return false;

            }
        }

        //풀에 있는 오브젝트를 넣어준다. 
        _returnObject = pool[poolNowIndex];
        return true;

    }

    public int lastIndex
    {
        get { return pool.Count; }
    }


    // 해당 인덱스의 게임 오브젝트가 존재하는 경우 반환.
    public bool GetObject(int _index, out GameObject _obj)
    {
        if (lastIndex <_index || pool[_index] ==null)
        {
            _obj = default(GameObject);

            return false;
        }

        _obj = pool[_index];
        return true;
    }

    
  


}
