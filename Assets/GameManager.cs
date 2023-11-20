using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    [SerializeField] private Camera mainCamera, battleCamera;
    [SerializeField] public PlayerController player;
    [SerializeField] public PlayerData playerData;

    [SerializeField] public bool inBattle = false;
    //[SerializeField] public bool inDungeon = false;
    [SerializeField] public BattleActionData[] battleActionDatas;
    [SerializeField] private TransitionScreen transitionScreen;
    [SerializeField] public MusicPlayer dungeonMusic;
    private void Start()
    {
        if (mainCamera != null) mainCamera.gameObject.SetActive(!inBattle);
        if (battleCamera != null) battleCamera.gameObject.SetActive(inBattle);
        playerData = SaveSystem.LoadPlayer();
        if (transitionScreen == null) transitionScreen = FindAnyObjectByType<TransitionScreen>();
        //if (transitionScreen == null) transitionScreen = Instantiate(new TransitionScreen(), transform);
    }
    
    public void BattleTransition(EnemyInfo instagator)
    {

        List<EnemyData> tempData = new List<EnemyData>();
        var room = instagator.GetComponentInParent<PrefabContainer>();
        foreach (var enemy in room.GetComponentsInChildren<EnemyInfo>())
        {
            tempData.Add(enemy.data);
            Destroy(enemy.gameObject);
        }
        BattleManager.Instance.OnStartBattle(tempData);

        //Invoke("BattleTransition", 1);
        //TransitionScreen();
        BattleTransition();
    }
    public void BattleTransition()
    {
        if (transitionScreen == null) transitionScreen = FindAnyObjectByType<TransitionScreen>();
        if (mainCamera == null) mainCamera = Camera.main;
        inBattle = !inBattle;
        player.detectInput = !inBattle;
        //swap cameras
        //if (!died)
        {
            TransitionScreen();
            Invoke("SwapCameras", 1);
            Invoke("TogglePlayer", 1);
        }
        //else Invoke("ResetCameras", 1);
        //playerData = new PlayerData(player);
        //Debug.Log(player.LVL);
        SaveSystem.SavePlayer(player);
        //Debug.Log("Exit");
        if (!inBattle) dungeonMusic.PlaySong();
    }
    public void TransitionScreen()
    {
        if (transitionScreen == null) transitionScreen = FindAnyObjectByType<TransitionScreen>();
        transitionScreen.InvokeToggle();
    }
    public void TogglePlayer()
    {
        player.gameObject.SetActive(!inBattle);
    }
    public void SwapCameras()
    {
        mainCamera.gameObject.SetActive(!inBattle);
        battleCamera.gameObject.SetActive(inBattle);
    }
    public void ResetCameras()
    {
        if (battleCamera != null) battleCamera.gameObject.SetActive(false);
        //mainCamera.gameObject.SetActive(true);
    }
}
