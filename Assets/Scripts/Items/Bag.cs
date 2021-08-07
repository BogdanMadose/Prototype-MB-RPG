using UnityEngine;

[CreateAssetMenu(fileName = "Bag", menuName = "Items/Bag", order = 1)]
public class Bag : Item, IUsable
{
    [Tooltip("Bag prefab")]
    [SerializeField] private GameObject bagPrefab;
    [Tooltip("No. of slots")]
    [SerializeField] private int slots;

    public BagScript BagScript { get; set; }
    public BagButton BagButton { get; set; }
    public int Slots => slots;

    /// <summary>
    /// Initialize number of slots in a bag
    /// </summary>
    /// <param name="slots">Number of slots</param>
    public void Initialize(int slots) => this.slots = slots;

    public void Use()
    {
        if (!InventoryScript.Instance.IsFull)
        {
            Remove();
            BagScript = Instantiate(bagPrefab, InventoryScript.Instance.transform).GetComponent<BagScript>();
            BagScript.AddSlots(slots);
            if (BagButton == null)
            {
                InventoryScript.Instance.AddBagToBar(this);
            }
            else
            {
                InventoryScript.Instance.AddBagToBar(this, BagButton);
            }
        }
    }

    public override string GetDescription()
    {
        return base.GetDescription() +
            string.Format("\n<color=#00ff00>Use to gain an extra {0} slot bag</color>", slots);
    }
}
