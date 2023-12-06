using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StatsScreenHandler : MonoBehaviour
{
    private bool isOpen = false;

    [SerializeField] private SliderTextHandler xpSlider;
    [SerializeField] private SliderTextHandler hpSlider;
    [SerializeField] private SliderTextHandler fpSlider;
    [SerializeField] private TMP_Text atkText;
    [SerializeField] private TMP_Text defText;
    [SerializeField] private TMP_Text spdText;
    [SerializeField] private TMP_Text lvlText;

    [SerializeField] private Animator animator;

    [SerializeField] private List<GameObject> menuSounds;

    //private bool prevOpen = false;
    
    void Update()
    {
        if (isOpen)
        {
            if (Input.GetKeyDown(KeyCode.X))
            {
                //Invoke("CloseStatsMenu", 0.1f);
                CloseStatsMenu();
            }
            if (Input.GetKeyDown(KeyCode.E))
            {
                CloseStatsMenu();
                LoadingScreen.Instance.LoadScene(1);
                BattleManager.Instance.ResetBattle();
                BattleManager.Instance.player.initialized = false;
            }
        }
    }
    public void CloseStatsMenu()
    {
        if (menuSounds != null) { Instantiate(menuSounds[4]); }
        isOpen = false;
        FindAnyObjectByType<PlayerController>().detectInput = true;
        animator.SetBool("IsOpen", false);

        Time.timeScale = 1;
    }
    public void OpenStatsMenu()
    {
        if (menuSounds != null) { Instantiate(menuSounds[3]); }
        isOpen = true;
        var player = FindAnyObjectByType<PlayerController>();
        BattleManager.Instance.player.OnInitialize(player);
        lvlText.text = "Level: " + player.level;
        atkText.text = "ATK: " + player.ATK;
        defText.text = "DEF: " + player.DEF;
        spdText.text = "SPD: " + player.SP;

        hpSlider.SetValue(BattleManager.Instance.player.HP, player.HP);
        fpSlider.SetValue(BattleManager.Instance.player.MP, player.MP);
        xpSlider.SetValue(player.XP, player.requiredXP);

        animator.SetBool("IsOpen", true);

        //if (prevOpen) Invoke("PauseGame", animator.GetCurrentAnimatorClipInfo(0).Length);
        Invoke("PauseGame", 0.6f);
        //prevOpen = true;
    }

    private void PauseGame()
    {
        if (isOpen) Time.timeScale = 0;
    }
}
