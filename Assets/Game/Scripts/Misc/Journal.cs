using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Journal : Interactable
{
    public override void OnInteract()
    {
       base.OnInteract();
       var skilltree = FindFirstObjectByType<SkillTree>();
       skilltree.OpenJournal();
       skilltree.SetAttentionGrabActive(false);
    }
}
