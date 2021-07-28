using UnityEngine;

[CreateAssetMenu(fileName = "Armors", menuName = "Items/Armors", order = 2)]
public class Armors : Item
{
    [Tooltip("Armor placement Type")]
    [SerializeField] private ArmorType armorType;
    [SerializeField] private int intellect;
    [SerializeField] private int strength;
    [SerializeField] private int dexterity;

    public override string GetDescription()
    {
        string stats = string.Empty;
        if (intellect > 0)
        {
            stats += string.Format("<color=#00ff00>\n +{0} INT</color>", intellect);
        }
        if (strength > 0)
        {
            stats += string.Format("<color=#00ff00>\n +{0} STR</color>", strength);
        }
        if (dexterity > 0)
        {
            stats += string.Format("<color=#00ff00>\n +{0} DEX</color>", dexterity);
        }
        return base.GetDescription() + stats;
    }
}
