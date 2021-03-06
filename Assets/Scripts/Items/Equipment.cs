using UnityEngine;

[CreateAssetMenu(fileName = "Equipment", menuName = "Items/Equipment", order = 2)]
public class Equipment : Item
{
    [Tooltip("Item placement Type")]
    [SerializeField] private ItemPlacement itemPlacement;
    [SerializeField] private int intellect;
    [SerializeField] private int strength;
    [SerializeField] private int dexterity;
    [SerializeField] private AnimationClip[] animationClips;

    public ItemPlacement ItemPlacement => itemPlacement;
    public AnimationClip[] AnimationClips => animationClips;

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

    /// <summary>
    /// Equip item
    /// </summary>
    public void Equip()
    {
        CharacterPannel.Instance.DesignateSlot(this);
    }
}
