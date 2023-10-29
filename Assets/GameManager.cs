using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    [SerializeField] private Camera mainCamera, battleCamera;
    [SerializeField] public PlayerController player;

    [SerializeField] public bool inBattle = false;


    private void Start()
    {
        mainCamera.gameObject.SetActive(!inBattle);
        battleCamera.gameObject.SetActive(inBattle);
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
        BattleTransition();
    }
    public void BattleTransition()
    {
        if (mainCamera == null) mainCamera = Camera.main;
        inBattle = !inBattle;
        player.gameObject.SetActive(!inBattle);
        //swap cameras
        mainCamera.gameObject.SetActive(!inBattle);
        battleCamera.gameObject.SetActive(inBattle);
    }
}
