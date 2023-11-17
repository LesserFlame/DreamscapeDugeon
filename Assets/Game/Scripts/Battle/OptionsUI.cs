using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class OptionsUI : MonoBehaviour
{
    public Image icon;
    public TextMeshProUGUI optionName;
    public bool selectable = true;

    public void OnUpdateDisplay(Sprite newicon, string newText)
    {
        if (icon != null && newText != null) 
        { 
            gameObject.SetActive(true);
            icon.sprite = newicon;
            optionName.text = newText;    
        }
        else
        {
            gameObject.SetActive(false);
        }
    }
    public void OnHighlight(bool highlight = true)
    {
        if (highlight)
        {
            optionName.color = Color.yellow;
            if (!selectable) optionName.color = Color.yellow * Color.gray;
        }
        else
        {
            optionName.color = Color.white;
            if (!selectable) optionName.color = Color.gray;
        }
    }

    //public void OnGrayout(bool grayout = true)
    //{
    //    if (grayout)
    //    {
    //        optionName.color = Color.gray;
    //        selectable = false;
    //    }
    //    else
    //    {
    //        //optionName.color = Color.white;
    //        selectable = true;
    //    }
    //}
}
