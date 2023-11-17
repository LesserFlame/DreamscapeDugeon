using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleScreenUIHandler : MonoBehaviour
{
    [SerializeField] private List<GameObject> buttons;

    [HideInInspector] public bool detectInput = true;

    private int menuOption = -1;
    //private int menuLayer = 0;
    
    void Update()
    {
        if (detectInput)
        {
            MenuInput();
        }
    }

    private void MenuInput()
    {
        int oldOption = menuOption;
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            menuOption++;
            if (menuOption >= buttons.Count) menuOption = 0;
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            menuOption--;
            if (menuOption < 0) menuOption = buttons.Count - 1;
        }

        if (Input.GetKeyDown(KeyCode.Z))
        {
            switch (menuOption)
            {
                case 0: LoadingScreen.Instance.LoadScene(1); break;
                case 2: Application.Quit(); Debug.Log("Quit"); break;
            }
        }

        if (Input.anyKeyDown)
        {
            if (oldOption != menuOption)
            {
                SelectButton(menuOption);
            }
        }
    }

    private void SelectButton(int id)
    {
        foreach (var button in buttons) 
        {
            button.GetComponentInChildren<TextMeshProUGUI>().color = Color.white;
        }
        buttons[id].GetComponentInChildren<TextMeshProUGUI>().color = Color.yellow;
    }
}
