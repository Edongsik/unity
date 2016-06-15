using UnityEngine;
using System.Collections;
using System;

public class Fence : MonoBehaviour,IDamageable {

    public void ReceivedDamage(float _damageTaken)
    {
        //농장의 HP를 감소시킨다. 
        GameData.Instance.gm.ReceivedDamage(_damageTaken);
    }

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
