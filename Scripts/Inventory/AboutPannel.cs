using System;
using System.Collections;
using System.Collections.Generic;
using Scriptable;
using TMPro;
using UnityEditor;
using UnityEngine;

public class AboutPannel : MonoBehaviour
{
    [SerializeField] private TMP_Text title;
    [SerializeField] private TMP_Text description;
    [SerializeField] private TMP_Text extraInfo;

    private bool _isBagDisplayed = true;
    

    private void Start()
    {
        gameObject.SetActive(false);
    }

    public void ShowInfo(Item item)
    {
        if (!_isBagDisplayed)
            return;
        
        gameObject.SetActive(true);
        if (item is null)
        {
            gameObject.SetActive(false);
            return;
        }

        title.text = item.Name;
        description.text = item.Desrcription;

        // string extra = "";
        // if (item is Weapon weapon)
        //     extra = weapon.ExtraInfo();
        // else if (item is HealItem heal)
        //     extra = heal.ExtraInfo();
        // else if (item is Melee melee)
        //     extra = melee.ExtraInfo;

        string extra = item.ExtraInfo();
        
        if (extra != "")
        {
            extraInfo.gameObject.SetActive(true);
            extraInfo.text = extra;
        }
        else
        {
            extraInfo.gameObject.SetActive(false);
        }

        
    }
    
    public void SetIsBagDisplayed(bool isDisplayed)
    {
        _isBagDisplayed = isDisplayed;
    }
}