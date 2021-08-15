class IdleState : IState
{
    private Enemy _parent;
    public void Enter(Enemy parent)
    {
        this._parent = parent;
        this._parent.Reset();
    }

    public void Update()
    {
        if (_parent.Target != null)
        {
            _parent.ChangeState(new FollowState());
        }
    }

    public void Exit() { }
}
