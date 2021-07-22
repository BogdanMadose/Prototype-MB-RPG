using UnityEngine;

public class EvadeState : IState
{
    private Enemy _parent;

    public void Enter(Enemy parent) => this._parent = parent;

    public void Exit()
    {
        _parent.Direction = Vector2.zero;
        _parent.Reset();
    }

    public void Update()
    {
        _parent.Direction = (_parent.StartPosition - _parent.transform.position).normalized;
        _parent.transform.position = Vector2.MoveTowards(_parent.transform.position, _parent.StartPosition, _parent.Speed * Time.deltaTime);
        float distance = Vector2.Distance(_parent.StartPosition, _parent.transform.position);

        if (distance <= 0)
        {
            _parent.ChangeState(new IdleState());
        }

    }
}

