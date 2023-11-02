using System.Collections;
using System.Collections.Generic;
using System.Net;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class BattleUIManager : Singleton<BattleUIManager>
{
    [SerializeField] private OptionsMenuHandler optionsMenu;
    [SerializeField] private List<SliderTextHandler> sliderHandlers;
    [SerializeField] private List<GameObject> buttons;
    [HideInInspector] public bool detectInput = true;

    [SerializeField] private List<int> options;

    private BattleAction playerAction;

    private int menuOption = 0;
    private int menuLayer = 0;


    private void Start()
    {
        playerAction = BattleManager.Instance.player.GetComponent<BattleAction>();
        HideAll();
    }
    private void Update()
    {
        if (detectInput) 
        {
            MenuInput();
        }
    }

    private void MenuInput()
    {
        bool updateMenus = true;
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            menuOption++;
            if (menuOption >= options[menuLayer]) menuOption = 0;
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            menuOption--;
            if (menuOption < 0) menuOption = options[menuLayer]-1;
        }

        if (Input.GetKeyDown(KeyCode.Z))
        {
            bool swap = true;
            if (menuOption == 0 && menuLayer == 0 && swap)
            {
                menuLayer = 1;
                swap = false;
            }
            if (menuOption == 2 && menuLayer == 0 && swap)
            {
                BattleManager.Instance.OnPlayerFlee();
                swap = false;
            }
            if (menuLayer == 1 && swap)
            {
                playerAction.data = optionsMenu.battleActions[menuOption];
                menuLayer = 2;
                menuOption = 0;
                swap = false;
            }
            if (menuLayer == 2 && swap)
            {
                playerAction.target = BattleManager.Instance.activeEnemies[menuOption];
                playerAction.Perform();
                menuLayer = 0;
                menuOption = 0;
                updateMenus = false;
                BattleManager.Instance.OnPlayerDecide();
            }
            menuOption = 0;
        }
        else if (Input.GetKeyDown(KeyCode.X))
        {
            menuLayer--;
            if (menuLayer < 0) menuLayer = 0;
            menuOption = 0; 
        }
        if (Input.anyKeyDown && updateMenus)
        {
            switch (menuLayer)
            {
                case 0:
                    OnShowButtons();
                    OnSelectButton(menuOption, true);
                    OnShowOptions(false);
                    break;
                case 1:
                    OnShowButtons(false);
                    OnShowOptions();
                    OnHighlightOptions(menuOption);
                    optionsMenu.ChangeMenuList(menuLayer - 1);
                    break;
                case 2:
                    OnShowButtons(false);
                    OnShowOptions();
                    OnHighlightOptions(menuOption);
                    optionsMenu.ChangeMenuList(menuLayer - 1);
                    break;
            }
        }
    }

    public void OnSliderChanged(int id, float value, float maxValue, bool lerp = true)
    {
        if (value < 0) { value = 0; }
        if (id >= 0 && id < sliderHandlers.Count) 
        {
            if (lerp) sliderHandlers[id].OnValueChanged(value, maxValue);
            else sliderHandlers[id].SetValue(value, maxValue);
        }
    }
    public void OnShowButtons(bool show = true)
    {
        foreach (var button in buttons) 
        {
            button.GetComponent<Animator>().SetBool("Open", show);
            if (!show) button.GetComponent<Animator>().SetBool("Selected", show);
        }
    }
    public void OnSelectButton(int id, bool select = true)
    {
        foreach (var button in buttons)
        {
            button.GetComponent<Animator>().SetBool("Selected", false);
        }
        if (id >= 0 && id < buttons.Count)
        {
            buttons[id].GetComponent<Animator>().SetBool("Selected", select);
        }
    }
    public void OnShowOptions(bool show = true)
    {
        optionsMenu.GetComponent<Animator>().SetBool("Open", show);
    }
    public void OnHighlightOptions(int id)
    {
        if (id >= optionsMenu.optionsButtons.Count)
        {
            optionsMenu.UpdateDisplayOptions();
            id = optionsMenu.optionsButtons.Count -1;
        }
        if (id < 0)
        {
            optionsMenu.UpdateDisplayOptions(false);
            id = 0;
        }
        for (int i = 0; i < optionsMenu.optionsButtons.Count; i++)
        {
            optionsMenu.optionsButtons[i].OnHighlight(false);
        }
        optionsMenu.optionsButtons[id].OnHighlight();
    }
    public void OnInitialize()
    {
        optionsMenu.GetPlayerActions();
        optionsMenu.UpdateDisplayOptions(false);
        options[1] = optionsMenu.battleActions.Count;
    }
    public void HideAll()
    {
        OnShowButtons(false);
        OnShowOptions(false);
    }
}
