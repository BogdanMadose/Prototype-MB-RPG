using UnityEngine;

public class EvadeState : IState
{
    private Enemy parent;

    public void Enter(Enemy parent)
    {
        this.parent = parent;
    }

    public void Exit()
    {
        parent.MDirection = Vector2.zero;
        parent.Reset();
    }

    public void Update()
    {
        parent.MDirection = (parent.MStartPosition - parent.transform.position).normalized;
        parent.transform.position = Vector2.MoveTowards(parent.transform.position, parent.MStartPosition, parent.MSpeed * Time.deltaTime);
        float distance = Vector2.Distance(parent.MStartPosition, parent.transform.position);

        if (distance <= 0)
        {
            parent.ChangeState(new IdleState());
        }
        
    }
}

