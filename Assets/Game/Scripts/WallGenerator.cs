using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class WallGenerator
{
    public static void CreateWalls(HashSet<Vector2Int> floorPositions, TilemapVisualizer tilemapVisualizer)
    {
        var basicWallPositions = FindWallsInDirections(floorPositions, Direction2D.cardinalDirections);
        var cornerWallPositions = FindWallsInDirections(floorPositions, Direction2D.diagonalDirections);
        CreateBasicWall(tilemapVisualizer, basicWallPositions, floorPositions);
        CreateCornerWall(tilemapVisualizer, cornerWallPositions, floorPositions);
        //foreach (var position in basicWallPositions)
        //{
        //    tilemapVisualizer.PaintSingleBasicWall(position, tilemapVisuali);
        //}
    }

    private static void CreateCornerWall(TilemapVisualizer tilemapVisualizer, HashSet<Vector2Int> cornerWallPositions, HashSet<Vector2Int> floorPositions)
    {
        foreach (var position in cornerWallPositions) 
        {
            string neighboursBinaryValue = "";
            foreach (var direction in Direction2D.eightDirections) 
            {
                var neighbourPosition = position + direction;
                if (floorPositions.Contains(neighbourPosition)) neighboursBinaryValue += "1";
                else neighboursBinaryValue += "0";
            }
            tilemapVisualizer.PaintSingleCornerWall(position, neighboursBinaryValue);
        }
    }

    private static void CreateBasicWall(TilemapVisualizer tilemapVisualizer, HashSet<Vector2Int> basicWallPositions, HashSet<Vector2Int> floorPositions)
    {
        foreach (var position in basicWallPositions)
        {
            string neighboursBinaryType = "";
            foreach (var direction in Direction2D.cardinalDirections)
            {
                var neighbourPosition = position + direction;
                if (floorPositions.Contains(neighbourPosition))
                {
                    neighboursBinaryType += "1";
                }
                else
                {
                    neighboursBinaryType += "0";
                }
            }
            tilemapVisualizer.PaintSingleBasicWall(position, neighboursBinaryType);
        }
    }

    private static HashSet<Vector2Int> FindWallsInDirections(HashSet<Vector2Int> floorPositions, List<Vector2Int> directionList)
    {
        HashSet<Vector2Int> wallPositions = new HashSet<Vector2Int>();
        foreach (var position in floorPositions)
        {
            foreach (var direction in directionList)
            {
                var neighbourPosition = position + direction;
                if (floorPositions.Contains(neighbourPosition) == false)
                    wallPositions.Add(neighbourPosition);
            }
        }
        return wallPositions;
    }
}
