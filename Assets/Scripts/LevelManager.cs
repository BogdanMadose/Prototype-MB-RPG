using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private Texture2D[] mapData;
    [SerializeField] private MapElement[] mapElements;
    [SerializeField] private Sprite defaultTile;
    [SerializeField] private Transform map;
    private Dictionary<Point, GameObject> waterTiles = new Dictionary<Point, GameObject>();

    private Vector3 WorldStartPosition
    {
        get
        {
            return Camera.main.ScreenToWorldPoint(new Vector3(0, 0));
        }
    }

    // Start is called before the first frame update
    void Start()
    { 
        GenerateMap();
    }

    /// <summary>
    /// Map generation alghorithm
    /// </summary>
    private void GenerateMap()
    {
        int height = mapData[0].height;
        int width = mapData[0].width;

        for (int i = 0; i < mapData.Length; i++)
        {
            for (int x = 0; x < mapData[i].width; x++)
            {
                for (int y = 0; y < mapData[i].height; y++)
                {
                    Color c = mapData[i].GetPixel(x, y);
                    MapElement newElement = Array.Find(mapElements, e => e.MColor == c);

                    if (newElement != null)
                    {
                        float xPos = WorldStartPosition.x + (defaultTile.bounds.size.x * x);
                        float yPos = WorldStartPosition.y + (defaultTile.bounds.size.y * y);

                        GameObject go = Instantiate(newElement.MElementPrefab);
                        go.transform.position = new Vector2(xPos, yPos);

                        if (newElement.MTileTag == "Water")
                        {
                            waterTiles.Add(new Point(x,y), go);
                        }

                        if (newElement.MTileTag == "Tree")
                        {
                            go.GetComponent<SpriteRenderer>().sortingOrder = height * 2 - y * 2;
                        }

                        go.transform.parent = map;
                    }
                }
            }
        }
        CheckWater();
    }

    /// <summary>
    /// Check if the tile in the dict is a water tile
    /// </summary>
    private void CheckWater ()
    {
        foreach (KeyValuePair<Point, GameObject> tile in waterTiles)
        {
            string composition = TileCheck(tile.Key);
        }       
    }

    /// <summary>
    /// Checks if the object on the current position is water or earth
    /// </summary>
    /// <param name="currentPoint">Current position coordonates</param>
    /// <returns>String of composition(eg.: EWWWWEEW)</returns>
    private string TileCheck(Point currentPoint)
    {
        string composition = string.Empty;

        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                if (x != 0 || y != 0)
                {
                    if (waterTiles.ContainsKey(new Point(currentPoint.MX + x, currentPoint.MY + y)))
                    {
                        composition += "W";
                    }
                    else
                    {
                        composition += "E";
                    }
                }
            }
        }
        Debug.Log(composition);
        return composition;
    }
}

[Serializable]
public class MapElement
{
    [SerializeField] private string tileTag;
    [SerializeField] private Color color;
    [SerializeField] private GameObject elementPrefab;

    public GameObject MElementPrefab { get => elementPrefab; }
    public Color MColor { get => color; }
    public string MTileTag { get => tileTag; }
}

public struct Point
{
    public int MX { get; set; }
    public int MY { get; set; }

    public Point(int x, int y)
    {
        this.MX = x;
        this.MY = y;
    }
}
