using System;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    private static UIManager _instance;
    public static UIManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<UIManager>();
            }
            return _instance;
        }
    }

    [Tooltip("Targeted NPC portratit frame UI object")]
    [SerializeField] private GameObject targetFrame;
    [Tooltip("Number of action buttons")]
    [SerializeField] private ActionButton[] actionButtons;
    [Tooltip("Portrait image sprite")]
    [SerializeField] private Image portraitFrame;
    [Tooltip("Keybinding menu UI object")]
    [SerializeField] private CanvasGroup keyBindMenu;
    [Tooltip("Spellbook menu UI object")]
    [SerializeField] private CanvasGroup spellBook;
    [Tooltip("Tooltip UI object")]
    [SerializeField] private GameObject toolTip;
    [Tooltip("Character pannel UI object")]
    [SerializeField] private CharacterPannel _characterPannel;
    [Tooltip("Tooltip pivot point reference")]
    [SerializeField] private RectTransform tooltipRect;
    [Tooltip("Target level text disply")]
    [SerializeField] private Text levelText;
    private Text _toolTipText;
    private GameObject[] _keyBindButtons;
    private Stat _healthStat;


    private void Awake()
    {
        _keyBindButtons = GameObject.FindGameObjectsWithTag("Keybind");
        _toolTipText = toolTip.GetComponentInChildren<Text>();
    }

    // Start is called before the first frame update
    void Start() => _healthStat = targetFrame.GetComponentInChildren<Stat>();

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.O))
        {
            OpenCloseMenuUIItem(keyBindMenu);
        }
        if (Input.GetKeyDown(KeyCode.K))
        {
            OpenCloseMenuUIItem(spellBook);
        }
        if (Input.GetKeyDown(KeyCode.I))
        {
            InventoryScript.Instance.OpenCloseInventory();
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            _characterPannel.OpenCloseCharacterPannel();
        }
    }

    /// <summary>
    /// Show target frame
    /// </summary>
    /// <param name="target">Selected NPC</param>
    public void ShowTargetFrame(Enemy target)
    {
        targetFrame.SetActive(true);
        _healthStat.Initialize(target.Health.CurrentValue, target.Health.MaxValue);
        portraitFrame.sprite = target.Portrait;
        levelText.text = target.Level.ToString();
        target.healthChangedEvent += new HealthChanged(UpdateTargetFrame);
        target.npcRemovedEvent += new NPCRemoved(HideTargetFrame);
        levelText.color = target.Level >= Player.Instance.Level + 5
            ? Color.red
            : target.Level == Player.Instance.Level + 3 || target.Level == Player.Instance.Level + 4
                ? (Color)new Color32(255, 124, 0, 255)
                : target.Level >= Player.Instance.Level - 2 && target.Level <= Player.Instance.Level + 2
                            ? Color.yellow
                            : target.Level <= Player.Instance.Level - 3 && target.Level > XPManager.CalulateGrayLevel() ? Color.green : Color.grey;
    }

    /// <summary>
    /// Hide target frame
    /// </summary>
    public void HideTargetFrame() => targetFrame.SetActive(false);

    /// <summary>
    /// Update target health
    /// </summary>
    /// <param name="health">Health value</param>
    public void UpdateTargetFrame(float health) => _healthStat.CurrentValue = health;

    /// <summary>
    /// Update key-bind menu text
    /// </summary>
    /// <param name="key">New key</param>
    /// <param name="keyCode">New keyboard Key</param>
    public void UpdateKeyText(string key, KeyCode keyCode)
    {
        Text tmp = Array.Find(_keyBindButtons, x => x.name == key).GetComponentInChildren<Text>();
        tmp.text = keyCode.ToString();
    }

    /// <summary>
    /// Click / Use Action buttons
    /// </summary>
    /// <param name="buttonName">Action button</param>
    public void UseActionButton(string buttonName) => Array.Find(actionButtons, x => x.gameObject.name == buttonName).Button.onClick.Invoke();

    /// <summary>
    /// Open / Close Menu UI
    /// </summary>
    /// <param name="canvasGroup">Desired menu object</param>
    public void OpenCloseMenuUIItem(CanvasGroup canvasGroup)
    {
        canvasGroup.alpha = canvasGroup.alpha > 0 ? 0 : 1;
        canvasGroup.blocksRaycasts = canvasGroup.blocksRaycasts != true;
    }

    /// <summary>
    /// Update Stack
    /// </summary>
    /// <param name="clickable">Stackable item</param>
    public void UpdateStackSize(IClickable clickable)
    {
        if (clickable.Count > 1)
        {
            clickable.StackText.text = clickable.Count.ToString();
            clickable.StackText.color = Color.white;
            clickable.Icon.color = Color.white;
        }
        else
        {
            ClearStackSizeCount(clickable);
        }
        if (clickable.Count == 0)
        {
            clickable.Icon.color = new Color(0, 0, 0, 0);
            clickable.StackText.color = new Color(0, 0, 0, 0);
        }
    }

    /// <summary>
    /// Reset stack
    /// </summary>
    /// <param name="clickable">Stackable item</param>
    public void ClearStackSizeCount(IClickable clickable)
    {
        clickable.StackText.color = new Color(0, 0, 0, 0);
        clickable.Icon.color = Color.white;
    }

    /// <summary>
    /// Display item tooltips
    /// </summary>
    /// <param name="pivot">Pivot point</param>
    /// <param name="position">Position</param>
    /// <param name="description">Item description</param>
    public void ShowToolTip(Vector2 pivot, Vector3 position, IDescribable description)
    {
        tooltipRect.pivot = pivot;
        toolTip.SetActive(true);
        toolTip.transform.position = position;
        _toolTipText.text = description.GetDescription();
    }

    /// <summary>
    /// Hide Tooltips
    /// </summary>
    public void HideToolTip() => toolTip.SetActive(false);

    /// <summary>
    /// Refresh tooltip (Eg.: on item swaps)
    /// </summary>
    /// <param name="describable">New item description</param>
    public void RefreshToolTip(IDescribable describable) => _toolTipText.text = describable.GetDescription();
}