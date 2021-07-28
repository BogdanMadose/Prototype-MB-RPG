using UnityEngine;
using UnityEngine.UI;

public class LootButton : MonoBehaviour
{
    [SerializeField] private Image icon;
    [SerializeField] private Text title;

    public Image Icon => icon;
    public Text Title => title;
}
