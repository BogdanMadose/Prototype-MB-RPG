using UnityEngine;

[CreateAssetMenu(fileName = "Bag", menuName = "Items/Bag", order = 1)]
public class Bag : Item, IUsable
{
    [SerializeField] private GameObject bagPrefab;
    private int _slots;

    public BagScript BagScript { get; set; }
    public int Slots => _slots;

    public void Initialize(int slots) => this._slots = slots;

    public void Use()
    {
        if (!InventoryScript.Instance.IsFull)
        {
            Remove();
            BagScript = Instantiate(bagPrefab, InventoryScript.Instance.transform).GetComponent<BagScript>();
            BagScript.AddSlots(_slots);
            InventoryScript.Instance.AddBag(this);
        }
    }
}
