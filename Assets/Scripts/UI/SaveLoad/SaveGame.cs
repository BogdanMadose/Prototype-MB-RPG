using UnityEngine;
using UnityEngine.UI;

public class SaveGame : MonoBehaviour
{
    [Tooltip("Save slot number")]
    [SerializeField] private int index;
    [Tooltip("Time and date of save file")]
    [SerializeField] private Text dateTime;
    [Tooltip("Health bar")]
    [SerializeField] private Image health;
    [Tooltip("Mana bar")]
    [SerializeField] private Image mana;
    [Tooltip("XP bar")]
    [SerializeField] private Image xp;
    [Tooltip("Health value")]
    [SerializeField] private Text healthText;
    [Tooltip("Mana value")]
    [SerializeField] private Text manaText;
    [Tooltip("XP value")]
    [SerializeField] private Text xpText;
    [Tooltip("Level value")]
    [SerializeField] private Text levelText;
    [Tooltip("Save slot game object")]
    [SerializeField] private GameObject visuals;

    public int Index => index;

    private void Awake()
    {
        visuals.SetActive(false);
    }

    /// <summary>
    /// Show save game information
    /// </summary>
    /// <param name="data">Save game data</param>
    public void ShowInfo(SaveData data)
    {
        visuals.SetActive(true);
        dateTime.text = "Date: " + data.DateTime.ToString("dd/MM/yyyy") + " - Time: " + data.DateTime.ToString("H:mm");
        health.fillAmount = data.PlayerData.Health / data.PlayerData.MaxHealth;
        healthText.text = data.PlayerData.Health + " / " + data.PlayerData.MaxHealth;
        mana.fillAmount = data.PlayerData.Mana / data.PlayerData.MaxMana;
        manaText.text = data.PlayerData.Mana + " / " + data.PlayerData.MaxMana;
        xp.fillAmount = data.PlayerData.XP / data.PlayerData.MaxXP;
        xpText.text = data.PlayerData.XP + " / " + data.PlayerData.MaxXP;
        levelText.text = data.PlayerData.Level.ToString();
    }

    /// <summary>
    /// Hide save game slot
    /// </summary>
    public void HideVisuals()
    {
        visuals.SetActive(false);
    }
}
