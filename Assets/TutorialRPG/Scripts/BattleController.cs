using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BattleController : MonoBehaviour
{
    private List<ActorStats> actorStats;

    [SerializeField] private GameObject battleMenu;
    [SerializeField] public TMP_Text damageText;

    void Start()
    {
        actorStats = new List<ActorStats>();
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        ActorStats currentActorStats = player.GetComponent<ActorStats>();
        currentActorStats.CalculateNextTurn(0);
        actorStats.Add(currentActorStats);

        GameObject enemy = GameObject.FindGameObjectWithTag("Enemy");
        ActorStats currentEnemyStats = enemy.GetComponent<ActorStats>();
        currentEnemyStats.CalculateNextTurn(0);
        actorStats.Add(currentEnemyStats);

        actorStats.Sort();
        this.battleMenu.SetActive(false);
        //Debug.Log(battleMenu.active.ToString());

        NextTurn();
    }

    public void NextTurn()
    {
        damageText.gameObject.SetActive(false);

        ActorStats currentActorStats = actorStats[0];
        actorStats.Remove(currentActorStats);
        if (!currentActorStats.GetDead())
        {
            GameObject currentActor = currentActorStats.gameObject;
            currentActorStats.CalculateNextTurn(currentActorStats.nextActorTurn);
            actorStats.Add(currentActorStats);
            actorStats.Sort();
            if (currentActor.tag == "Player")
            {
                battleMenu.SetActive(true);
                //Debug.Log("Player Turn");
            }
            else
            {
                battleMenu.SetActive(false);
                string attackType = Random.Range(0, 2) == 1 ? "melee" : "spell";
                currentActor.GetComponent<Action>().SelectAttack(attackType);
                //Debug.Log("Enemy Turn " + attackType);
            }
        }
        else
        {
            NextTurn();
        }
    }
}
