using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Action : MonoBehaviour
{
    private GameObject enemy;
    private GameObject player;

    [SerializeField] GameObject meleePrefab;
    [SerializeField] GameObject spellPrefab;

    [SerializeField] Sprite faceIcon;

    private GameObject currentAttack;

    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");        
        enemy = GameObject.FindGameObjectWithTag("Enemy");  
    }
    public void SelectAttack(string btn)
    {
        GameObject target = player;
        if (tag == "Player") target = enemy;

        if (btn.CompareTo("melee") == 0) meleePrefab.GetComponent<ActionScript>().Attack(target);
        else if (btn.CompareTo("spell") == 0) spellPrefab.GetComponent<ActionScript>().Attack(target);
        else if (btn.CompareTo("flee") == 0) Debug.Log("Run");
    }
}
