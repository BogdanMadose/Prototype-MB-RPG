using UnityEngine;

public class Enemy : NPC
{
    [Tooltip("Healthbar UI object")]
    [SerializeField] private CanvasGroup healthGroup;
    [Tooltip("Initial aggro range")]
    [SerializeField] private float initAggroRange;
    [Tooltip("Possible items to drop")]
    [SerializeField] private LootTable lootTable;
    private IState _currentState;

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

    public override Transform Select()
    {
        healthGroup.alpha = 1;
        return base.Select();
    }

    public override void Deselect()
    {
        healthGroup.alpha = 0;
        base.Deselect();
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

    /// <summary>
    /// Transition between states
    /// </summary>
    /// <param name="newState">State to be transitioned into</param>
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
    /// <param name="target">Target which will take aggro</param>
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

    public override void Interact()
    {
        if (!IsAlive)
        {
            lootTable.ShowLoot();
        }
    }
}
