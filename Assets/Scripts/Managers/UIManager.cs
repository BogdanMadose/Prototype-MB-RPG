using System;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{   
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

    [SerializeField] private GameObject targetFrame;
    [SerializeField] private ActionButton[] actionButtons;
    [SerializeField] private Image portraitFrame;
    [SerializeField] private CanvasGroup keyBindMenu;
    [SerializeField] private CanvasGroup spellBook;
    private GameObject[] keyBindButtons;
    private Stat healthStat;

    private void Awake()
    {
        keyBindButtons = GameObject.FindGameObjectsWithTag("Keybind");
    }

    // Start is called before the first frame update
    void Start()
    {
        healthStat = targetFrame.GetComponentInChildren<Stat>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            OpenClose(keyBindMenu);
        }
        if (Input.GetKeyDown(KeyCode.K))
        {
            OpenClose(spellBook);
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

    public void UpdateKeyText(string key, KeyCode keyCode)
    {
        Text tmp = Array.Find(keyBindButtons, x => x.name == key).GetComponentInChildren<Text>();
        tmp.text = keyCode.ToString();
    }

    public void UseActionButton(string buttonName)
    {
        Array.Find(actionButtons, x => x.gameObject.name == buttonName).MButton.onClick.Invoke();
    }

    public void OpenClose(CanvasGroup canvasGroup)
    {
        canvasGroup.alpha = canvasGroup.alpha > 0 ? 0 : 1;
        canvasGroup.blocksRaycasts = canvasGroup.blocksRaycasts == true ? false : true;
    }
}
