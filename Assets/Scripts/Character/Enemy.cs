using UnityEngine;

public class Enemy : NPC
{
    [SerializeField] private CanvasGroup healthGroup;
    [SerializeField] private float initAggroRange;
    private IState currentState;

    public float MAttackRange { get; set; }
    public float MAttackTime { get; set; }
    public float MAggroRange { get; set; }
    public bool InRange => Vector2.Distance(transform.position, MTarget.position) < MAggroRange;
    public Vector3 MStartPosition { get; set; }


    protected void Awake()
    {
        MStartPosition = transform.position;
        MAggroRange = initAggroRange;
        MAttackRange = 1;
        ChangeState(new IdleState());
    }

    protected override void Update()
    {
        if (IsAlive)
        {
            if (!IsAttacking)
            {
                MAttackTime += Time.deltaTime;
            }
            currentState.Update();
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
        if (!(currentState is EvadeState))
        {
            SetTarget(damageSource);
            base.TakeDamage(damage, damageSource);
            OnHealthChanged(MHealth.MCurrentValue);
        }
    }

    /// <summary>
    /// Transition between states
    /// </summary>
    /// <param name="newState">State to be transitioned into</param>
    public void ChangeState(IState newState)
    {
        if (currentState != null)
        {
            currentState.Exit();
        }

        currentState = newState;
        currentState.Enter(this);
    }

    /// <summary>
    /// Set aggro target
    /// </summary>
    /// <param name="target">Target which will take aggro</param>
    public void SetTarget(Transform target)
    {
        if (MTarget == null && !(currentState is EvadeState))
        {
            float distance = Vector2.Distance(transform.position, target.position);
            MAggroRange = initAggroRange;
            MAggroRange += distance;
            MTarget = target;
        }
    }

    public void Reset()
    {
        this.MTarget = null;
        this.MAggroRange = initAggroRange;
        this.MHealth.MCurrentValue = this.MHealth.MMaxValue;
        OnHealthChanged(MHealth.MCurrentValue);
    }
}
