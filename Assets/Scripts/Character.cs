using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Character : MonoBehaviour
{
    /// <summary>
    /// Variable to set the speed at which every character moves
    /// </summary>
    [SerializeField] private float speed;

    /// <summary>
    /// Variable to set the direction in which every character moves
    /// </summary>
    protected Vector2 direction;

    // Update is called once per frame
    protected virtual void Update()
    {
        Move();
    }

    /// <summary>
    /// Handles all character's movements
    /// </summary>
    public void Move()
    {
        transform.Translate(direction * speed * Time.deltaTime);
    }
}
