using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CorridorFirstGenerator : SimpleWalkGenerator
{
    [SerializeField] private int corridorLength = 14, corridorCount = 5;
    [SerializeField, Range(0.1f, 1)] private float roomPercent = 0.8f;
    [SerializeField, Min(1)] private int corridorSize = 1;
    //[SerializeField] public SimpleWalkData roomGenerationParameters;
    protected override void RunProceduralGeneration()
    {
        CorridorFirstGeneration();
    }

    private void CorridorFirstGeneration()
    {
        HashSet<Vector2Int> floorPositions = new HashSet<Vector2Int>();
        HashSet<Vector2Int> potentialRoomPositions = new HashSet<Vector2Int>();

        //CreateCorridors(floorPositions, potentialRoomPositions);

        List<List<Vector2Int>> corridors = CreateCorridors(floorPositions, potentialRoomPositions);

        HashSet<Vector2Int> roomPostitions = CreateRooms(potentialRoomPositions);

        List<Vector2Int> deadEnds = FindAllDeadEnds(floorPositions);

        CreateRoomsAtDeadEnds(deadEnds, roomPostitions);

        floorPositions.UnionWith(roomPostitions);

        Debug.Log(corridors.Count);
        if (corridorSize >= 2)
        for (int i = 0; i < corridors.Count; i++)
        {
            //corridors[i] = IncreaseCorridorSizeByOne(corridors[i]);
            corridors[i] = IncreaseCorridorSize(corridors[i], corridorSize);
            floorPositions.UnionWith(corridors[i]);
        }

        tilemapVisualizer.PaintFloorTiles(floorPositions);
        WallGenerator.CreateWalls(floorPositions, tilemapVisualizer);
    }

    private List<Vector2Int> IncreaseCorridorSize(List<Vector2Int> corridor, int size)
    {
        List<Vector2Int> newCorridor = new List<Vector2Int>();
        //ew triple nested for loop
        if (size >= 2)
        {
            for (int i = 1; i < corridor.Count; i++) 
            {
                for (int x = -1; x < size - 1; x++)
                {
                    for (int y = -1; y < size - 1; y++) 
                    {
                        newCorridor.Add(corridor[i - 1] + new Vector2Int(x, y));
                    }
                }
            }
        }
        return newCorridor;
    }

    //private List<Vector2Int> IncreaseCorridorSizeByOne(List<Vector2Int> corridor)
    //{
        
    //    List<Vector2Int> newCorridor = new List<Vector2Int>();
    //    Vector2Int previousDirection = Vector2Int.zero;
    //    for (int i = 1; i < corridor.Count; i++) 
    //    { 
    //        Vector2Int directionFromCell = corridor[i] - corridor[i - 1];
    //        if(previousDirection != Vector2Int.zero && directionFromCell != previousDirection)
    //        {
    //            //corner handler
    //            for (int x = -1; x < 2; x++) 
    //            { 
    //                for (int y = -1; y < 2; y++) 
    //                {
    //                    newCorridor.Add(corridor[i - 1] + new Vector2Int(x, y));
    //                }
    //            }
    //            previousDirection = directionFromCell;
    //        }
    //        else
    //        {
    //            Vector2Int newCorridorTileOffset = GetDirection90From(directionFromCell);
    //            newCorridor.Add(corridor[i - 1]);
    //            newCorridor.Add(corridor[i - 1] + newCorridorTileOffset);
    //        }
    //    }
    //    return newCorridor;
    //}

    //private Vector2Int GetDirection90From(Vector2Int direction)
    //{
    //    if (direction == Vector2Int.up) return Vector2Int.right;
    //    if (direction == Vector2Int.right) return Vector2Int.down;
    //    if (direction == Vector2Int.down) return Vector2Int.left;
    //    if (direction == Vector2Int.left) return Vector2Int.up;
    //    return Vector2Int.zero;
    //}

    private void CreateRoomsAtDeadEnds(List<Vector2Int> deadEnds, HashSet<Vector2Int> roomFloorPositions)
    {
        foreach (var position in deadEnds)
        {
            if(roomFloorPositions.Contains(position) == false)
            {
                var room = RunRandomWalk(randomWalkParameters, position);
                roomFloorPositions.UnionWith(room);
            }
        }
    }

    private List<Vector2Int> FindAllDeadEnds(HashSet<Vector2Int> floorPositions)
    {
        List<Vector2Int> deadEnds = new List<Vector2Int>();
        foreach(var position in floorPositions)
        {
            int neighboursCount = 0;
            foreach(var direction in Direction2D.cardinalDirections) 
            { 
                if(floorPositions.Contains(position + direction)) neighboursCount++;
            }
            if (neighboursCount == 1) deadEnds.Add(position);
        }
        return deadEnds;
    }

    private HashSet<Vector2Int> CreateRooms(HashSet<Vector2Int> potentialRoomPositions)
    {
        HashSet<Vector2Int> roomPositions = new HashSet<Vector2Int>();
        int roomsToCreateCount = Mathf.RoundToInt(potentialRoomPositions.Count * roomPercent);

        List<Vector2Int> roomsToCreate = potentialRoomPositions.OrderBy(x => Guid.NewGuid()).Take(roomsToCreateCount).ToList(); //Guid is a very unique number that makes the order of the list random

        foreach (var roomPosition in roomsToCreate)
        {
            var roomFloor = RunRandomWalk(randomWalkParameters, roomPosition);
            roomPositions.UnionWith(roomFloor);
        }
        return roomPositions;
    }

    private List<List<Vector2Int>> CreateCorridors(HashSet<Vector2Int> floorPositions, HashSet<Vector2Int> potentialRoomPositions)
    {
        var currentPosition = startPosition;
        potentialRoomPositions.Add(currentPosition);
        List<List<Vector2Int>> corridors = new List<List<Vector2Int>>();
        for (int i = 0; i < corridorCount; i++) 
        {
            var corridor = ProceduralAlgorithms.RandomWalkCorridor(currentPosition, corridorLength);
            corridors.Add(corridor);
            currentPosition = corridor[corridor.Count - 1];
            potentialRoomPositions.Add(currentPosition);
            floorPositions.UnionWith(corridor);
        }
        return corridors;
    }
}
