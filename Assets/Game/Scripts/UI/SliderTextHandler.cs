using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using static UnityEngine.Rendering.DebugUI;

public class SliderTextHandler : MonoBehaviour
{
    [SerializeField] private string displayText;
    [SerializeField] private TextMeshProUGUI textObject;
    [SerializeField] private Slider slider;
    [SerializeField] private float speed = 2;
    [SerializeField] private bool percentage = false;

    public float trueValue = 0;
    public float currentValue = 0;
    private float maxValue = 0;
    public void SetValue(float value, float maxValue)
    {
        currentValue = value;
        trueValue = value;
        this.maxValue = maxValue;
        slider.maxValue = maxValue;
        slider.value = value;
        if (!percentage) textObject.text = displayText + (int)value + " / " + maxValue;
        else textObject.text = (int)(value * 100) + "%";
    }
    public void OnValueChanged(float value, float maxValue)
    {
        this.maxValue = maxValue;
        slider.maxValue = maxValue;
        //currentValue = trueValue;
        trueValue = value;
    }
    private void Update()
    {
        //if (currentValue < 1 && currentValue > 0) currentValue = 1;
        if (currentValue < 1 && trueValue == 0) currentValue = 0; //don't want to linger in death with 1 hp
        float tempValue = currentValue;
        currentValue = Mathf.Lerp(currentValue, trueValue, speed * Time.deltaTime);
        if (tempValue == currentValue) currentValue = trueValue;
        slider.value = currentValue;
        if (!percentage) textObject.text = displayText + Mathf.CeilToInt(currentValue) + " / " + maxValue;
        else textObject.text = (int)(currentValue * 100) + "%";

        //if (currentValue > maxValue - 1) currentValue = maxValue;
    }
}
