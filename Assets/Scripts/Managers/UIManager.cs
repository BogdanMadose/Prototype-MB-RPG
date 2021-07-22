using System;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject targetFrame;
    [SerializeField] private ActionButton[] actionButtons;
    [SerializeField] private Image portraitFrame;
    [SerializeField] private CanvasGroup KeyBindMenu;
    private GameObject[] keyBindButtons;
    private Stat healthStat;

    /// <summary>
    /// Singleton instance for UIManager
    /// </summary>
    private static UIManager instance;
    public static UIManager MInstance 
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<UIManager>();
            }
            return instance;
        }
    }

    private void Awake()
    {
        keyBindButtons = GameObject.FindGameObjectsWithTag("Keybind");
    }

    // Start is called before the first frame update
    void Start()
    {
        healthStat = targetFrame.GetComponentInChildren<Stat>();
        SetUsable(actionButtons[0], SpellBook.MInstance.GetSpell("Fireball"));
        SetUsable(actionButtons[1], SpellBook.MInstance.GetSpell("Frostbolt"));
        SetUsable(actionButtons[2], SpellBook.MInstance.GetSpell("Lightningbolt"));
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            OpenCloseMenu();
        }
    }

    /// <summary>
    /// Shows portrait frame of target
    /// </summary>
    /// <param name="target">Selected NPC</param>
    public void ShowTargetFrame(NPC target)
    {
        targetFrame.SetActive(true);
        healthStat.Initialize(target.MHealth.MCurrentValue, target.MHealth.MMaxValue);
        portraitFrame.sprite = target.MPortrait;
        target.healthChanged += new HealthChanged(UpdateTargetFrame);
        target.npcRemoved += new NPCRemoved(HideTargetFrame);
    }

    /// <summary>
    /// Hides portrait frame of target
    /// </summary>
    public void HideTargetFrame()
    {
        targetFrame.SetActive(false);
    }

    /// <summary>
    /// Update portrait health text of target
    /// </summary>
    /// <param name="health">Health value</param>
    public void UpdateTargetFrame(float health)
    {
        healthStat.MCurrentValue = health;
    }

    public void OpenCloseMenu()
    {
        KeyBindMenu.alpha = KeyBindMenu.alpha > 0 ? 0 : 1;
        KeyBindMenu.blocksRaycasts = KeyBindMenu.blocksRaycasts == true ? false : true;
        Time.timeScale = Time.timeScale > 0 ? 0 : 1;
    }

    public void UpdateKeyText(string key, KeyCode keyCode)
    {
        Text tmp = Array.Find(keyBindButtons, x => x.name == key).GetComponentInChildren<Text>();
        tmp.text = keyCode.ToString();
    }

    public void UseActionButton(string buttonName)
    {
        Array.Find(actionButtons, x => x.gameObject.name == buttonName).MButton.onClick.Invoke();
    }

    public void SetUsable(ActionButton btn, IUsable usable)
    {
        btn.MButton.image.sprite = usable.MIcon;
        btn.MButton.image.color = Color.white;
        btn.MUsable = usable;
    }
}
