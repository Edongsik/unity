using UnityEngine;
using System.Collections;
/*
 * 아이템 정보들 
 *
 */ 
public class ItemInfo
{
    int index;
    int sell_cost;
    int buy_cost;
    string iconSpriteName;
    string name_En;
    string name_Kr;

    public int Index
    {
        get
        {
            return index;
        }

        set
        {
            index = value;
        }
    }

    public int Sell_cost
    {
        get
        {
            return sell_cost;
        }

        set
        {
            sell_cost = value;
        }
    }

    public int Buy_cost
    {
        get
        {
            return buy_cost;
        }

        set
        {
            buy_cost = value;
        }
    }

    public string IconSpriteName
    {
        get
        {
            return iconSpriteName;
        }

        set
        {
            iconSpriteName = value;
        }
    }

    public string Name_En
    {
        get
        {
            return name_En;
        }

        set
        {
            name_En = value;
        }
    }

    public string Name_Kr
    {
        get
        {
            return name_Kr;
        }

        set
        {
            name_Kr = value;
        }
    }
}
