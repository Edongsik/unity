using UnityEngine;
using System.Collections;
/*
 *
 *  * 
     * 각 아이템은 이 클래스를 가지고 있습니다. 
     * 
     */



public class Items : MonoBehaviour
{


    public UISprite currentItem;//지금 아이템

    public UISprite itemFrame; //선택프레임스프라이트 

    public InventoryController IC; //인벤토리 컨트롤    

    public ItemInfo itemInfo;//아이템 정보 클래스 


    public void Awake()
    {
        itemFrame.gameObject.SetActive(false); //선택꺼줌
    }



    //정보설정
    public void SetInfo(ItemInfo _itemInfo)
    {
        itemInfo = _itemInfo;

        //적용할 스프라이트의 이름을 선택해준다. 
        // 같은 아틀라스에 있으니 스프라이트 이름 찾아 넣어주면 이미지가 바껴요.
        Debug.Log(itemInfo.IconSpriteName);
        currentItem.spriteName = itemInfo.IconSpriteName;
       

    }

    public void IsSelected(bool _selected)
    {
        // 선택 여부에 따라 조절합니다.
        itemFrame.gameObject.SetActive(_selected);
        // 이렇게 하셔도 됩니다.
        //m_sprFrame.enabled = bSelected;
    }

    //터치함수
    void OnClick()
    {
        Debug.Log(itemInfo.Index + "의 구매가격은 " + itemInfo.Buy_cost + ", 판매가격은 " + itemInfo.Sell_cost + "입니다."
            + itemInfo.IconSpriteName);

        //부모한테 선택됐다고 알리기 
        IC.SelectItem(this);
    }

}
