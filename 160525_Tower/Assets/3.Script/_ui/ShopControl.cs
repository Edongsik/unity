using UnityEngine;
using System.Collections;

public class ShopControl : MonoBehaviour
{

    public ShopWindow shopWindow;
    public UIButton shopButton;

    public void Awake()
    {
        shopButton.onClick.Add(new EventDelegate(OnClick_ShopBTN));
    }

    void OnClick_ShopBTN()
    {
        shopWindow.Open();
    }
}
