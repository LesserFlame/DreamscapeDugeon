using NUnit.Framework.Constraints;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Doorway : MonoBehaviour
{
    [HideInInspector] public Vector2 directionVector = Vector2.zero;
    public enum Direction
    {
        NORTH,
        EAST, 
        SOUTH,
        WEST
    }
    [SerializeField] public Direction direction = Direction.NORTH;
    [SerializeField] public Direction partnerDirection = Direction.SOUTH;

    private void Start()
    {
        int id = (int)direction;
        directionVector = Direction2D.cardinalDirections[id];

        //switch (direction)
        //{
        //    case Direction.NORTH: partnerDirection = Direction.SOUTH; break;
        //    case Direction.EAST: partnerDirection = Direction.WEST; break;
        //    case Direction.SOUTH: partnerDirection = Direction.NORTH; break;
        //    case Direction.WEST: partnerDirection = Direction.EAST; break;   
        //}
    }
}
