using UnityEngine;

/// <summary>
/// Follow State Machine
/// </summary>
class FollowState : IState
{
    private Enemy parent;

    public void Enter(Enemy parent)
    {
        this.parent = parent;
    }

    public void Exit()
    {
        parent.MDirection = Vector2.zero;
    }

    public void Update()
    {
        if(parent.MTarget != null)
        {
            parent.MDirection = (parent.MTarget.transform.position - parent.transform.position).normalized;
            parent.transform.position = Vector2.MoveTowards(parent.transform.position, parent.MTarget.position, parent.MSpeed * Time.deltaTime);
            float distance = Vector2.Distance(parent.MTarget.position, parent.transform.position);
            
            if (distance <= parent.MAttackRange)
            {
                parent.ChangeState(new AttackState());
            }
        }
        else
        {
            parent.ChangeState(new IdleState());
        }
    }
}

