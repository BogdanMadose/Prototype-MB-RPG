using UnityEngine;
using UnityEngine.UI;

public class HandScript : MonoBehaviour
{
    private static HandScript instance;
    public static HandScript MInstance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<HandScript>();
            }
            return instance;
        }
    }

    private Image icon;
    [SerializeField] private Vector3 offset;
    public IMovable MMovable { get; set; }
    // Start is called before the first frame update
    void Start()
    {
        icon = GetComponent<Image>();
    }

    private void Update()
    {
        icon.transform.position = Input.mousePosition + offset;
    }

    public void TakeMovable(IMovable movable)
    {
        this.MMovable = movable;
        icon.sprite = movable.MIcon;
        icon.color = Color.white;
    }

    public IMovable Put()
    {
        IMovable tmp = MMovable;
        MMovable = null;
        icon.color = new Color(0, 0, 0, 0);
        return tmp;
    }
}
