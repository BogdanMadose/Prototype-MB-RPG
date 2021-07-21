using System.Collections;
using UnityEngine;

/// <summary>
/// Attack state Machine
/// </summary>
public class AttackState : IState
{
    private Enemy parent;
    private float attackCooldown = 3;
    private float extraRange = 0.1f;

    public void Enter(Enemy parent)
    {
        this.parent = parent;
    }

    public void Exit()
    {

    }

    public void Update()
    {
        if (parent.MAttackTime >= attackCooldown && !parent.IsAttacking)
        {
            parent.MAttackTime = 0;
            parent.StartCoroutine(Attack());
        }

        if (parent.MTarget != null)
        {
            float distance = Vector2.Distance(parent.MTarget.position, parent.transform.position);

            if (distance >= parent.MAttackRange + extraRange && !parent.IsAttacking)
            {
                parent.ChangeState(new FollowState());
            }
        }
        else
        {
            parent.ChangeState(new IdleState());
        }
    }

    /// <summary>
    /// Attack function
    /// </summary>
    /// <returns>Waits for current animation exit time then stops attack animation</returns>
    public IEnumerator Attack()
    {
        parent.IsAttacking = true;
        parent.MAnimator.SetTrigger("attack");
        yield return new WaitForSeconds(parent.MAnimator.GetCurrentAnimatorStateInfo(2).length);
        parent.IsAttacking = false;
    }
}
