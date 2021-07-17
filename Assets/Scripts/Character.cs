using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Character : MonoBehaviour
{
    /// <summary>
    /// Variable to hold the speed at which every character moves
    /// </summary>
    [SerializeField] private float speed;

    /// <summary>
    /// Variable to hold the direction in which every character moves
    /// </summary>
    protected Vector2 direction;

    /// <summary>
    /// Variable to hold the animator component of each character
    /// </summary>
    private Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>(); 
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        Move();
    }

    /// <summary>
    /// Handles all character's movements and animations
    /// </summary>
    public void Move()
    {
        transform.Translate(direction * speed * Time.deltaTime);
        AnimateMovement(direction);
    }

    /// <summary>
    /// Controls the animation transitions for characters on given Vector2 direction 
    /// </summary>
    /// <param name="direction"> x - horizontal, y - vertical </param>
    public void AnimateMovement(Vector2 direction)
    {
        animator.SetFloat("X", direction.x);
        animator.SetFloat("Y", direction.y);
    }
}
