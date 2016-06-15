using UnityEngine;
using System.Collections;

public class ItemBehaviour : MonoBehaviour
{

    UISprite sprite;

    // Use this for initialization
    void Start()
    {

        sprite = GetComponent<UISprite>();


    }

    void OnClick()
    {
        sprite.color = new Color(Random.value, Random.value, Random.value);

        Debug.Log("ui클릭중");
    }

    //마우스 오버
    void OnHover(bool isOver)
    {
        //크기 키우기
        sprite.cachedTransform.localScale = (isOver) ? Vector3.one * 1.2f : Vector3.one;
    }

    void OnDrag(Vector2 delat)
    {
        sprite.transform.localPosition += (Vector3)delat;
    }

}
