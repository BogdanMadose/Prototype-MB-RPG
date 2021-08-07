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
        if (Input.GetKeyDown(KeyCode.Escape))
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
    /// Shows portrait frame of target
    /// </summary>
    /// <param name="target">Selected NPC</param>
    public void ShowTargetFrame(NPC target)
    {
        targetFrame.SetActive(true);
        _healthStat.Initialize(target.Health.CurrentValue, target.Health.MaxValue);
        portraitFrame.sprite = target.Portrait;
        target.healthChangedEvent += new HealthChanged(UpdateTargetFrame);
        target.npcRemovedEvent += new NPCRemoved(HideTargetFrame);
    }

    /// <summary>
    /// Hides portrait frame of target
    /// </summary>
    public void HideTargetFrame() => targetFrame.SetActive(false);

    /// <summary>
    /// Update portrait health text of target
    /// </summary>
    /// <param name="health">Health value</param>
    public void UpdateTargetFrame(float health) => _healthStat.CurrentValue = health;

    /// <summary>
    /// Handles updating the text in keybinding menu
    /// </summary>
    /// <param name="key">New key</param>
    /// <param name="keyCode">New keyboard Key</param>
    public void UpdateKeyText(string key, KeyCode keyCode)
    {
        Text tmp = Array.Find(_keyBindButtons, x => x.name == key).GetComponentInChildren<Text>();
        tmp.text = keyCode.ToString();
    }

    /// <summary>
    /// Handles clicking / using action bar buttons
    /// </summary>
    /// <param name="buttonName">Name of the action button</param>
    public void UseActionButton(string buttonName) => Array.Find(actionButtons, x => x.gameObject.name == buttonName).Button.onClick.Invoke();

    /// <summary>
    /// Handles the display of Keybind and Spellbook menu objects
    /// </summary>
    /// <param name="canvasGroup">Desired menu object</param>
    public void OpenCloseMenuUIItem(CanvasGroup canvasGroup)
    {
        canvasGroup.alpha = canvasGroup.alpha > 0 ? 0 : 1;
        canvasGroup.blocksRaycasts = canvasGroup.blocksRaycasts != true;
    }

    /// <summary>
    /// Update stack size functionality
    /// </summary>
    /// <param name="clickable">Object in stack</param>
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
    /// Clear stack size functionality
    /// </summary>
    /// <param name="clickable">Object in stack</param>
    public void ClearStackSizeCount(IClickable clickable)
    {
        clickable.StackText.color = new Color(0, 0, 0, 0);
        clickable.Icon.color = Color.white;
    }

    /// <summary>
    /// Handles showing tooltips
    /// </summary>
    /// <param name="pivot">Pivot point of tooltip</param>
    /// <param name="position">Position of tooltip</param>
    /// <param name="description">Description that needs to be displayed</param>
    public void ShowToolTip(Vector2 pivot, Vector3 position, IDescribable description)
    {
        tooltipRect.pivot = pivot;
        toolTip.SetActive(true);
        toolTip.transform.position = position;
        _toolTipText.text = description.GetDescription();
    }

    /// <summary>
    /// Handles hiding tooltips
    /// </summary>
    public void HideToolTip()
    {
        toolTip.SetActive(false);
    }

    /// <summary>
    /// Handles refreshing tooltips (Eg.: on item swaps)
    /// </summary>
    /// <param name="describable">New description that needs to be displayed</param>
    public void RefreshToolTip(IDescribable describable)
    {
        _toolTipText.text = describable.GetDescription();
    }
}