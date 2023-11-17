using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : Interactable
{
    public int sceneID;
    public override void OnInteract()
    {
        base.OnInteract();
        LoadingScreen.Instance.LoadScene(sceneID);
    }
}
