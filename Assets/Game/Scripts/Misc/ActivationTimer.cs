using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivationTimer : MonoBehaviour
{
    [SerializeField] private GameObject objectToActivate;
    [SerializeField] private float timer;
    void Start()
    {
        if (objectToActivate != null) { Invoke("ActivateObject", timer); }
    }

    private void ActivateObject()
    {
        objectToActivate.SetActive(true);
    }
}
