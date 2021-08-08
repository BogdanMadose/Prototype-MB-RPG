using UnityEngine;

public class CharacterPannel : MonoBehaviour
{
    private static CharacterPannel _instance;
    public static CharacterPannel Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<CharacterPannel>();
            }
            return _instance;
        }
    }

    [Tooltip("Character pannel parent object")]
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private CharacterButton helmet, shoulders, chest, gloves, waist, boots, mainHand, offHand, twoHand;

    public CharacterButton SelectedButton { get; set; }

    public void OpenCloseCharacterPannel()
    {
        if (canvasGroup.alpha <= 0)
        {
            canvasGroup.blocksRaycasts = true;
            canvasGroup.alpha = 1;
        }
        else
        {
            canvasGroup.blocksRaycasts = false;
            canvasGroup.alpha = 0;
        }
    }

    /// <summary>
    /// Handles atuomatic character pannel slot designation
    /// </summary>
    /// <param name="equipment">Item to be equipped</param>
    public void DesignateSlot(Equipment equipment)
    {
        switch (equipment.ItemPlacement)
        {
            case ItemPlacement.Helmet:
                helmet.EquipItem(equipment);
                break;
            case ItemPlacement.Shoulders:
                shoulders.EquipItem(equipment);
                break;
            case ItemPlacement.Chest:
                chest.EquipItem(equipment);
                break;
            case ItemPlacement.Gloves:
                gloves.EquipItem(equipment);
                break;
            case ItemPlacement.Waist:
                waist.EquipItem(equipment);
                break;
            case ItemPlacement.Boots:
                boots.EquipItem(equipment);
                break;
            case ItemPlacement.MainHand:
                mainHand.EquipItem(equipment);
                break;
            case ItemPlacement.OffHand:
                offHand.EquipItem(equipment);
                break;
            case ItemPlacement.TwoHand:
                break;
        }
    }
}