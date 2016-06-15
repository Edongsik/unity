using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// 인벤토리를 관리하는 클래스 입니다.
/// 버튼을 누를때마다 아이템을 넣도록 할께요.
/// </summary>

public class InventoryController : MonoBehaviour
{

    // 지금은 DB가 없으니... 노다가로 아이템 이름을 넣어요.
    List<string> itemNames = new List<string>();

    // 새로 만들어진 아이템들을 모아둡니다.(삭제 및 수정 등을 하기위해서)
    List<Items> itemList = new List<Items>();

    // 우리가 만든 ItemObject를 복사해서 만들기 위해 선언합니다.
    public GameObject itemGameObj;

    // 스크롤뷰를 reposition 하기위해 선언합니다.
    public UIScrollView scrollView;

    // 그리드를 reset position 하기위해 선언합니다.
    public UIGrid grid;

    public UILabel infoLabel;//정보보여줄 레이블

    public GameObject selectBTN; //선택버튼

 // 속도 향상을 위해 현재 선택된 아이템을 저장해 놓겠습니다.
    Items selectedItem;
    int myMoney;

    void Start()
    {
       

        InitItems();

    }

    public void Update()
    {   // I 키를 누르면 아이템이 추가됩니다.
        if (Input.GetKeyDown(KeyCode.I))
        {
            AddItem();
        }
        // C키를 누르면 모두 삭제합니다.
        if (Input.GetKeyDown(KeyCode.C))
        {
            ClearAll();
        }
        // R키를 눌르면 랜덤으로 하나가 삭제됩니다.
        if (Input.GetKeyDown(KeyCode.R))
        {
            ClearRand(); 
        }
    }



    private void InitItems()
    {
        selectedItem = null;
        infoLabel.text = string.Empty;
        selectBTN.gameObject.SetActive(false);
    }



    // 키를 누르면 아이템을 화면에 보여주는 역할을 합니다.
    // 여기에서 가장 중요한 역할을 하는 함수죠.
    private void AddItem()
    { // 랜덤으로 만들도록 할께요.
      // 난수는 0부터 아이템이름 리스트의 갯수 - 1 만큼입니다.

      // 아이템 id가 1부터 시작하니까, 1부터 갯수만큼을 난수로 사용할께요.
        int ranIndex = Random.Range(1, ItemManager.Instance.GetItemCount());

        // 새로 만들어서 그리드 자식으로 넣겠습니다.   
        GameObject newitemObj = NGUITools.AddChild(grid.gameObject, itemGameObj);
        
        newitemObj.SetActive(true);

        // 이제 이름과 아이콘을 세팅할께요.
        // 그럴려면 먼저 아까 만든 ItemScript를 가져와야겠죠.
        // GetComponent는 해당 게임 오브젝트가 가지고 있는 컴포넌트를 가져오는 역할을 해요.
        Items newItem = newitemObj.GetComponent<Items>();
        newItem.SetInfo(ItemManager.Instance.GetItem(ranIndex));//여기 수정해야함

       // newitem.name = itemNames[ranIndex];
        // 이제 그리드와 스크롤뷰를 재정렬 시킵시다.
        grid.Reposition();
        scrollView.ResetPosition();
        // 그리고 관리를 위해 만든걸 리스트에 넣어둡시다.
        itemList.Add(newItem);
    }

    void ClearOne(Items _itemscrpt)
    {
        for (int i = 0; i <itemList.Count; i++)
        {
            // 인자로 넘어온것과 같으면 리스트에서 제거하고
            // GameObject를 없애서 화면에서 지워줍니다.
            if (_itemscrpt==itemList[i])
            {
                DestroyImmediate(itemList[i].gameObject);//바로 삭제
                itemList.RemoveAt(i); //인덱스도 지워주기

                break;

            }
        }
    }
    // 하나 랜덤으로 삭제합니다.
    private void ClearRand()
    {
        if (itemList.Count ==0)
        {

            return;
        }

        int ran = Random.Range(0,itemList.Count);
        DestroyImmediate(itemList[ran].gameObject);
        itemList.RemoveAt(ran);//리스트에서도 지워줌
        grid.Reposition();//정렬
        scrollView.ResetPosition();

    }

    //다지우기
    private void ClearAll()
    {
        for (int i = 0; i < itemList.Count; i++)
        {
            if (itemList[i] != null && itemList[i].gameObject !=null)
            {
                Destroy(itemList[i].gameObject);
            }
            else
            {
                Debug.Log("지울 아이템 없음");
            }
            
        }

        itemList.Clear(); //지우기
        grid.Reposition();//정렬
        scrollView.ResetPosition();//스크롤뷰 정렬
    }
    // 이 게임 오브젝트가 파괴될 때 생성했던 아이템도 삭제해줍시다.
    // 메모리 정리라고 생각하세요
    void OnDestroy()
    {
        for (int i = 0; i < itemList.Count; i++)
        { // 혹시 모르니까 null이 아닐 때에만 삭제하도록 해요.
            if (itemList[i] != null && itemList[i].gameObject !=null)
            {
                Destroy(itemList[i].gameObject);
            }
        }

        itemList.Clear();
        itemList = null;

        itemNames.Clear();
        itemNames = null;
    }

    // 아이템이 선택되면 이 함수가 호출됩니다.
    // 넘어온 정보를 가지고 이것저것 설정을 하고, 
    // 선택 프레임도 활성/비활성 시켜줍니다.
    public void SelectItem(Items _currentitem)
    { // 현재 선택된 정보와 같으면 표시할 갱신할 필요 없겠죠?
        if (selectedItem == _currentitem)
        {
            Debug.Log("같습니다.");
            return;
        }
        // 현재 선택된 아이템을 선택 아이템으로 저장
        selectedItem = _currentitem;

        for (int i = 0; i < itemList.Count; i++)
        {
           
            //if (selectedItem == itemList[i])
            //{
            //    itemList[i].IsSelected(true);
            //}
            //else
            //{
            //    itemList[i].IsSelected(false);
            //}
               itemList[i].IsSelected(selectedItem == itemList[i]);// 한줄로 하기 
        }

        infoLabel.text = "이름:" + selectedItem.itemInfo.Name_Kr + "\n판매금액 :" + selectedItem.itemInfo.Sell_cost + "입니다.";

        //판매버튼 보이기
        selectBTN.SetActive(true);
    }

    // 판매하는 함수 입니다.
    // 판매 누르면 해당 아이템이 삭제되는것 까지만? 할께요.

    public void Sellitems()

    {  // 현재 선택된 데이터가 없으면 안되겠죠?
        if (selectedItem==null) 
        {
            Debug.Log("팔 아이템이 없습니다.");
            return;
        }
        // 판매를 누르면 판매한 금액을 합산할께요.
        myMoney += selectedItem.itemInfo.Sell_cost;
        // 그리고 현재 아이템을 삭제해주어야 합니다.(위에 만든 삭제함수를 사용할꺼에요)
        ClearOne(selectedItem);
        // 그리고 현재 선택된 아이템 정보를 초기화 합니다.
        selectedItem = null;
        // 버튼도 숨기고 정보 레이블도 초기화합니다.
        infoLabel.text = string.Empty;
        selectBTN.SetActive(false);
        // 다시 정렬을 해줍시다.
        grid.Reposition();
        scrollView.ResetPosition();


        Debug.Log("현재 금액 : " + myMoney.ToString());
    }
}
