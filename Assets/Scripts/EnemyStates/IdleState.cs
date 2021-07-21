﻿
/// <summary>
/// Idle State Machine
/// </summary>
class IdleState : IState
{
    private Enemy parent;
    public void Enter(Enemy parent)
    {
        this.parent = parent;
    }

    public void Update()
    {
        if (parent.MTarget != null)
        {
            parent.ChangeState(new FollowState());
        }
    } 

   public void Exit()
    {

    }
}
