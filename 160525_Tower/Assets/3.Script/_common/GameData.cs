using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// sealed 한정자를 통해서 해당 클래스가 상속이 불가능하도록 조치.
public sealed class GameData  {

    public GameManager gm;

    //싱글톤 인스턴스 저장

    private static volatile GameData uniqueInstance; //volatile 동시에 실행되는 여러 스레드에 의해 필드가 수정될수 있다. 
    private static object _lock = new System.Object();
    

    //생성자
    private GameData() { }


    public static GameData Instance
    {
        get
        {// lock으로 지정된 블록안의 코드를 하나의 쓰레드만 접근하도록 한다.
            lock (_lock)
            {
                if (uniqueInstance == null)
                {
                    uniqueInstance = new GameData();

                }
            }
            return uniqueInstance;
        }
          

    }


    //UI 카메라 사이즈 
    public float uiWidth = 0.0f;
    public float uiHeight = 640.0f;

}
