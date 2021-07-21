using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
public abstract class Character : MonoBehaviour
{
    #region Variables
    /// <summary>
    /// Variable to hold the speed at which every character moves
    /// </summary>
    [Tooltip("Character movement speed (float value)")]
    [SerializeField] private float speed;

    /// <summary>
    /// Initial allowed character health
    /// </summary>
    [Tooltip("Character initial health (float value)")]
    [SerializeField] private float initHealth;

    /// <summary>
    /// Character health stat reference
    /// </summary>
    [Tooltip("Character health stat script")]
    [SerializeField] private Stat health;

    /// <summary>
    /// Character physics reference
    /// </summary>
    private Rigidbody2D rb;

    [Tooltip("Character HitBox")]
    [SerializeField] protected Transform hitBox;

    /// <summary>
    /// Variable to hold the animator component of each character
    /// </summary>
    private Animator animator;

    /// <summary>
    /// Reference to attack coroutine
    /// </summary>
    protected Coroutine attackRoutine;
    #endregion

    /// <summary>
    /// Returns 0 if character is not moving, 1 if character moves
    /// </summary>
    public bool IsMoving
    {
        get
        {
            return MDirection.x != 0 || MDirection.y != 0;
        }
    }

    public Transform MTarget { get; set; }
    public Stat MHealth { get => health; }
    public Vector2 MDirection { get; set; }
    public float MSpeed { get => speed; set => speed = value; }
    public bool IsAttacking { get; set; }
    public Animator MAnimator { get => animator; set => animator = value; }

    public bool IsAlive
    {
        get
        {
            return health.MCurrentValue > 0;
        }
    }

    protected virtual void Start()
    {
        MHealth.Initialize(initHealth, initHealth);
        rb = GetComponent<Rigidbody2D>();
        MAnimator = GetComponent<Animator>(); 
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
        if (IsAlive)
        {
            rb.velocity = MDirection.normalized * MSpeed;
        }
    }

    #region Animation Handlers
    /// <summary>
    /// Handles animation transitions
    /// </summary>
    public void HandleLayers()
    {
        if (IsAlive)
        {
            if (IsMoving)
            {
                ActivateLayer("WalkLayer");

                MAnimator.SetFloat("X", MDirection.x);
                MAnimator.SetFloat("Y", MDirection.y);
            }
            else if (IsAttacking)
            {
                ActivateLayer("AttackLayer");
            }
            else
            {
                ActivateLayer("IdleLayer");
            }
        }
        else
        {
            ActivateLayer("DeathLayer");
        }
    }

    /// <summary>
    /// Handles animation state layers 
    /// </summary>
    /// <param name="layerName">layer that needs to be activated</param>
    public void ActivateLayer(string layerName)
    {
        for (int i = 0; i < MAnimator.layerCount; i++)
        {
            MAnimator.SetLayerWeight(i, 0);
        }

        MAnimator.SetLayerWeight(MAnimator.GetLayerIndex(layerName), 1);
    } 
    #endregion

    /// <summary>
    /// Handles damaging characters
    /// </summary>
    /// <param name="damage">Damage amount value</param>
    public virtual void TakeDamage(float damage, Transform damageSource)
    {
        if (MTarget == null)
        {
            MTarget = damageSource;
        }

        MHealth.MCurrentValue -= damage;

        if (MHealth.MCurrentValue <= 0)
        {
            MDirection = Vector2.zero;
            rb.velocity = MDirection;
            MAnimator.SetTrigger("die");
        }
    }
}
