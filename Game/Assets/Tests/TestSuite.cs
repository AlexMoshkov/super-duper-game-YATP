using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class Test
{
    // A Test behaves as an ordinary method
    [Test]
    public void TestSimplePasses()
    {
        CellType[,] map = new CellType[,]
        {
            {CellType.Occupied, CellType.Occupied, CellType.Occupied},
            {CellType.Occupied, CellType.Empty, CellType.Occupied},
            {CellType.Occupied, CellType.Empty, CellType.Occupied},
            {CellType.Occupied, CellType.Occupied, CellType.Occupied}
        };
        Debug.Log(map[1, 1]);
        Debug.Log(map[2, 1]);
        var start = new Vector2Int(1, 1);
        var end = new Vector2Int(2, 1);
        var pathFinder = new PathFinder();
        var result = pathFinder.FindPath(start, end, map);
        // Use the Assert class to test conditions
        Assert.AreEqual(2, result.Count);
        Assert.AreEqual(start, result[0]);
        Assert.AreEqual(end, result[1]);
    }
}
