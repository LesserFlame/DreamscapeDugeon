using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[System.Serializable]
public class RoomInfo : MonoBehaviour
{
    public List<Doorway> doorways;
    //public Vector2Int roomCenter = Vector2Int.zero;
    public BoundsInt roomSize;
    public Tilemap floorTilemap, wallTilemap, overlayTilemap;

    //public List<EnemyInfo> enemies;
    public GameObject prefabContainer;
}
