using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]//, RequireComponent(typeof(TMP_Text))]
public class FloatingMessage : MonoBehaviour
{
    private Rigidbody2D rb;
    private TMP_Text messageText;

    public float initalYVelocity = 7f;
    public float initalXVelocityRange = 6f;
    public float lifeTime = 0.8f;
    public bool moveLeft = true;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        messageText = GetComponentInChildren<TMP_Text>();
    }

    private void Start()
    {
        if (moveLeft) rb.velocity = new Vector2(Random.Range(-initalXVelocityRange, -2), initalYVelocity);
        else rb.velocity = new Vector2(Random.Range(2, initalXVelocityRange), initalYVelocity);
        Invoke("Freeze", lifeTime * 0.5f);
        Destroy(gameObject, lifeTime);
    }
    private void Freeze()
    {
        rb.gravityScale = 0;
        rb.velocity= Vector3.zero;
    }
    public void SetMessage(string msg)
    {
        messageText.SetText(msg);
    }
}
