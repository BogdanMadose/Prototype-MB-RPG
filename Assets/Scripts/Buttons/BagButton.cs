using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BagButton : MonoBehaviour, IPointerClickHandler
{
    [Tooltip("Sprites used for empty and full bag bar slots")]
    [SerializeField] private Sprite full, empty;
    private Bag _bag;

    public Bag Bag
    {
        get => _bag;
        set
        {
            GetComponent<Image>().sprite = value != null ? full : empty;
            _bag = value;
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            if (InventoryScript.Instance.FromSlot != null && HandScript.Instance.Movable != null && HandScript.Instance.Movable is Bag)
            {
                if (Bag != null)
                {
                    InventoryScript.Instance.SwapBagsFromBar(Bag, HandScript.Instance.Movable as Bag);
                }
                else
                {
                    Bag tmpBag = (Bag)HandScript.Instance.Movable;
                    tmpBag.BagButton = this;
                    tmpBag.Use();
                    Bag = tmpBag;
                    HandScript.Instance.DropItem();
                    InventoryScript.Instance.FromSlot = null;
                }
            }
            else if (Input.GetKey(KeyCode.LeftShift))
            {
                HandScript.Instance.TakeMovable(Bag);
            }
            else if (_bag != null)
            {
                _bag.BagScript.OpenClose();
            }
        }
    }

    /// <summary>
    /// Remove bag from bag slot bar
    /// </summary>
    public void RemoveBag()
    {
        InventoryScript.Instance.RemoveBagFromBar(Bag);
        Bag.BagButton = null;
        foreach (Item item in Bag.BagScript.GetItems())
        {
            InventoryScript.Instance.AddItemToInventory(item);
        }
        Bag = null;
    }
}
