using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SliderTextHandler : MonoBehaviour
{
    [SerializeField] private string displayText;
    [SerializeField] private TextMeshProUGUI textObject;
    [SerializeField] private Slider slider;
    public void OnValueChanged(float value, float maxValue)
    {
        textObject.text = displayText + value + " / " + maxValue;
        slider.maxValue = maxValue;
        slider.value = value;
    }
}
