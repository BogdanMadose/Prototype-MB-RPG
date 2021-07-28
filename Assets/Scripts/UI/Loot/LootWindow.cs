using UnityEngine;

public class LootWindow : MonoBehaviour
{
    [SerializeField] private LootButton[] lootButtons;
    [SerializeField] private Item[] items; // TODO: Remove after testing

    // Start is called before the first frame update
    void Start()
    {
        AddLoot();
    }
    
    private void AddLoot()
    {
        lootButtons[0].Icon.sprite = items[0].Icon;
        lootButtons[0].gameObject.SetActive(true);
        string title = string.Format("<color={0}>{1}</color>", QualityColor.Colors[items[0].Quality], items[0].Title);
        lootButtons[0].Title.text = title;
    }
}
