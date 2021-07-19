using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour, IComparable<Obstacle>
{
    private Color defaultColor;
    private Color fadedColor;
    public SpriteRenderer MSpriteRenderer { get; set; }

    public int CompareTo(Obstacle other)
    {
        if (MSpriteRenderer.sortingOrder > other.MSpriteRenderer.sortingOrder)
        {
            return 1;
        }
        else if (MSpriteRenderer.sortingOrder < other.MSpriteRenderer.sortingOrder)
        {
            return -1;
        }

        return 0;
    }

    // Start is called before the first frame update
    void Start()
    {
        MSpriteRenderer = GetComponent<SpriteRenderer>();
        defaultColor = MSpriteRenderer.color;
        fadedColor = defaultColor;
        fadedColor.a = 0.55f;
    }

    public void FadeOut()
    {
        MSpriteRenderer.color = fadedColor;
    }

    public void FadeIn()
    {
        MSpriteRenderer.color = defaultColor;
    }
}
