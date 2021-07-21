using UnityEngine;

public class Enemy : NPC
{
    [SerializeField] private CanvasGroup healthGroup;
    private IState currentState;

    public float MAttackRange { get; set; }
    public float MAttackTime { get; set; }

    protected void Awake()
    {
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
        base.TakeDamage(damage, damageSource);

        OnHealthChanged(MHealth.MCurrentValue);
    }

    public void ChangeState(IState newState)
    {
        if (currentState != null)
        {
            currentState.Exit();
        }

        currentState = newState;
        currentState.Enter(this);
    }
}
