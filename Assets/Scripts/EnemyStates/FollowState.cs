using UnityEngine;

/// <summary>
/// Follow State Machine
/// </summary>
class FollowState : IState
{
    private Enemy _parent;

    public void Enter(Enemy parent)
    {
        this._parent = parent;
    }

    public void Exit()
    {
        _parent.Direction = Vector2.zero;
    }

    public void Update()
    {
        if (_parent.Target != null)
        {
            _parent.Direction = (_parent.Target.transform.position - _parent.transform.position).normalized;
            _parent.transform.position = Vector2.MoveTowards(_parent.transform.position, _parent.Target.position, _parent.Speed * Time.deltaTime);
            float distance = Vector2.Distance(_parent.Target.position, _parent.transform.position);

            if (distance <= _parent.AttackRange)
            {
                _parent.ChangeState(new AttackState());
            }
        }
        if (!_parent.InRange)
        {
            _parent.ChangeState(new EvadeState());
        }
    }
}

