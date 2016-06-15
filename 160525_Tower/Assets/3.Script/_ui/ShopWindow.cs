using UnityEngine;
using System.Collections;

public class ShopWindow : MonoBehaviour {

public void Open()
    {
        gameObject.SetActive(true);
    }

public void Close()
    {
        gameObject.SetActive(false);
    }
}
