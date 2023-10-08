using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;
using Random = UnityEngine.Random;

public class PrefabRoomGenerator : SimpleWalkGenerator
{
    [SerializeField] private int minRoomWidth = 4, minRoomHeight = 4;
    [SerializeField] private int dungeonWidth = 20, dungeonHeight = 20;
    [SerializeField, Range(0, 10)] private int offset = 1;
    //[SerializeField] bool randomWalkRooms = false;
    //[SerializeField] private RoomsList roomsListInfo;
    [SerializeField] private List<RoomInfo> roomsListInfo;
    [SerializeField] private List<RoomInfo> placedRooms;

    protected override void RunProceduralGeneration()
    {
        CreateRooms();
    }

    private void CreateRooms()
    {
        tilemapVisualizer.Clear();
        var roomsBoundsList = ProceduralAlgorithms.BinarySpacePartioning(new BoundsInt((Vector3Int)startPosition, new Vector3Int(dungeonWidth, dungeonHeight, 0)), minRoomWidth, minRoomHeight);
        
        CreatePrefabRooms(roomsBoundsList);
        //HashSet<Vector2Int> floor = CreateSimpleRooms(roomsList);

        //if (randomWalkRooms) floor = CreateWalkRooms(roomsList);
        //else floor = CreateSimpleRooms(roomsList);

        List<List<Vector2Int>> doorways = new List<List<Vector2Int>>();
        foreach (var room in placedRooms)
        {
            doorways.Add(room.doorPositions);    
        }
        Debug.Log(doorways[0].Count);
        HashSet<Vector2Int> corridors = ConnectRooms(doorways);
        //floor.UnionWith(corridors);

        tilemapVisualizer.PaintFloorTiles(corridors);
        WallGenerator.CreateWalls(corridors, tilemapVisualizer);
        //Debug.Log("Dungeon Generated");

    }

    private void CreatePrefabRooms(List<BoundsInt> roomsBoundsList)
    {
        if (placedRooms.Count > 0) placedRooms.Clear();
        for (int i = 0; i < roomsBoundsList.Count; i++)
        {
            
            bool found = false;
            RoomInfo potentialRoom = null;
            while (!found)
            {
                potentialRoom = roomsListInfo[Random.Range(0, roomsListInfo.Count)];
                if (potentialRoom.roomSize.x < roomsBoundsList[i].size.x - offset && potentialRoom.roomSize.y < roomsBoundsList[i].size.y - offset)
                {
                    found = true;
                }
            }
            PlacePrefabRoom(potentialRoom, new Vector2Int((int)roomsBoundsList[i].position.x, (int)roomsBoundsList[i].position.y));
            //roomsBoundsList.Remove(roomsBoundsList[i]);
        }
    }
    private void PlacePrefabRoom(RoomInfo room, Vector2Int position)
    {
        placedRooms.Add(room);
        tilemapVisualizer.PaintRoom(room, position);
    }

    //private HashSet<Vector2Int> CreateWalkRooms(List<BoundsInt> roomsList)
    //{
    //    HashSet<Vector2Int> floor = new HashSet<Vector2Int>();
    //    for (int i = 0; i < roomsList.Count; i++)
    //    {
    //        var roomBounds = roomsList[i];
    //        var roomCenter = new Vector2Int(Mathf.RoundToInt(roomBounds.center.x), Mathf.RoundToInt(roomBounds.center.y));
    //        var roomFloor = RunRandomWalk(randomWalkParameters, roomCenter);
    //        foreach (var position in roomFloor)
    //        {
    //            if (position.x >= (roomBounds.xMin + offset) && position.x <= (roomBounds.xMax - offset) && position.y >= (roomBounds.yMin - offset) && position.y <= (roomBounds.yMax - offset))
    //            {
    //                floor.Add(position);
    //            }
    //        }
    //        //floor.UnionWith(roomFloor);
    //    }
    //    return floor;
    //}

    private HashSet<Vector2Int> ConnectRooms(List<List<Vector2Int>> doorways)
    {
        HashSet<Vector2Int> corridors = new HashSet<Vector2Int>();
        int roomID = Random.Range(0, doorways.Count);
        //var currentRoomDoorways = doorways[roomID];
        int doorID = Random.Range(0, doorways[roomID].Count);
        var currentDoorway = doorways[roomID][doorID];
        doorways[roomID].Remove(currentDoorway);

        //if (doorways[roomID].Count > 0) doorways.Remove(doorways[roomID]);

        while (doorways.Count > 0)
        {
            List<Vector2Int> otherDoorways = new List<Vector2Int>();
            foreach (var room in doorways)
            {
                foreach (var doorway in room)
                {
                    if (!doorways[roomID].Contains(doorway)) otherDoorways.Add(doorway);
                }
            }
            
            Vector2Int closest = FindClosestRoomTo(currentDoorway, otherDoorways);

            int otherRoomID = 0;
            foreach (var room in doorways)
            {
                if (room.Contains(closest)) continue;
                otherRoomID++;
            }

            doorways[otherRoomID].Remove(closest);
            if (doorways[otherRoomID].Count > 0) doorways.RemoveAt(otherRoomID);
            HashSet<Vector2Int> newCorridor = CreateCorridor(currentDoorway, closest);
            currentDoorway = doorways[otherRoomID][0];
            corridors.UnionWith(newCorridor);
            if (doorways[roomID].Count > 0) doorways.RemoveAt(roomID);
        }
        return corridors;
    }

    private HashSet<Vector2Int> CreateCorridor(Vector2Int currentRoomCenter, Vector2Int destination)
    {
        HashSet<Vector2Int> corridor = new HashSet<Vector2Int>();
        var position = currentRoomCenter;
        corridor.Add(position);
        while (position.y != destination.y)
        {
            if (destination.y > currentRoomCenter.y) position += Vector2Int.up;
            else position += Vector2Int.down;
            corridor.Add(position);
        }
        while (position.x != destination.x)
        {
            if (destination.x > currentRoomCenter.x) position += Vector2Int.right;
            else position += Vector2Int.left;
            corridor.Add(position);
        }
        Debug.Log(corridor.Count);
        return corridor;
    }

    private Vector2Int FindClosestRoomTo(Vector2Int currentRoomCenter, List<Vector2Int> roomCenters)
    {
        Vector2Int closest = Vector2Int.zero;
        float distance = float.MaxValue;
        foreach (var position in roomCenters)
        {
            float currentDistance = Vector2Int.Distance(position, currentRoomCenter);
            if (currentDistance < distance)
            {
                distance = currentDistance;
                closest = position;
            }
        }
        return closest;
    }

    //private HashSet<Vector2Int> CreateSimpleRooms(List<BoundsInt> roomsList)
    //{
    //    HashSet<Vector2Int> floor = new HashSet<Vector2Int>();
    //    foreach (var room in roomsList)
    //    {
    //        for (int col = offset; col < room.size.x - offset; col++)
    //        {
    //            for (int row = offset; row < room.size.y - offset; row++)
    //            {
    //                Vector2Int position = (Vector2Int)room.min + new Vector2Int(col, row);
    //                floor.Add(position);
    //            }
    //        }
    //    }
    //    return floor;
    //}
}
