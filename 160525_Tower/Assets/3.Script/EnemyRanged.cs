using UnityEngine;
using System.Collections;

public class EnemyRanged : Enemy {

    //발사할 게임 오브젝트
    public GameObject bullet;

    //발사위치
    public Transform firePosition;

    //발사 속도

    public float fireSpeed = 3;

    //발사할 게임 오브젝트 생성에 사용
}
