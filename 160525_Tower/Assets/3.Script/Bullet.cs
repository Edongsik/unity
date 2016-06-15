using UnityEngine;
using System.Collections;
using System;

public class Bullet :  MonoBehaviour 
{

    protected float attackPower = 1;

    public void InitShotObj(float _setupAttackPower)
    {
        attackPower = _setupAttackPower;
    }


    public void OnEnable()
    {
        Destroy(this.gameObject, 3.0f);
    }

  

    public void OnTriggerEnter2D(Collider2D _other)
    {
       // Debug.Log(_other.gameObject.name);

        if (_other.gameObject.tag == "Enemy" || _other.gameObject.tag == "Boss")
        {
             AttackAndRemove(_other);

           // Debug.Log(_other.gameObject.name+ "에게 공격 데미지 주기 ");
        }
    }

 

    public void Update()
    {

    }



    //데미지 주기 구현하기 
    protected void AttackAndRemove(Collider2D _other)
    {
        IDamageable damageTarget =  (IDamageable)_other.GetComponent(typeof(IDamageable)); //typeof는 개체를 얻어온다
        damageTarget.ReceivedDamage(attackPower);
        // 공격 후 초기화. 
       // Debug.Log("공격하고 있다."+ _other.gameObject);
        Destroy(this.gameObject, 1.0f);
    }
}
