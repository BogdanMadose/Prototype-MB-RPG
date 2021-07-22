using System.Collections.Generic;
using UnityEngine;

public class InventoryScript : MonoBehaviour
{
    private static InventoryScript _instance;
    public static InventoryScript Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<InventoryScript>();
            }
            return _instance;
        }
    }

    [SerializeField] private BagButton[] bagButtons;
    private List<Bag> _bags = new List<Bag>();

    public bool IsFull => !(_bags.Count < 5);

    //==============DEBUGGING=====================
    [SerializeField] private Item[] items;

    private void Awake()
    {
        Bag bag = (Bag)Instantiate(items[0]);
        bag.Initialize(20);
        bag.Use();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.J))
        {
            Bag bag = (Bag)Instantiate(items[0]);
            bag.Initialize(20);
            bag.Use();
        }
    }
    //============================================

    public void AddBag(Bag bag)
    {
        foreach(BagButton bagButton in bagButtons)
        {
            if (bagButton.Bag == null)
            {
                bagButton.Bag = bag;
                _bags.Add(bag);
                break;
            }
        }
    }

    public void OpenClose()
    {
        bool closedBag = _bags.Find(x => !x.BagScript.IsOpen);
        foreach (Bag bag in _bags)
        {
            if (bag.BagScript.IsOpen != closedBag)
            {
                bag.BagScript.OpenClose();
            }
        }
    }
}
