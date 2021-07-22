using System;
using UnityEngine;

public class Obstacle : MonoBehaviour, IComparable<Obstacle>
{
    private Color _defaultColor;
    private Color _fadedColor;
    public SpriteRenderer SpriteRenderer { get; set; }

    public int CompareTo(Obstacle other)
    {
        if (SpriteRenderer.sortingOrder > other.SpriteRenderer.sortingOrder)
        {
            return 1;
        }
        else if (SpriteRenderer.sortingOrder < other.SpriteRenderer.sortingOrder)
        {
            return -1;
        }

        return 0;
    }

    // Start is called before the first frame update
    void Start()
    {
        SpriteRenderer = GetComponent<SpriteRenderer>();
        _defaultColor = SpriteRenderer.color;
        _fadedColor = _defaultColor;
        _fadedColor.a = 0.55f;
    }

    public void FadeOut()
    {
        SpriteRenderer.color = _fadedColor;
    }

    public void FadeIn()
    {
        SpriteRenderer.color = _defaultColor;
    }
}
