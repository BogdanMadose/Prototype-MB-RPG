using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CharacterButton : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    [Tooltip("Type of equipped item")]
    [SerializeField] private ItemPlacement itemPlacement;
    [Tooltip("Icon of equipped item")]
    [SerializeField] private Image icon;
    [Tooltip("Specific equipment socket animation reference")]
    [SerializeField] private EquipmentSocket equipmentSocket;
    private Equipment _equipment;

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            if (HandScript.Instance.Movable is Equipment)
            {
                Equipment tmp = (Equipment)HandScript.Instance.Movable;
                if (tmp.ItemPlacement == itemPlacement)
                {
                    EquipItem(tmp);
                }
                UIManager.Instance.RefreshToolTip(tmp);
            }
            else if (HandScript.Instance.Movable == null && _equipment != null)
            {
                HandScript.Instance.TakeMovable(_equipment);
                CharacterPannel.Instance.SelectedButton = this;
                icon.color = Color.grey;
            }
        }
    }

    /// <summary>
    /// Handles adding item sprites and stats to the character pannel slots
    /// </summary>
    /// <param name="equipment">Item to be equipped</param>
    public void EquipItem(Equipment equipment)
    {
        equipment.Remove();
        if (_equipment != null)
        {
            if (_equipment != equipment)
            {
                equipment.Slot.AddItemToSlot(_equipment);
            }
            UIManager.Instance.RefreshToolTip(_equipment);
        }
        else
        {
            UIManager.Instance.HideToolTip();
        }
        icon.enabled = true;
        icon.sprite = equipment.Icon;
        icon.color = Color.white;
        this._equipment = equipment;
        if (HandScript.Instance.Movable == (equipment as IMovable))
        {
            HandScript.Instance.DropItem();
        }
        if (equipmentSocket != null && _equipment.AnimationClips != null)
        {
            equipmentSocket.AnimateEquip(_equipment.AnimationClips);
        }
    }

    /// <summary>
    /// Handles removing item sprites and stats from the character pannel slots
    /// </summary>
    public void DequipItem()
    {
        icon.color = Color.white;
        icon.enabled = false;
        if (equipmentSocket != null && _equipment.AnimationClips != null)
        {
            equipmentSocket.AnimateDequip();
        }
        _equipment = null;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (_equipment != null)
        {
            UIManager.Instance.ShowToolTip(new Vector2(0, 0.5f), transform.position, _equipment);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        UIManager.Instance.HideToolTip();
    }
}
