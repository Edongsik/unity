using UnityEngine;
using System.Collections;
using System.Collections.Generic;



//싱글톤으로 만들기 
public class ItemManager : MonoBehaviour {

    static ItemManager instanceObj;
    static object Lock = new object();

    public static ItemManager Instance
    {
        get
        {
            lock (Lock)
            {
                instanceObj = (ItemManager)FindObjectOfType(typeof(ItemManager));

                if (FindObjectsOfType(typeof(ItemManager)).Length >1)
                {
                    return instanceObj;
                }

                if (instanceObj==null)
                {
                    GameObject singleton = new GameObject();
                    instanceObj = singleton.AddComponent<ItemManager>();
                    singleton.name = typeof(ItemManager).ToString();

                    DontDestroyOnLoad(singleton);
                }

               
            }


            return instanceObj;

        }
               
    }



    // 파싱한 정보를 다 저장할꺼에요.
    Dictionary<int, ItemInfo> itemData = new Dictionary<int, ItemInfo>();


    // 아이템 추가.

    public void AddItem(ItemInfo _itemInfo)

    {   // 아이템은 고유해야 되니까, 먼저 체크!
        if (itemData.ContainsKey(_itemInfo.Index)) 
        {
            return;
        }
        // 이제 아이템을 추가.
        itemData.Add(_itemInfo.Index, _itemInfo);
    }


    public ItemInfo GetItem(int _Id)
    {
        if (itemData.ContainsKey(_Id))
        {
            return itemData[_Id];
        }

        Debug.Log("GatItem=== 아이템아이디가 없습니다.");

        return null;

    }

    //아이템 데이타 저장 
    public Dictionary<int, ItemInfo> GetAllItems()
    {
        return itemData;
    }

    //아이템 데이타 수
    public int GetItemCount()
    {
        return itemData.Count;
    }
}
