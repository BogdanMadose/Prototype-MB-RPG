using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class HandScript : MonoBehaviour
{
    private static HandScript _instance;
    public static HandScript Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<HandScript>();
            }
            return _instance;
        }
    }

    [Tooltip("Desired icon display offset")]
    [SerializeField] private Vector3 offset;
    private Image _icon;

    public IMovable Movable { get; set; }
    // Start is called before the first frame update
    void Start() => _icon = GetComponent<Image>();

    private void Update()
    {
        _icon.transform.position = Input.mousePosition + offset;
        if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject() && Instance.Movable != null)
        {
            DeleteItem();
        }
    }

    /// <summary>
    /// Handles picking up the object
    /// </summary>
    /// <param name="movable">Object to be moved</param>
    public void TakeMovable(IMovable movable)
    {
        this.Movable = movable;
        _icon.sprite = movable.Icon;
        _icon.color = Color.white;
    }

    /// <summary>
    /// Handlles diplaying object on pointer location ( drag simulation )
    /// </summary>
    public IMovable PutItem()
    {
        IMovable tmp = Movable;
        Movable = null;
        _icon.color = new Color(0, 0, 0, 0);
        return tmp;
    }

    /// <summary>
    /// Handles placing of object
    /// </summary>
    public void DropItem()
    {
        Movable = null;
        _icon.color = new Color(0, 0, 0, 0);
        InventoryScript.Instance.FromSlot = null;
    }

    /// <summary>
    /// Handles deleting / destroying the picked up object if outside of UI
    /// </summary>
    public void DeleteItem()
    {
        if (Movable is Item)
        {
            Item item = (Item)Movable;
            if (item.Slot != null)
            {
                item.Slot.TrashItems();
            }
            else if (item.CharacterButton != null)
            {
                item.CharacterButton.DequipItem();
            }
        }
        DropItem();
        InventoryScript.Instance.FromSlot = null;
    }
}