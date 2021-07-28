using UnityEngine;

[CreateAssetMenu(fileName = "HealthPotion", menuName = "Items/Potion", order = 1)]
public class HealthPotion : Item, IUsable
{
    [Tooltip("Health restored")]
    [SerializeField] private int health;

    public void Use()
    {
        if (Player.Instance.Health.CurrentValue < Player.Instance.Health.MaxValue)
        {
            Remove();
            Player.Instance.Health.CurrentValue += health;
        }
    }

    public override string GetDescription()
    {
        return base.GetDescription() +
            string.Format("\n<color=#00ff00>Use to restore {0} health</color>", health);
    }
}
