using UnityEngine;
using System.Collections;
using System.IO;
using System.Xml;
using System.Text;
using System;
/***********************************
 * 
 * 아이템 정보 문서를 읽어옵니다. 
 * 
 * ********************************/
public class ItemParse : MonoBehaviour {

    //읽을 문서 
    string xmlName = "item.xml";


	// Use this for initialization
	void Start () {
        StartCoroutine(Process());
	}
	
	IEnumerator Process()
    {
        string filePath = string.Empty;

//#if (UNITY_EDITOR || UNITY_STANDALONE_WIN)
//        filePath += ("file:///");
//        filePath += (Application.streamingAssetsPath + "/" + xmlName);
//#elif UNITY_ANDROID
//        strPath =  "jar:file://" + Application.dataPath + "!/assets/" + m_strName;
//#endif

        WWW www = new WWW(filePath);

        yield return www;

        Debug.Log("Read Content : " + www.text);

        Interpret(www.text);
    }

    private void Interpret(string _text)
    { // 인코딩 문제 예외처리.
        // 읽은 데이터의 앞 2바이트 제거(BOM제거)
        StringReader stringReader = new StringReader(_text);

        stringReader.Read();// BOM 제거 한 데이터로 파싱해요.

        XmlNodeList xmlNodeList = null;

        XmlDocument xmlDoc = new XmlDocument();

        try
        {
            // XML 로드하고.
            xmlDoc.LoadXml(stringReader.ReadToEnd());
        }
        catch (Exception e)
        {
            xmlDoc.LoadXml(_text);
        }
      

        xmlNodeList = xmlDoc.SelectNodes("Items");

        foreach (XmlNode _node in xmlNodeList)
        {
            if (_node.Name.Equals("Items") && _node.HasChildNodes)
            {
                foreach (XmlNode child in _node.ChildNodes)
                {
                    //읽어서 작동할것 넣어주기 
                    ItemInfo item = new ItemInfo();
                    item.Index = int.Parse(child.Attributes.GetNamedItem("itemIndex").Value);
                    item.Buy_cost = int.Parse(child.Attributes.GetNamedItem("buy_coset").Value);
                    item.Sell_cost = int.Parse(child.Attributes.GetNamedItem("sell_coset").Value);
                    item.Name_En = child.Attributes.GetNamedItem("name_En").Value;
                    item.IconSpriteName = child.Attributes.GetNamedItem("icon").Value;
                    item.Name_Kr = child.Attributes.GetNamedItem("name_Kr").Value;


                    Debug.Log("@@@@@@@@@@@"+ item.IconSpriteName);

                    ItemManager.Instance.AddItem(item);



                }
            }
        }

    }
}
