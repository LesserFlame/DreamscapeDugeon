using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;
using Random = UnityEngine.Random;

public class PrefabRoomGenerator : SimpleWalkGenerator
{
    //[SerializeField] private int minRoomWidth = 4, minRoomHeight = 4;
    //[SerializeField] private int dungeonWidth = 20, dungeonHeight = 20;
    //[SerializeField, Range(0, 10)] private int offset = 1;
    //[SerializeField] bool randomWalkRooms = false;
    //[SerializeField] private RoomsList roomsListInfo;
    [SerializeField] private RoomsList roomsListInfo;
    [SerializeField] private List<RoomInfo> placedRooms;
    [SerializeField] private List<RoomInfo> doorwayCaps;
    [SerializeField] private List<Doorway> doorways;
    [SerializeField] private List<GameObject> prefabContainers;

    protected override void RunProceduralGeneration()
    {
        CreateRooms();
    }

    protected override void DeleteDungeon()
    {
        base.DeleteDungeon();
        foreach (var doorway in doorways)
        {
            if (doorway != null) DestroyImmediate(doorway.gameObject);
        }
        foreach (var container in prefabContainers)
        {
            if (container != null) DestroyImmediate(container);
        }
        doorways.Clear();
        prefabContainers.Clear();
        placedRooms.Clear();
    }

    private void CreateRooms()
    {
        foreach(var doorway in doorways) 
        { 
            if (doorway != null) DestroyImmediate(doorway.gameObject);
        }
        foreach (var container in prefabContainers)
        {
            if (container != null) DestroyImmediate(container);
        }
        doorways.Clear();
        prefabContainers.Clear();
        placedRooms.Clear();
        //roomsListInfo.Clear();
        tilemapVisualizer.Clear();
        //var roomsBoundsList = ProceduralAlgorithms.BinarySpacePartioning(new BoundsInt((Vector3Int)startPosition, new Vector3Int(dungeonWidth, dungeonHeight, 0)), minRoomWidth, minRoomHeight);
        
        CreatePrefabRooms();
        DeleteDuplicateDoorways();
        CapDoorways();
        //HashSet<Vector2Int> floor = CreateSimpleRooms(roomsList);

        //if (randomWalkRooms) floor = CreateWalkRooms(roomsList);
        //else floor = CreateSimpleRooms(roomsList);

        //List<List<Vector2Int>> doorways = new List<List<Vector2Int>>();
        //foreach (var room in placedRooms)
        //{
        //    //doorways.Add(room.doorPositions);    
        //}
        //Debug.Log(doorways[0].Count);
        //HashSet<Vector2Int> corridors = ConnectRooms(doorways);
        //floor.UnionWith(corridors);

        //tilemapVisualizer.PaintFloorTiles(corridors);
        //WallGenerator.CreateWalls(corridors, tilemapVisualizer);
        //Debug.Log("Dungeon Generated");

    }

    //private void CreatePrefabRooms(List<BoundsInt> roomsBoundsList)
    //{
    //    if (placedRooms.Count > 0) placedRooms.Clear();
    //    for (int i = 0; i < roomsBoundsList.Count; i++)
    //    {
    //        bool found = false;
    //        RoomInfo potentialRoom = null;
    //        while (!found)
    //        {
    //            potentialRoom = roomsListInfo.starterRooms[Random.Range(0, roomsListInfo.starterRooms.Count)];
    //            if (potentialRoom.roomSize.x < roomsBoundsList[i].size.x - offset && potentialRoom.roomSize.y < roomsBoundsList[i].size.y - offset)
    //            {
    //                found = true;
    //            }
    //        }
    //        PlacePrefabRoom(potentialRoom, new Vector2Int((int)roomsBoundsList[i].position.x, (int)roomsBoundsList[i].position.y));
    //        //roomsBoundsList.Remove(roomsBoundsList[i]);
    //    }
    //}
    private void CapDoorways()
    {
        foreach (var door in doorways)
        {
            int id = 0;
            switch (door.direction)
            {
                case Doorway.Direction.NORTH:
                    id = 0;
                    break;
                case Doorway.Direction.EAST:
                    id = 1;
                    break;
                case Doorway.Direction.SOUTH:
                    id = 2;
                    break;
                case Doorway.Direction.WEST:
                    id = 3;
                    break;
            }
            Vector2Int doorGridPosition = tilemapVisualizer.GetGridPosition(door.transform.position);
            tilemapVisualizer.PaintRoom(doorwayCaps[id], doorGridPosition, false);
        }
    }
    private void DeleteDuplicateDoorways()
    {
        for (int i = 0; i < doorways.Count; i++)
        {
            for (int j = 0; j < doorways.Count; j++)
            {
                if (i != j && doorways[i].transform.position == doorways[j].transform.position)
                {
                    var doorOne = doorways[i];
                    var doorTwo = doorways[j];
                    doorways.Remove(doorOne);
                    doorways.Remove(doorTwo);
                    DestroyImmediate(doorOne.gameObject);
                    DestroyImmediate(doorTwo.gameObject);
                    if (i > 0) i--;
                    if (j > 0) j--;
                }
            }
        }
    }
    private void CreatePrefabRooms()
    {
        PlacePrefabRoom(roomsListInfo.starterRooms[Random.Range(0, roomsListInfo.starterRooms.Count)], Vector2Int.zero);
        for (int i = 0; i < roomsListInfo.basicRoomAmount; i++)
        {
            bool found = false;
            Doorway potentialConnection = null;
            RoomInfo potentialRoom = null;
            Doorway potentialDoor = null;

            //potentialDoor = doorways[Random.Range(0, doorways.Count)];
            //potentialRoom = roomsListInfo.basicRooms[Random.Range(0, roomsListInfo.basicRooms.Count)];
            ////potentialConnection = potentialRoom.doorways[Random.Range(0, potentialRoom.doorways.Count)];
            //foreach (var door in potentialRoom.doorways) 
            //{ 
            //    if (potentialDoor.direction == door.partnerDirection) { potentialConnection = door; } continue;
            //}
            //Debug.Log(potentialDoor.name + " - " + potentialRoom.name + " - " + potentialConnection.name);
            while (!found)
            {
                potentialDoor = doorways[Random.Range(0, doorways.Count)];
                potentialRoom = roomsListInfo.basicRooms[Random.Range(0, roomsListInfo.basicRooms.Count)];
                foreach (var door in potentialRoom.doorways)
                {
                    if (potentialDoor.direction == door.partnerDirection) potentialConnection = door; continue;
                }
                if (potentialConnection != null) { found = true; }
            }
            if (potentialRoom != null)
            {
                //Debug.Log(potentialDoor.name + " - " + potentialRoom.name + " - " + potentialConnection.name);
                Vector2Int position = tilemapVisualizer.GetGridPosition(potentialDoor.transform.position);
                
                if (PlacePrefabRoom(potentialRoom, position, potentialDoor))
                {
                    doorways.Remove(potentialDoor);
                    DestroyImmediate(potentialDoor.gameObject);
                }
                else i--;
            }
        }
    }
    private bool PlacePrefabRoom(RoomInfo room, Vector2Int position, Doorway door = null)
    {
        if (door != null)
        {
            foreach (var doorway in room.doorways)
            {
                if (door != null && door.direction == doorway.partnerDirection)
                {
                    position -= tilemapVisualizer.GetGridPosition(doorway.transform.position);
                }
            }
        }
        if (CheckRoomAvailable(room, position))
        {
            placedRooms.Add(room);
            foreach (var doorway in room.doorways)
            {
                var positionOffset = tilemapVisualizer.GetWorldPosition(position);
                positionOffset += doorway.transform.position;
                var placedDoorway = Instantiate(doorway, this.transform);
                placedDoorway.transform.position = positionOffset;
                doorways.Add(placedDoorway);
                if (door != null && door.partnerDirection == doorway.direction)
                {
                    doorways.Remove(placedDoorway);
                    DestroyImmediate(placedDoorway.gameObject);
                }
            }
            if (room.prefabContainer != null)
            {
                var prefabContainer = Instantiate(room.prefabContainer, transform);
                prefabContainer.transform.position = tilemapVisualizer.GetWorldPosition(position);
                prefabContainers.Add(prefabContainer);
            }
            //Debug.Log(position);
            tilemapVisualizer.PaintRoom(room, position);
            return true;
        }
        else return false;
    }

    private bool CheckRoomAvailable(RoomInfo room, Vector2Int position)
    {
        return tilemapVisualizer.CheckEmpty(room, position);
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

    //private HashSet<Vector2Int> ConnectRooms(List<List<Vector2Int>> doorways)
    //{
    //    HashSet<Vector2Int> corridors = new HashSet<Vector2Int>();
    //    int roomID = Random.Range(0, doorways.Count);
    //    var currentRoomDoorways = doorways[roomID];
    //    int doorID = Random.Range(0, doorways[roomID].Count);
    //    var currentDoorway = doorways[roomID][doorID];
    //    doorways[roomID].Remove(currentDoorway);

    //    if (doorways[roomID].Count > 0) doorways.Remove(doorways[roomID]);

    //    while (doorways.Count > 0)
    //    {
    //        List<Vector2Int> otherDoorways = new List<Vector2Int>();
    //        foreach (var room in doorways)
    //        {
    //            foreach (var doorway in room)
    //            {
    //                if (!doorways[roomID].Contains(doorway)) otherDoorways.Add(doorway);
    //            }
    //        }
            
    //        Vector2Int closest = FindClosestRoomTo(currentDoorway, otherDoorways);

    //        int otherRoomID = 0;
    //        foreach (var room in doorways)
    //        {
    //            if (room.Contains(closest)) continue;
    //            otherRoomID++;
    //        }

    //        doorways[otherRoomID].Remove(closest);
    //        if (doorways[otherRoomID].Count > 0) doorways.RemoveAt(otherRoomID);
    //        HashSet<Vector2Int> newCorridor = CreateCorridor(currentDoorway, closest);
    //        currentDoorway = doorways[otherRoomID][0];
    //        corridors.UnionWith(newCorridor);
    //        if (doorways[roomID].Count > 0) doorways.RemoveAt(roomID);
    //    }
    //    return corridors;
    //}

    //private HashSet<Vector2Int> CreateCorridor(Vector2Int currentRoomCenter, Vector2Int destination)
    //{
    //    HashSet<Vector2Int> corridor = new HashSet<Vector2Int>();
    //    var position = currentRoomCenter;
    //    corridor.Add(position);
    //    while (position.y != destination.y)
    //    {
    //        if (destination.y > currentRoomCenter.y) position += Vector2Int.up;
    //        else position += Vector2Int.down;
    //        corridor.Add(position);
    //    }
    //    while (position.x != destination.x)
    //    {
    //        if (destination.x > currentRoomCenter.x) position += Vector2Int.right;
    //        else position += Vector2Int.left;
    //        corridor.Add(position);
    //    }
    //    Debug.Log(corridor.Count);
    //    return corridor;
    //}

    //private Vector2Int FindClosestRoomTo(Vector2Int currentRoomCenter, List<Vector2Int> roomCenters)
    //{
    //    Vector2Int closest = Vector2Int.zero;
    //    float distance = float.MaxValue;
    //    foreach (var position in roomCenters)
    //    {
    //        float currentDistance = Vector2Int.Distance(position, currentRoomCenter);
    //        if (currentDistance < distance)
    //        {
    //            distance = currentDistance;
    //            closest = position;
    //        }
    //    }
    //    return closest;
    //}

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
