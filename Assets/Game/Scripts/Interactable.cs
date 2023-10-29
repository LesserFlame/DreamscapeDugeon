using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Interactable : MonoBehaviour
{
    public virtual void OnInteract()
    {
        Debug.Log("interact");
    }
}
