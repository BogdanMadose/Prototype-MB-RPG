using UnityEngine;
using UnityEngine.UI;

public class Stat : MonoBehaviour
{
    #region Variables
    /// <summary>
    /// Reference to the text component under image object
    /// </summary>
    [SerializeField] private Text statValue;

    /// <summary>
    /// Lerp speed for display purposes, can be changed in editor
    /// </summary>
    [SerializeField] private float lerpSpeed;

    /// <summary>
    /// Variable to hold the fill amount
    /// </summary>
    private float _currentFill;

    /// <summary>
    /// Variable to hold the current value (eg.: health, mana)
    /// </summary>
    private float _currentValue;

    /// <summary>
    /// Reference to the (mana, health bar ) image component in editor
    /// </summary>
    private Image _content;
    #endregion

    #region Properties
    /// <summary>
    /// Property to get and set the max allowed value (eg.: maxHealth, maxMana)
    /// </summary>
    public float MaxValue { get; set; }

    /// <summary>
    /// Property to get and set the current vallue of both current and fill values accordingly
    /// </summary>
    public float CurrentValue
    {
        get
        {
            return _currentValue;
        }
        set
        {
            if (value > MaxValue)
            {
                _currentValue = MaxValue;
            }
            else if (value < 0)
            {
                _currentValue = 0;
            }
            else
            {
                _currentValue = value;
            }

            _currentFill = _currentValue / MaxValue;

            if (statValue != null)
            {
                statValue.text = _currentValue + " / " + MaxValue;
            }
        }
    }
    #endregion

    void Start()
    {
        _content = GetComponent<Image>();
    }

    void Update()
    {
        // Lerp functionality for smooth UI graphic transitions
        if (_currentFill != _content.fillAmount)
        {
            _content.fillAmount = Mathf.Lerp(_content.fillAmount, _currentFill, Time.deltaTime * lerpSpeed);
        }
    }

    /// <summary>
    /// Initializes default character stats
    /// </summary>
    /// <param name="currentValue">Current value of x stat</param>
    /// <param name="maxValue">Maximum value of x stat</param>
    public void Initialize(float currentValue, float maxValue)
    {
        if (_content == null)
        {
            _content = GetComponent<Image>();
        }

        MaxValue = maxValue;
        CurrentValue = currentValue;
        _content.fillAmount = CurrentValue / MaxValue;
    }
}
