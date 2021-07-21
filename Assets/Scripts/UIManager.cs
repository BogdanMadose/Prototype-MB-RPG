using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject targetFrame;
    [SerializeField] private Button[] actionButtons;
    [SerializeField] private Image portraitFrame;
    private KeyCode action1, action2, action3;
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

    // Start is called before the first frame update
    void Start()
    {
        healthStat = targetFrame.GetComponentInChildren<Stat>();

        action1 = KeyCode.Alpha1;
        action2 = KeyCode.Alpha2;
        action3 = KeyCode.Alpha3;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(action1))
        {
            ActionButtonOnClick(0);
        }
        if (Input.GetKeyDown(action2))
        {
            ActionButtonOnClick(1);
        }
        if (Input.GetKeyDown(action3))
        {
            ActionButtonOnClick(2);
        }
    }

    /// <summary>
    /// Invoke onClick event for each key pressed
    /// </summary>
    /// <param name="buttonIndex">Key number</param>
    private void ActionButtonOnClick(int buttonIndex)
    {
        actionButtons[buttonIndex].onClick.Invoke();
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
}
