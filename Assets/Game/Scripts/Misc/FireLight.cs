using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireLight : MonoBehaviour
{
    public Vector3 destination;
    public Vector3 startingPosition;
    public float speed = 0.1f;

    private void Start()
    {
        startingPosition = transform.position;
        destination = transform.position;
    }
    void FixedUpdate()
    {
        Vector3 newPosition = Vector3.zero;
        newPosition.x = Mathf.Lerp(transform.position.x, destination.x, speed);
        newPosition.y = Mathf.Lerp(transform.position.y, destination.y, speed);
        //Mathf.Lerp(transform.localPosition.x, destination.x, 5);
        transform.position = newPosition;
        
    }

    //public void UpdateDestination(Vector3 newDestination)
    //{
    //    if (newDestination != destination) { destination = newDestination; }
    //}
}
