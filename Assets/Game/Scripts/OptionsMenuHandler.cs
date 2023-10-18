using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class OptionsMenuHandler : MonoBehaviour
{
    public List<OptionsUI> optionsButtons;
    public List<BattleActionData> battleActions;
    //public List<EnemyActor> enemyActors;
    public Sprite testSprite;
    private int shift = 0;

    public void UpdateDisplayOptions(bool up = true)
    {
        //attempted to allow menu scrolling, works in one way but can't figure out how to do other way.
        //Might attempt to fix, but probably will just limit options to 5 for now.
        if (up) shift++;
        else shift--;

        //Debug.Log(shift);
        if (shift < 0) { shift = 0; }
        else if (shift > battleActions.Count - optionsButtons.Count) shift = battleActions.Count - optionsButtons.Count;
        //Debug.Log(shift);
        for (int i = 0; i < optionsButtons.Count; i++)
        {
            if (i < optionsButtons.Count && i < battleActions.Count)
            {
                optionsButtons[i].OnUpdateDisplay(testSprite, battleActions[i + shift].actionName);
            }
            else
            {
                optionsButtons[i].OnUpdateDisplay(null, null);
            }
        }
    }
    public void ChangeMenuList(int id)
    {
        foreach(var button in optionsButtons) button.OnUpdateDisplay(null, null);
        switch (id) 
        {
            case 0:
                //GetPlayerActions();
                for (int i = 0; i < optionsButtons.Count; i++)
                {
                    if (i < optionsButtons.Count && i < battleActions.Count)
                    {
                        optionsButtons[i].OnUpdateDisplay(testSprite, battleActions[i].actionName);
                    }
                    else
                    {
                        optionsButtons[i].OnUpdateDisplay(null, null);
                    }
                }
                break;
            case 1:
                for (int i = 0; i < optionsButtons.Count; i++)
                {
                    if (i < optionsButtons.Count && i < BattleManager.Instance.activeEnemies.Count)
                    {
                        optionsButtons[i].OnUpdateDisplay(testSprite, BattleManager.Instance.activeEnemies[i].actorName);
                    }
                    else
                    {
                        optionsButtons[i].OnUpdateDisplay(null, null);
                    }
                }
                break;
        }
    }
    public void GetPlayerActions()
    {
        battleActions.Clear();
        foreach(var action in BattleManager.Instance.player.actions)
        {
            battleActions.Add(action);
        }
    }
    
}
