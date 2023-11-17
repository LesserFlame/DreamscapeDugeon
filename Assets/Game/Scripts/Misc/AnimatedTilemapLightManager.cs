using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class AnimatedTilemapLight : MonoBehaviour
{
    [SerializeField] private Tilemap tilemap;
    [SerializeField] private TileBase tileLight;
    [SerializeField] private AnimatedLight animatedLight;
    //public void SpawnLights()
    //{
    //    BoundsInt bounds = tilemap.cellBounds;

    //    foreach (Vector3Int position in bounds.allPositionsWithin)
    //    {
    //        TileBase tile = tilemap.GetTile(position);

    //        if (tile != null)
    //        {
    //            if (tileLight == tile) 
    //            { 
    //                Instantiate(animatedLight, position, Quaternion.identity);
    //                //animatedLight.SetSpeed();
    //            }
    //        }
    //    }
    //}
}
