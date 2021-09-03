using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour, IInteractable
{
    [Tooltip("Chest renderer")]
    [SerializeField] private SpriteRenderer chestSR;
    [Tooltip("Chest opened sprite")]
    [SerializeField] private Sprite chestOpened;
    [Tooltip("Chest closed sprite")]
    [SerializeField] private Sprite chestClosed;
    [Tooltip("Chest UI object")]
    [SerializeField] private CanvasGroup canvasGroup;
    [Tooltip("BagScript reference in chest script")]
    [SerializeField] private BagScript bagScriptC;
    private List<Item> _items;
    private bool _isOpened;

    public List<Item> Items { get => _items; set => _items = value; }
    public BagScript BagScriptC { get => bagScriptC; set => bagScriptC = value; }
    private void Awake()
    {
        _items = new List<Item>();
    }

    public void Interact()
    {
        if (_isOpened)
        {
            StopInteracting();
        }
        else
        {
            AddItemsToChest();
            _isOpened = true;
            chestSR.sprite = chestOpened;
            canvasGroup.alpha = 1;
            canvasGroup.blocksRaycasts = true;
        }
    }

    public void StopInteracting()
    {
        if (_isOpened)
        {
            StoreItemsInChest();
        }
        BagScriptC.ClearBag();
        _isOpened = false;
        chestSR.sprite = chestClosed;
        canvasGroup.alpha = 0;
        canvasGroup.blocksRaycasts = false;
    }

    /// <summary>
    /// Show items that are in the chest
    /// </summary>
    public void AddItemsToChest()
    {
        if (Items != null)
        {
            foreach (Item item in Items)
            {
                item.Slot.AddItemToSlot(item);
            }
        }
    }

    /// <summary>
    /// Store items into chest
    /// </summary>
    public void StoreItemsInChest()
    {
        Items = BagScriptC.GetItems();
    }
}
