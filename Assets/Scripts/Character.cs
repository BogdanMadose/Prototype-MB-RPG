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

    protected virtual void Start()
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
        transform.Translate(direction.normalized * speed * Time.deltaTime);
        if (direction.x != 0 || direction.y != 0)
        {
            AnimateMovement(direction);
        }
        else
        {
            // if character is not moving set the walking layer back to 0 (off)
            animator.SetLayerWeight(1, 0);
        }
    }

    /// <summary>
    /// Controls the animation transitions for characters on given Vector2 direction 
    /// </summary>
    /// <param name="direction"> x - horizontal, y - vertical </param>
    public void AnimateMovement(Vector2 direction)
    {
        // activate walking animation layer
        animator.SetLayerWeight(1, 1);

        animator.SetFloat("X", direction.x);
        animator.SetFloat("Y", direction.y);
    }
}
