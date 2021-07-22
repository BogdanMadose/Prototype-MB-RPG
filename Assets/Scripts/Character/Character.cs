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
    private Rigidbody2D _rb;

    [Tooltip("Character HitBox")]
    [SerializeField] protected Transform hitBox;

    /// <summary>
    /// Variable to hold the animator component of each character
    /// </summary>
    private Animator _animator;

    /// <summary>
    /// Reference to attack coroutine
    /// </summary>
    protected Coroutine _attackRoutine;
    #endregion

    /// <summary>
    /// Returns 0 if character is not moving, 1 if character moves
    /// </summary>
    public bool IsMoving => Direction.x != 0 || Direction.y != 0;

    public Transform Target { get; set; }
    public Stat Health => health;
    public Vector2 Direction { get; set; }
    public float Speed { get => speed; set => speed = value; }
    public bool IsAttacking { get; set; }
    public Animator Animator { get => _animator; set => _animator = value; }

    public bool IsAlive => health.CurrentValue > 0;

    protected virtual void Start()
    {
        Health.Initialize(initHealth, initHealth);
        _rb = GetComponent<Rigidbody2D>();
        Animator = GetComponent<Animator>();
    }

    protected virtual void Update() => HandleLayers();

    private void FixedUpdate() => Move();

    /// <summary>
    /// Handles all character's movements
    /// </summary>
    public void Move()
    {
        if (IsAlive)
        {
            _rb.velocity = Direction.normalized * Speed;
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

                Animator.SetFloat("X", Direction.x);
                Animator.SetFloat("Y", Direction.y);
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
        for (int i = 0; i < Animator.layerCount; i++)
        {
            Animator.SetLayerWeight(i, 0);
        }

        Animator.SetLayerWeight(Animator.GetLayerIndex(layerName), 1);
    }
    #endregion

    /// <summary>
    /// Handles damaging characters
    /// </summary>
    /// <param name="damage">Damage amount value</param>
    /// <param name="damageSource">Object that deals damage</param>
    public virtual void TakeDamage(float damage, Transform damageSource)
    {
        Health.CurrentValue -= damage;

        if (Health.CurrentValue <= 0)
        {
            Direction = Vector2.zero;
            _rb.velocity = Direction;
            Animator.SetTrigger("die");
        }
    }
}
