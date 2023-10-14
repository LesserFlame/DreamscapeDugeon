using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    [SerializeField] private Camera mainCamera, battleCamera;
    [SerializeField] private PlayerController player;

    [SerializeField] public bool inBattle = false;


    private void Start()
    {
        mainCamera.gameObject.SetActive(!inBattle);
        battleCamera.gameObject.SetActive(inBattle);
    }

    public void BattleTransition(EnemyInfo instagator)
    {
        List<EnemyData> tempData = new List<EnemyData>();
        foreach (var enemy in instagator.roomInfo.enemies)
        {
            tempData.Add(enemy.data);
            Destroy(enemy.gameObject);
        }
        BattleManager.Instance.OnStartBattle(tempData);
        BattleTransition();
    }
    public void BattleTransition()
    {
        inBattle = !inBattle;
        player.gameObject.SetActive(!inBattle);
        //swap cameras
        mainCamera.gameObject.SetActive(!inBattle);
        battleCamera.gameObject.SetActive(inBattle);
    }
}
