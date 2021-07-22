using System.Collections.Generic;
using UnityEngine;

public class BagScript : MonoBehaviour
{
    [SerializeField] private GameObject slotPrefab;
    private CanvasGroup _canvasGroup;
    private List<SlotScript> _slots = new List<SlotScript>();

    public bool IsOpen => _canvasGroup.alpha > 0;

    public List<SlotScript> Slots => _slots;

    private void Awake() => _canvasGroup = GetComponent<CanvasGroup>();

    public void AddSlots(int slotCount)
    {
        for (int i = 0; i < slotCount; i++)
        {
            SlotScript slot = Instantiate(slotPrefab, transform).GetComponent<SlotScript>();
            Slots.Add(slot);
        }
    }

    public bool AddItemToBag(Item item)
    {
        foreach (SlotScript slot in Slots)
        {
            if (slot.IsEmpty)
            {
                slot.AddItemToSlot(item);
                return true;
            }
        }

        return false;
    }

    public void OpenClose()
    {
        _canvasGroup.alpha = _canvasGroup.alpha > 0 ? 0 : 1;
        _canvasGroup.blocksRaycasts = _canvasGroup.blocksRaycasts == true ? false : true;
    }
}
