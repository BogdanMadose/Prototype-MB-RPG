using UnityEngine;

public class Range : MonoBehaviour
{
    private Enemy parent;

    private void Start()
    {
        parent = GetComponentInParent<Enemy>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            parent.MTarget = collision.transform;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            parent.MTarget = null;
        }
    }
}
