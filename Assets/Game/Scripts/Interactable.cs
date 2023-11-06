using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Interactable : MonoBehaviour
{
    public virtual void OnInteract()
    {
        if(TryGetComponent<DialogueTrigger>(out DialogueTrigger trigger))
        {
            Invoke("OnTriggerDialogue", 0.1f);
        }
    }
    public void OnTriggerDialogue()
    {
        if (TryGetComponent<DialogueTrigger>(out DialogueTrigger trigger)) trigger.TriggerDialogue();
    }
}
