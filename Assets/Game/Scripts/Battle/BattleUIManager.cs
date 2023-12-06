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
    /*[HideInInspector]*/ public bool detectInput = false;
    [HideInInspector] public bool active = false;
    [SerializeField] private float actionDelay = 1;
    [SerializeField] private GameObject spellFX; 

    [SerializeField] public List<int> options; 
    [SerializeField] private List<GameObject> menuSounds; 
    [SerializeField] private FireLight fireLight; 

    private BattleAction playerAction;

    private int menuOption = 0;
    private int menuLayer = 0;


    private void Start()
    {
        playerAction = BattleManager.Instance.player.GetComponent<BattleAction>();
        HideAll();
        active = false;
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
            OnPlaySound(0);
            menuOption++;
            if (menuOption >= options[menuLayer]) menuOption = 0;
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            OnPlaySound(0);
            menuOption--;
            if (menuOption < 0) menuOption = options[menuLayer]-1;
        }

        if (Input.GetKeyDown(KeyCode.Z))
        {
            bool swap = true;
            if (menuOption == 0 && menuLayer == 0 && swap)
            {
                OnPlaySound(1);
                menuLayer = 1;
                menuOption = 0;
                //optionsMenu.UpdateDisplayOptions();
                BattleManager.Instance.player.animator.SetBool("StaffEquip", true);
                OnHighlightOptions(menuOption);
                swap = false;
            }
            if (menuOption == 1 && menuLayer == 0 && swap) OnPlaySound(4);
            if (menuOption == 2 && menuLayer == 0 && swap)
            {
                OnPlaySound(1);
                BattleManager.Instance.OnPlayerFlee();
                swap = false;
            }
            if (menuLayer == 1 && swap)
            {
                playerAction.data = optionsMenu.battleActions[menuOption];
                if (BattleManager.Instance.player.MP >= playerAction.data.manaCost)
                {
                    OnPlaySound(1);
                    menuLayer = 2;
                    menuOption = 0;
                    swap = false;
                }
                else OnPlaySound(4);
            }
            if (menuLayer == 2 && swap)
            {
                if (BattleManager.Instance.player.MP >= playerAction.data.manaCost)
                {
                    fireLight.destination = fireLight.startingPosition;
                    OnPlaySound(1);
                    playerAction.target = BattleManager.Instance.activeEnemies[menuOption];
                    Invoke("PerformAction", actionDelay);
                    //PerformAction();
                    Instantiate(spellFX, BattleManager.Instance.player.spellTransform);
                    menuLayer = 0;
                    menuOption = 0;
                    updateMenus = false;
                    BattleManager.Instance.OnPlayerDecide(playerAction.data.effect.castDuration + 1);
                }
                else OnPlaySound(4);
            }
            //menuOption = 0;
        }
        else if (Input.GetKeyDown(KeyCode.X))
        {
            if (menuLayer != 0)OnPlaySound(2);
            menuLayer--;
            if (menuLayer < 0) menuLayer = 0;
            menuOption = 0;

            if (menuLayer == 0) BattleManager.Instance.player.animator.SetBool("StaffEquip", false);
        }
        if (Input.anyKeyDown && updateMenus)
        {
            switch (menuLayer)
            {
                case 0:
                    fireLight.destination = fireLight.startingPosition;
                    OnShowButtons();
                    OnSelectButton(menuOption, true);
                    OnShowOptions(false);
                    break;
                case 1:
                    fireLight.destination = fireLight.startingPosition;
                    OnShowButtons(false);
                    OnShowOptions();
                    optionsMenu.ChangeMenuList(menuLayer - 1);
                    OnHighlightOptions(menuOption);
                    break;
                case 2:
                    fireLight.destination = BattleManager.Instance.activeEnemies[menuOption].transform.position;
                    var spriteHeight = (BattleManager.Instance.activeEnemies[menuOption].GetComponent<SpriteRenderer>().bounds.size.y);
                    //Debug.Log(spriteHeight);
                    
                    fireLight.destination.y += spriteHeight;
                    fireLight.destination.y += spriteHeight * 0.1f;
                    if (spriteHeight < 0.5f) fireLight.destination.y += 1; 
                    OnShowButtons(false);
                    OnShowOptions();
                    optionsMenu.ChangeMenuList(menuLayer - 1);
                    OnHighlightOptions(menuOption);
                    break;
            }
        }
    }
    public void PerformAction()
    {
        //playerAction.Perform();
        var player = BattleManager.Instance.player;
        player.action = playerAction;
        BattleManager.Instance.player.OnDecide();
        //BattleManager.Instance.player.animator.SetBool("StaffEquip", false);
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
        //if (show) { Instantiate(menuSounds[3]); }
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
    public void OnPlaySound(int id)
    {
        Instantiate(menuSounds[id]);
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
        //if (GameManager.Instance.inBattle) Instantiate(menuSounds[4]);
        OnShowButtons(false);
        OnShowOptions(false);
        menuLayer = 0;
        menuOption = 0;
        active = false;
    }

    public void ActivateFireLight(bool active = true)
    {
        fireLight.gameObject.SetActive(active);
    }
}
