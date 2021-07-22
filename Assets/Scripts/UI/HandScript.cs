using UnityEngine;
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

    [SerializeField] private Vector3 offset;
    private Image _icon;

    public IMovable Movable { get; set; }
    // Start is called before the first frame update
    void Start()
    {
        _icon = GetComponent<Image>();
    }

    private void Update()
    {
        _icon.transform.position = Input.mousePosition + offset;
    }

    public void TakeMovable(IMovable movable)
    {
        this.Movable = movable;
        _icon.sprite = movable.Icon;
        _icon.color = Color.white;
    }

    public IMovable Put()
    {
        IMovable tmp = Movable;
        Movable = null;
        _icon.color = new Color(0, 0, 0, 0);
        return tmp;
    }
}
