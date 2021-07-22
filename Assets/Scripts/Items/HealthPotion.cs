using UnityEngine;

[CreateAssetMenu(fileName = "HealthPotion", menuName = "Items/Potion", order = 1)]
public class HealthPotion : Item, IUsable
{
    [SerializeField] private int health;
    public void Use()
    {
        if (Player.Instance.Health.CurrentValue < Player.Instance.Health.MaxValue)
        {
            Remove();
            Player.Instance.Health.CurrentValue += health;
        }
    }
}
