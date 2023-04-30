using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System.Linq;

public class CoinsScript : MonoBehaviour
{
    private Tilemap map;
    private int totalCoinsCount;

    void Start()
    {
        map = GetComponent<Tilemap>();
        totalCoinsCount = GetTotalTiles();
    }

    void Update()
    {
        int currentCoinsCount = GetTotalTiles();

        if (currentCoinsCount < totalCoinsCount)
        {
            GameManager.CollectCoin();
            totalCoinsCount = currentCoinsCount;
        }

    }

    private int GetTotalTiles()
    {
        List<TileBase> allTiles = new List<TileBase>(map.GetTilesBlock(map.cellBounds));
        return allTiles.Count(tile => tile != null);
    }
}
