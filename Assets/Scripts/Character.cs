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

    private Rigidbody2D rb;

    /// <summary>
    /// Variable to hold the animator component of each character
    /// </summary>
    private Animator mAnimator;

    /// <summary>
    /// Returns 0 if character is not, 1 if character moves
    /// </summary>
    public bool IsMoving
    {
        get
        {
            return direction.x != 0 || direction.y != 0;
        }
    }

    protected virtual void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        mAnimator = GetComponent<Animator>(); 
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        HandleLayers();
    }

    private void FixedUpdate()
    {
        Move();
    }

    /// <summary>
    /// Handles all character's movements
    /// </summary>
    public void Move()
    {
        rb.velocity = direction.normalized * speed;
    }

    /// <summary>
    /// Handles animation transitions
    /// </summary>
    public void HandleLayers()
    {
        if (IsMoving)
        {
            // activate walking animation layer
            ActivateLayer("WalkLayer");

            mAnimator.SetFloat("X", direction.x);
            mAnimator.SetFloat("Y", direction.y);
        }
        else
        {
            // if character is not moving set the walking layer back to 0 (off)
            ActivateLayer("IdleLayer");
        }
    }

    /// <summary>
    /// Handles animation state layers 
    /// </summary>
    /// <param name="layerName">layer that needs to be activated</param>
    public void ActivateLayer(string layerName)
    {
        // loops through all current layers and sets them all to 0
        for (int i = 0; i < mAnimator.layerCount; i++)
        {
            mAnimator.SetLayerWeight(i, 0);
        }

        // sets desired layer to active
        mAnimator.SetLayerWeight(mAnimator.GetLayerIndex(layerName), 1);
    }
}
