using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreateButton : MonoBehaviour
{
    [SerializeField] bool phyisical;
    GameObject player;
    void Start()
    {
        string temp = gameObject.name;
        gameObject.GetComponent<Button>().onClick.AddListener(() => AttachCallback(temp));
        player = GameObject.FindGameObjectWithTag("Player");
    }

    private void AttachCallback(string btn)
    {

        if (btn.CompareTo("Melee") == 0)
        {
            player.GetComponent<Action>().SelectAttack("melee");
        }
        else if (btn.CompareTo("Spell") == 0)
        {
            player.GetComponent<Action>().SelectAttack("spell");
        }
        else if (btn.CompareTo("Flee") == 0)
        {
            //player.GetComponent<Action>().SelectAttack("flee");
            Debug.Log("Joestar Secret Technique!!!");
        }
    }
}
