using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(EnemyDetection))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(SpriteRenderer))]
public class EnemyMovement : MonoBehaviour
{
    private EnemyDetection detection;
    private Rigidbody2D rb;
    private SpriteRenderer renderer;
    [SerializeField] private float chaseForce = 1.0f;
    private void Start()
    {
        detection = GetComponent<EnemyDetection>();
        rb = GetComponent<Rigidbody2D>();
        renderer = GetComponent<SpriteRenderer>();
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        if(detection.playerDetected)
        {
            renderer.enabled = true;
            rb.AddForce(detection.targetDirection * chaseForce);
        }
        else renderer.enabled = false;
    }
}
