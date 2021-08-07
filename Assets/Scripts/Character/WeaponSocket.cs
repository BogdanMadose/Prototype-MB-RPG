using UnityEngine;

class WeaponSocket : EquipmentSocket
{
    [Tooltip("Player sprite reference")]
    [SerializeField] private SpriteRenderer playerSR;
    private float _y;

    public override void SetDirection(float x, float y)
    {
        base.SetDirection(x, y);
        if (_y != y)
        {
            if (y == 1)
            {
                _spriteRenderer.sortingOrder = playerSR.sortingOrder - 1;
            }
            else
            {
                _spriteRenderer.sortingOrder = playerSR.sortingOrder + 5;
            }
        }
    }
}

