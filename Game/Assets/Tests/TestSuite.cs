using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class Test
{
    // A Test behaves as an ordinary method
    [Test]
    public void TestPathFinder()
    {
        CellType[,] map = new CellType[,]
        {
            {CellType.Occupied, CellType.Occupied, CellType.Occupied},
            {CellType.Occupied, CellType.Empty, CellType.Occupied},
            {CellType.Occupied, CellType.Empty, CellType.Occupied},
            {CellType.Occupied, CellType.Occupied, CellType.Occupied}
        };

        var start = new Vector2Int(1, 1);
        var end = new Vector2Int(2, 1);
        var pathFinder = new PathFinder();
        var result = pathFinder.FindPath(start, end, map);
        // Use the Assert class to test conditions
        Assert.AreEqual(2, result.Count);
        Assert.AreEqual(start, result[0]);
        Assert.AreEqual(end, result[1]);
    }
    
    [Test] 
    public void TestPathFinderWhenStartEqualEnd()
    {
        CellType[,] map = new CellType[,]
        {
            {CellType.Occupied, CellType.Occupied, CellType.Occupied},
            {CellType.Occupied, CellType.Empty, CellType.Occupied},
            {CellType.Occupied, CellType.Empty, CellType.Occupied},
            {CellType.Occupied, CellType.Occupied, CellType.Occupied}
        };
        var start = new Vector2Int(1, 1);
        var end = new Vector2Int(1, 1);
        var pathFinder = new PathFinder();
        var result = pathFinder.FindPath(start, end, map);
        Assert.AreEqual(1, result.Count);
        Assert.AreEqual(start, result[0]);
    }
    
    [Test] 
    public void TestPathFinderWhenNoPathToEnd()
    {
        CellType[,] map = new CellType[,]
        {
            {CellType.Occupied, CellType.Occupied, CellType.Occupied},
            {CellType.Occupied, CellType.Empty, CellType.Occupied},
            {CellType.Occupied, CellType.Occupied, CellType.Occupied},
            {CellType.Occupied, CellType.Empty, CellType.Occupied},
            {CellType.Occupied, CellType.Occupied, CellType.Occupied}
        };
        var start = new Vector2Int(1, 1);
        var end = new Vector2Int(3, 1);
        var pathFinder = new PathFinder();
        var result = pathFinder.FindPath(start, end, map);
        Assert.AreEqual(0, result.Count);
    }
    
    [Test] 
    public void TestPathFinderWithRotate()
    {
        CellType[,] map = new CellType[,]
        {
            {CellType.Occupied, CellType.Occupied, CellType.Occupied},
            {CellType.Occupied, CellType.Empty, CellType.Occupied},
            {CellType.Occupied, CellType.Empty, CellType.Occupied},
            {CellType.Occupied, CellType.Empty, CellType.Empty},
            {CellType.Occupied, CellType.Occupied, CellType.Occupied}
            
        };
        Debug.Log(map[3, 2]);
        Debug.Log(map[2, 1]);
        var start = new Vector2Int(3, 2);
        var end = new Vector2Int(1, 1);
        var pathFinder = new PathFinder();
        var result = pathFinder.FindPath(start, end, map);
        Assert.AreEqual(4, result.Count);
        Assert.AreEqual(start, result[0]);
        Assert.AreEqual(new Vector2Int(3 ,1), result[1]);
        Assert.AreEqual(new Vector2Int(2 ,1) , result[2]);
        Assert.AreEqual(end, result[3]);
    }
}
