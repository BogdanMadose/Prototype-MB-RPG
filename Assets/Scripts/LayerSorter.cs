using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LayerSorter : MonoBehaviour
{
    private SpriteRenderer parentRendere;
    private List<Obstacle> obstacles = new List<Obstacle>();
    // Start is called before the first frame update
    void Start()
    {
        parentRendere = transform.parent.GetComponent<SpriteRenderer>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Obstacle")
        {
            Obstacle o = collision.GetComponent<Obstacle>();
            o.FadeOut();
            if (obstacles.Count == 0 || o.MSpriteRenderer.sortingOrder - 1 < parentRendere.sortingOrder)
            {
                parentRendere.sortingOrder = o.MSpriteRenderer.sortingOrder - 1;
            }
            obstacles.Add(o);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Obstacle")
        {
            Obstacle o = collision.GetComponent<Obstacle>();
            o.FadeIn();
            obstacles.Remove(o);
            if (obstacles.Count == 0)
            {
                parentRendere.sortingOrder = 200;
            }
            else
            {
                obstacles.Sort();
                parentRendere.sortingOrder = obstacles[0].MSpriteRenderer.sortingOrder - 1;
            }
        }
    }
}
