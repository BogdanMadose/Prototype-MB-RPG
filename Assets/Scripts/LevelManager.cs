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

    // Update is called once per frame
    void Update()
    {
        
    }

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

                        if (newElement.MTileTag == "Tree")
                        {
                            go.GetComponent<SpriteRenderer>().sortingOrder = height * 2 - y * 2;
                        }

                        go.transform.parent = map;
                    }
                }
            }
        }
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
