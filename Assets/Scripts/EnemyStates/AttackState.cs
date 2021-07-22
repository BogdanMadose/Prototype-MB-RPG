using System.Collections;
using UnityEngine;

/// <summary>
/// Attack state Machine
/// </summary>
public class AttackState : IState
{
    private Enemy _parent;
    private float _attackCooldown = 3;
    private float _extraRange = 0.1f;

    public void Enter(Enemy parent)
    {
        this._parent = parent;
    }

    public void Exit()
    {

    }

    public void Update()
    {
        if (_parent.AttackTime >= _attackCooldown && !_parent.IsAttacking)
        {
            _parent.AttackTime = 0;
            _parent.StartCoroutine(Attack());
        }

        if (_parent.Target != null)
        {
            float distance = Vector2.Distance(_parent.Target.position, _parent.transform.position);

            if (distance >= _parent.AttackRange + _extraRange && !_parent.IsAttacking)
            {
                _parent.ChangeState(new FollowState());
            }
        }
        else
        {
            _parent.ChangeState(new IdleState());
        }
    }

    /// <summary>
    /// Attack function
    /// </summary>
    /// <returns>Waits for current animation exit time then stops attack animation</returns>
    public IEnumerator Attack()
    {
        _parent.IsAttacking = true;
        _parent.Animator.SetTrigger("attack");
        yield return new WaitForSeconds(_parent.Animator.GetCurrentAnimatorStateInfo(2).length);
        _parent.IsAttacking = false;
    }
}
