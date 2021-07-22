using UnityEngine;

public class Range : MonoBehaviour
{
    private Enemy _parent;

    private void Start()
    {
        _parent = GetComponentInParent<Enemy>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            _parent.SetTarget(collision.transform);
        }
    }
}
