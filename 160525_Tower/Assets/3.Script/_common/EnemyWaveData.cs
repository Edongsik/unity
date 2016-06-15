using UnityEngine;
using System.Collections;
//XML용
using System.Xml;
using System.Xml.Serialization;



[XmlRoot]

public struct EnemyWaveData
{
    [XmlAttribute("waveNo")]
    public int waveNo;

    [XmlElement]
    public string type;

    [XmlElement]
    public int amount;

    [XmlElement]
    public int spawnPosition;

    [XmlElement]
    public int level;

    [XmlElement]
    public string tagName;

    [XmlElement]
    public float MoveSpeed;

    [XmlElement]
    public float AttackPower; //공격파워

    [XmlElement]
    public float HP;


}
