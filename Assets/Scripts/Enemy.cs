using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : NPC
{
    [SerializeField] private CanvasGroup healthGroup;
    private Transform target;

    public Transform MTarget { get => target; set => target = value; }

    protected override void Update()
    {
        FollowTarget();
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

    public override void TakeDamage(float damage)
    {
        base.TakeDamage(damage);

        OnHealthChanged(MHealth.MCurrentValue);
    }

    private void FollowTarget()
    {
        if(target != null)
        {
            direction = (target.transform.position - transform.position).normalized;
            transform.position = Vector2.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
        }
        else
        {
            direction = Vector2.zero;
        }
    }
}
