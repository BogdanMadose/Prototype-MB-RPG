using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BagButton : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private Sprite full, empty;
    private Bag _bag;

    public Bag Bag
    {
        get => _bag;
        set
        {
            if (value != null)
            {
                GetComponent<Image>().sprite = full;
            }
            else
            {
                GetComponent<Image>().sprite = empty;
            }
            _bag = value;
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (_bag != null)
        {
            _bag.BagScript.OpenClose();
        }
    }
}
