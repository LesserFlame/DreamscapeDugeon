using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Bed : Interactable
{
    public override void OnInteract()
    {
        base.OnInteract();
        SceneManager.LoadScene("ProceduralTest");
    }
}
