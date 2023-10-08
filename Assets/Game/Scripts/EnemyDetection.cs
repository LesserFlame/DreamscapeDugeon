using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyDetection : MonoBehaviour
{
    public bool playerDetected { get; private set; }
    public Vector2 targetDirection => target.transform.position - detectionTransform.position;

    [SerializeField] private string playerTag = "Player";
    [SerializeField] private Transform detectionTransform;
    [SerializeField] private float detectionRadius = 5.0f;

    public float detectionRate = 0.5f;
    public LayerMask dectectLayerMask;
    public LayerMask obstacleLayerMask;

    private GameObject target;

    public GameObject Target
    {
        get => target;
        private set
        {
            target = value;
            playerDetected = target != null; //if target is given value then player detected
        }
    }

    void Start()
    {
        StartCoroutine(DetectionCoroutine());
    }

    
    IEnumerator DetectionCoroutine()
    {
        yield return new WaitForSeconds(detectionRate);
        DetectPlayer();
        StartCoroutine(DetectionCoroutine());
    }

    public void DetectPlayer()
    {
        Collider2D collider = Physics2D.OverlapCircle(detectionTransform.position, detectionRadius, dectectLayerMask);
        if (collider != null && collider.tag == playerTag)
        {
            RaycastHit2D ray = Physics2D.Raycast(detectionTransform.position, collider.transform.position - detectionTransform.position, detectionRadius, obstacleLayerMask);
            if (!ray) Target = collider.gameObject;
        }
        else Target = null;
        //if (target != null)Debug.Log(target.name);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //SceneManager.LoadScene(0); //Load battle scene OR maybe enable battle overlay to stay in scene.
    }
    //private void OnDrawGizmos()
    //{
    //    if (detectionTransform != null) 
    //    { 
    //        Gizmos.color = Color.green;
    //        if (playerDetected) Gizmos.color = Color.red;
    //        Gizmos.DrawWireSphere(detectionTransform.position, detectionRadius);
    //    }
    //}
}