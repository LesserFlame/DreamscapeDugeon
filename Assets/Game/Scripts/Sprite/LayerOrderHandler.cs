using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class LayerOrderHandler : MonoBehaviour
{
    private SpriteRenderer sprite;
    [SerializeField] private bool isStatic = true;
    // Start is called before the first frame update
    void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
        sprite.sortingOrder = (int)(-transform.position.y * 10);
    }

    // Update is called once per frame
    void Update()
    {
        if (!isStatic) sprite.sortingOrder = (int)(-transform.position.y * 10);
    }
}
