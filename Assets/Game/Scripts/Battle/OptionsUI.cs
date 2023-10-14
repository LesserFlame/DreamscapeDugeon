using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OptionsUI : MonoBehaviour
{
    public Image icon;
    public TextMeshProUGUI optionName;

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
        if(highlight) optionName.color = Color.yellow;
        else optionName.color = Color.white;
    }
}
