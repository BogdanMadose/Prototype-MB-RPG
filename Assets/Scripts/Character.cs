using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
public abstract class Character : MonoBehaviour
{
    #region Variables
    /// <summary>
    /// Variable to hold the speed at which every character moves
    /// </summary>
    [SerializeField] private float speed;

    /// <summary>
    /// Variable to hold the direction in which every character moves
    /// </summary>
    protected Vector2 direction;

    /// <summary>
    /// Character physics reference
    /// </summary>
    private Rigidbody2D rb;

    /// <summary>
    /// Variable to hold the animator component of each character
    /// </summary>
    protected Animator mAnimator;

    /// <summary>
    /// Says if a character is attacking or not // Default = false;
    /// </summary>
    protected bool isAttacking = false;

    /// <summary>
    /// Reference to attack coroutine
    /// </summary>
    protected Coroutine attackRoutine;

    /// <summary>
    /// Initial allowed player health
    /// </summary>
    [SerializeField] private float initHealth;

    [SerializeField] protected Transform hitBox;

    [SerializeField] protected Stat health;
    #endregion

    /// <summary>
    /// Returns 0 if character is not moving, 1 if character moves
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
        health.Initialize(initHealth, initHealth);
        rb = GetComponent<Rigidbody2D>();
        mAnimator = GetComponent<Animator>(); 
    }

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

    #region Animation Handlers
    /// <summary>
    /// Handles animation transitions
    /// </summary>
    public void HandleLayers()
    {
        if (IsMoving)
        {
            ActivateLayer("WalkLayer");

            mAnimator.SetFloat("X", direction.x);
            mAnimator.SetFloat("Y", direction.y);

            StopAttack();
        }
        else if (isAttacking)
        {
            ActivateLayer("AttackLayer");
        }
        else
        {
            ActivateLayer("IdleLayer");
        }
    }

    /// <summary>
    /// Handles animation state layers 
    /// </summary>
    /// <param name="layerName">layer that needs to be activated</param>
    public void ActivateLayer(string layerName)
    {
        for (int i = 0; i < mAnimator.layerCount; i++)
        {
            mAnimator.SetLayerWeight(i, 0);
        }

        mAnimator.SetLayerWeight(mAnimator.GetLayerIndex(layerName), 1);
    } 

    /// <summary>
    /// Function to stop attacking.
    /// 
    /// <para>Sets both animator bool and isAttacking to false and stops the attack coroutine</para>
    /// </summary>
    public virtual void StopAttack()
    {
        isAttacking = false;
        mAnimator.SetBool("attack", isAttacking);

        if (attackRoutine != null)
        {
            StopCoroutine(attackRoutine);
        }
    }
    #endregion

    public virtual void TakeDamage(float damage)
    {
        health.MCurrentValue -= damage;

        if (health.MCurrentValue <= 0)
        {
            mAnimator.SetTrigger("die");
        }
    }
}
