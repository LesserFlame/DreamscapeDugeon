using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UIElements;

public class AnimatedLight : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private float minSpeed;
    [SerializeField] private float maxSpeed;
    //private AnimatedTile animatedTile;
    //private float speed = 0;
    //public void SetSpeed(float animationSpeed)
    //{
    //    //speed = animationSpeed;
    //    animator.speed = animationSpeed;
    //}
    private void Awake()
    {
        //TileBase tile = tilemap.GetTile(transform.position);
        //tile = 
        animator.speed = Random.Range(minSpeed, maxSpeed);
    }
}
