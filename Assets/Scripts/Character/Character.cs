using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
public abstract class Character : MonoBehaviour
{
    [Tooltip("Character movement speed (float value)")]
    [SerializeField] private float speed;
    [Tooltip("Character initial health (float value)")]
    [SerializeField] private float initHealth;
    [Tooltip("Character health stat script")]
    [SerializeField] private Stat health;
    [Tooltip("Character type")]
    [SerializeField] private string type;
    private Rigidbody2D _rb;

    [Tooltip("Character HitBox")]
    [SerializeField] protected Transform hitBox;
    private Animator _animator;
    protected Coroutine _attackRoutine;

    public bool IsMoving => Direction.x != 0 || Direction.y != 0;

    public Transform Target { get; set; }
    public Stat Health => health;
    public Vector2 Direction { get; set; }
    public float Speed { get => speed; set => speed = value; }
    public bool IsAttacking { get; set; }
    public Animator Animator { get => _animator; set => _animator = value; }
    public bool IsAlive => health.CurrentValue > 0;
    public string Type => type;

    protected virtual void Start()
    {
        Health.Initialize(initHealth, initHealth);
        _rb = GetComponent<Rigidbody2D>();
        Animator = GetComponent<Animator>();
    }

    protected virtual void Update() => HandleLayers();

    private void FixedUpdate() => Move();

    /// <summary>
    /// Move character
    /// </summary>
    public void Move()
    {
        if (IsAlive)
        {
            _rb.velocity = Direction.normalized * Speed;
        }
    }

    /// <summary>
    /// Make animation layer transitions
    /// </summary>
    public virtual void HandleLayers()
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
    /// Activate animation layer
    /// </summary>
    /// <param name="layerName">Layer active</param>
    public virtual void ActivateLayer(string layerName)
    {
        for (int i = 0; i < Animator.layerCount; i++)
        {
            Animator.SetLayerWeight(i, 0);
        }

        Animator.SetLayerWeight(Animator.GetLayerIndex(layerName), 1);
    }

    /// <summary>
    /// Damage Character
    /// </summary>
    /// <param name="damage">Damage amount</param>
    /// <param name="damageSource">Damage source</param>
    public virtual void TakeDamage(float damage, Transform damageSource)
    {
        Health.CurrentValue -= damage;

        if (Health.CurrentValue <= 0)
        {
            Direction = Vector2.zero;
            _rb.velocity = Direction;
            GameManager.Instance.OnKillConfirmed(this);
            Animator.SetTrigger("die");
        }
    }
}
