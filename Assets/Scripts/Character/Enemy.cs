using UnityEngine;

public delegate void HealthChanged(float health);
public delegate void NPCRemoved();

public class Enemy : Character, IInteractable
{
    public event HealthChanged healthChangedEvent;
    public event NPCRemoved npcRemovedEvent;

    [Tooltip("Healthbar UI object")]
    [SerializeField] private CanvasGroup healthGroup;
    [Tooltip("Initial aggro range")]
    [SerializeField] private float initAggroRange;
    [Tooltip("Possible items to drop")]
    [SerializeField] private LootTable lootTable;
    [Tooltip("Character portrait sprite")]
    [SerializeField] private Sprite portrait;
    private IState _currentState;

    public Sprite Portrait => portrait;
    public float AttackRange { get; set; }
    public float AttackTime { get; set; }
    public float AggroRange { get; set; }
    public bool InRange => Vector2.Distance(transform.position, Target.position) < AggroRange;
    public Vector3 StartPosition { get; set; }


    protected void Awake()
    {
        StartPosition = transform.position;
        AggroRange = initAggroRange;
        AttackRange = 1;
        ChangeState(new IdleState());
    }

    protected override void Update()
    {
        if (IsAlive)
        {
            if (!IsAttacking)
            {
                AttackTime += Time.deltaTime;
            }
            _currentState.Update();
        }
        base.Update();
    }

    /// <summary>
    /// Select targeted Enemy
    /// </summary>
    /// <returns>Hitbox of the targeted NPC</returns>
    public Transform Select()
    {
        healthGroup.alpha = 1;
        return hitBox;
    }

    /// <summary>
    /// Deselect targeted Enemy
    /// </summary>
    public void Deselect()
    {
        healthGroup.alpha = 0;
        healthChangedEvent -= new HealthChanged(UIManager.Instance.UpdateTargetFrame);
        npcRemovedEvent -= new NPCRemoved(UIManager.Instance.HideTargetFrame);
    }

    public override void TakeDamage(float damage, Transform damageSource)
    {
        if (!(_currentState is EvadeState))
        {
            SetTarget(damageSource);
            base.TakeDamage(damage, damageSource);
            OnHealthChanged(Health.CurrentValue);
        }
    }

    public void OnHealthChanged(float health) => healthChangedEvent?.Invoke(health);

    /// <summary>
    /// Active state transitions
    /// </summary>
    /// <param name="newState">New state</param>
    public void ChangeState(IState newState)
    {
        if (_currentState != null)
        {
            _currentState.Exit();
        }

        _currentState = newState;
        _currentState.Enter(this);
    }

    /// <summary>
    /// Set aggro target
    /// </summary>
    /// <param name="target">Aggroed targed</param>
    public void SetTarget(Transform target)
    {
        if (Target == null && !(_currentState is EvadeState))
        {
            float distance = Vector2.Distance(transform.position, target.position);
            AggroRange = initAggroRange;
            AggroRange += distance;
            Target = target;
        }
    }

    public void Reset()
    {
        this.Target = null;
        this.AggroRange = initAggroRange;
        this.Health.CurrentValue = this.Health.MaxValue;
        OnHealthChanged(Health.CurrentValue);
    }

    public void Interact()
    {
        if (!IsAlive)
        {
            lootTable.ShowLoot();
        }
    }

    public void StopInteracting()
    {
        LootWindow.Instance.CloseLootWindow();
    }

    public void OnNPCRemoved()
    {
        npcRemovedEvent?.Invoke();
        Destroy(gameObject);
    }
}
