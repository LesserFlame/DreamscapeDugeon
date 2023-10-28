using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TilemapVisualizer : MonoBehaviour
{
    [SerializeField] private Tilemap floorTilemap, wallTilemap, overlayTilemap;

    [SerializeField] private TileBase floorTile, wallTop, wallSideRight, wallSideLeft, wallBottom, wallFull;
    [SerializeField] private TileBase wallInnerCornerDownLeft, wallInnerCornerDownRight, wallInnerCornerUpLeft, wallInnerCornerUpRight;
    [SerializeField] private TileBase wallDiagonalCornerDownRight, wallDiagonalCornerDownLeft, wallDiagonalCornerUpRight, wallDiagonalCornerUpLeft;

    public void PaintFloorTiles(IEnumerable<Vector2Int> floorPositions)
    {
        PaintTiles(floorPositions, floorTilemap, floorTile);
    }

    public void PaintTiles(IEnumerable<Vector2Int> positions, Tilemap tilemap, TileBase tile)
    {
        foreach (var position in positions) 
        {
            PaintSingleTile(tilemap, tile, position);
        }
    }

    public void PaintRoom(RoomInfo room, Vector2Int position, bool overlap = true)
    {
        for (int x = room.floorTilemap.cellBounds.x; x < room.floorTilemap.cellBounds.xMax; x++)
        {
            for (int y = room.floorTilemap.cellBounds.y; y < room.floorTilemap.cellBounds.yMax; y++)
            {
                Vector3Int cellPosition = new Vector3Int(x, y, 0);
                
                TileBase tile = room.floorTilemap.GetTile(cellPosition);

                if (tile != null)
                {
                    if (!overlap)
                    {
                        if (floorTilemap.GetTile(cellPosition + (Vector3Int)position) != null) continue;
                    }
                    floorTilemap.SetTile(cellPosition + (Vector3Int)position, tile);
                }
            }
        }
        for(int x = room.wallTilemap.cellBounds.x; x < room.wallTilemap.cellBounds.xMax; x++)
        {
            for (int y = room.wallTilemap.cellBounds.y; y < room.wallTilemap.cellBounds.yMax; y++)
            {
                Vector3Int cellPosition = new Vector3Int(x, y, 0);

                TileBase tile = room.wallTilemap.GetTile(cellPosition);

                if (tile != null)
                {
                    if (!overlap)
                    {
                        if (wallTilemap.GetTile(cellPosition + (Vector3Int)position) != null) continue;
                    }
                    wallTilemap.SetTile(cellPosition + (Vector3Int)position, tile);
                }
            }
        }
        for(int x = room.overlayTilemap.cellBounds.x; x < room.overlayTilemap.cellBounds.xMax; x++)
        {
            for (int y = room.overlayTilemap.cellBounds.y; y < room.overlayTilemap.cellBounds.yMax; y++)
            {
                Vector3Int cellPosition = new Vector3Int(x, y, 0);

                TileBase tile = room.overlayTilemap.GetTile(cellPosition);

                if (tile != null)
                {
                    if (!overlap)
                    {
                        if (overlayTilemap.GetTile(cellPosition + (Vector3Int)position) != null) continue;
                    }
                    overlayTilemap.SetTile(cellPosition + (Vector3Int)position, tile);
                }
            }
        }
    }

    public bool CheckEmpty(RoomInfo room, Vector2Int position)
    {
        for (int x = room.floorTilemap.cellBounds.x - 1; x < room.floorTilemap.cellBounds.xMax + 1; x++)
        {
            for (int y = room.floorTilemap.cellBounds.y - 1; y < room.floorTilemap.cellBounds.yMax + 1; y++)
            {
                Vector3Int cellPosition = new Vector3Int(x, y, 0);

                TileBase tile = floorTilemap.GetTile(cellPosition + (Vector3Int)position);
                TileBase roomTile = room.floorTilemap.GetTile(cellPosition);

                if (tile != null && roomTile != null)
                {
                    return false;
                }
            }
        }
        for (int x = room.wallTilemap.cellBounds.x - 1; x < room.wallTilemap.cellBounds.xMax + 1; x++)
        {
            for (int y = room.wallTilemap.cellBounds.y - 1; y < room.wallTilemap.cellBounds.yMax + 1; y++)
            {
                Vector3Int cellPosition = new Vector3Int(x, y, 0);

                TileBase tile = wallTilemap.GetTile(cellPosition + (Vector3Int)position);
                TileBase roomTile = room.wallTilemap.GetTile(cellPosition);

                if (tile != null && roomTile != null)
                {
                    return false;
                }
            }
        }
        for (int x = room.overlayTilemap.cellBounds.x - 1; x < room.overlayTilemap.cellBounds.xMax + 1; x++)
        {
            for (int y = room.overlayTilemap.cellBounds.y; y < room.overlayTilemap.cellBounds.yMax + 1; y++)
            {
                Vector3Int cellPosition = new Vector3Int(x, y, 0);

                TileBase tile = overlayTilemap.GetTile(cellPosition + (Vector3Int)position);
                TileBase roomTile = room.overlayTilemap.GetTile(cellPosition);

                if (tile != null && roomTile != null)
                {
                    return false;
                }
            }
        }
        return true;
    }

    public void PaintSingleTile(Tilemap tilemap, TileBase tile, Vector2Int position)
    {
        var tilePosition = tilemap.WorldToCell((Vector3Int)position);
        tilemap.SetTile(tilePosition, tile);
    }

    public void Clear()
    {
        floorTilemap.ClearAllTiles();
        wallTilemap.ClearAllTiles();
        overlayTilemap.ClearAllTiles();
    }

    internal void PaintSingleBasicWall(Vector2Int position, string binaryValue)
    {
        //Debug.Log(position + " type: " + binaryValue);
        int typeAsInt = Convert.ToInt32(binaryValue, 2);
        TileBase tile = null;
        if (WallTypesHelper.wallTop.Contains(typeAsInt)) tile = wallTop;
        else if (WallTypesHelper.wallSideRight.Contains(typeAsInt)) tile = wallSideRight;
        else if (WallTypesHelper.wallSideLeft.Contains(typeAsInt)) tile = wallSideLeft;
        else if (WallTypesHelper.wallBottom.Contains(typeAsInt)) tile = wallBottom;
        else if (WallTypesHelper.wallInnerCornerUpLeft.Contains(typeAsInt)) tile = wallInnerCornerUpLeft;
        else if (WallTypesHelper.wallInnerCornerUpRight.Contains(typeAsInt)) tile = wallInnerCornerUpRight;
        else if (WallTypesHelper.wallFull.Contains(typeAsInt)) tile = wallFull;
        if (tile != null) PaintSingleTile(wallTilemap, tile, position);
    }

    internal void PaintSingleCornerWall(Vector2Int position, string binaryValue)
    {
        //disgusting but functional
        int typeAsInt = Convert.ToInt32(binaryValue, 2);
        TileBase tile = null;
        //if (WallTypesHelper.wallTop.Contains(typeAsInt)) tile = wallTop;
        if (WallTypesHelper.wallInnerCornerDownLeft.Contains(typeAsInt)) tile = wallInnerCornerDownLeft;
        else if (WallTypesHelper.wallInnerCornerDownRight.Contains(typeAsInt)) tile = wallInnerCornerDownRight;
        else if (WallTypesHelper.wallDiagonalCornerDownRight.Contains(typeAsInt)) tile = wallDiagonalCornerDownRight;
        else if (WallTypesHelper.wallDiagonalCornerDownLeft.Contains(typeAsInt)) tile = wallDiagonalCornerDownLeft;
        else if (WallTypesHelper.wallDiagonalCornerUpRight.Contains(typeAsInt)) tile = wallDiagonalCornerUpRight;
        else if (WallTypesHelper.wallDiagonalCornerUpLeft.Contains(typeAsInt)) tile = wallDiagonalCornerUpLeft;
        else if (WallTypesHelper.wallFullEightDirections.Contains(typeAsInt)) tile = wallFull;
        if (tile != null) PaintSingleTile(wallTilemap, tile, position);
    }

    public Vector3 GetWorldPosition(Vector2Int position)
    {
        Vector3 worldPosition = floorTilemap.CellToWorld((Vector3Int)position);
        return worldPosition;
    }
    public Vector2Int GetGridPosition(Vector3 position) 
    {
        //var gridPosition = new Vector2Int((int)position.x - (int)floorTilemap.cellSize.x, (int)position.y - (int)floorTilemap.cellSize.y);
        var gridPosition = floorTilemap.WorldToCell(position);
        //float cellSize = floorTilemap.cellSize.x;
        //position /= cellSize;
        //Debug.Log(gridPosition);
        //Vector2Int gridPosition = new Vector2Int((int)position.x, (int)position.y);
        //Vector2Int gridPosition = new Vector2Int((int)position.x * 2, (int)position.y * 2);
        Vector2Int pos = new Vector2Int(gridPosition.x, gridPosition.y);
        return pos;
    }
}   
