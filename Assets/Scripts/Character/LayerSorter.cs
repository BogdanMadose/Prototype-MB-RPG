using System.Collections.Generic;
using UnityEngine;

public class LayerSorter : MonoBehaviour
{
    private SpriteRenderer _parentRenderer;
    private List<Obstacle> _obstacles = new List<Obstacle>();
    // Start is called before the first frame update
    void Start() => _parentRenderer = transform.parent.GetComponent<SpriteRenderer>();

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Obstacle")
        {
            Obstacle o = collision.GetComponent<Obstacle>();
            o.FadeOut();
            if (_obstacles.Count == 0 || o.SpriteRenderer.sortingOrder - 1 < _parentRenderer.sortingOrder)
            {
                _parentRenderer.sortingOrder = o.SpriteRenderer.sortingOrder - 1;
            }
            _obstacles.Add(o);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Obstacle")
        {
            Obstacle o = collision.GetComponent<Obstacle>();
            o.FadeIn();
            _obstacles.Remove(o);
            if (_obstacles.Count == 0)
            {
                _parentRenderer.sortingOrder = 200;
            }
            else
            {
                _obstacles.Sort();
                _parentRenderer.sortingOrder = _obstacles[0].SpriteRenderer.sortingOrder - 1;
            }
        }
    }
}
