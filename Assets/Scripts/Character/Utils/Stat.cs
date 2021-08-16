using UnityEngine;
using UnityEngine.UI;

public class Stat : MonoBehaviour
{
    [Tooltip("Stat value text under image")]
    [SerializeField] private Text statValue;
    [Tooltip("Prefferd lerp speed")]
    [SerializeField] private float lerpSpeed;
    private float _currentFill;
    private float _currentValue;
    private float _xpOverflow;
    private Image _content;

    public bool IsFull
    {
        get
        {
            return _content.fillAmount == 1;
        }
    }
    public float MaxValue { get; set; }
    public float CurrentValue
    {
        get => _currentValue;
        set
        {
            if (value > MaxValue)
            {
                _xpOverflow = value - MaxValue;
                _currentValue = MaxValue;
            }
            else
            {
                _currentValue = value < 0 ? 0 : value;
            }
            _currentFill = _currentValue / MaxValue;
            if (statValue != null)
            {
                statValue.text = _currentValue + " / " + MaxValue;
            }
        }
    }
    public float XPOverflow
    {
        get
        {
            float tmp = _xpOverflow;
            _xpOverflow = 0;
            return tmp;
        }
    }

    void Start() => _content = GetComponent<Image>();

    void Update() => FillBar();

    /// <summary>
    /// Fill stat bar
    /// </summary>
    private void FillBar()
    {
        if (_currentFill != _content.fillAmount)
        {
            _content.fillAmount = Mathf.MoveTowards(_content.fillAmount, _currentFill, Time.deltaTime * lerpSpeed);
        }
    }

    /// <summary>
    /// Initialize default character stats
    /// </summary>
    /// <param name="currentValue">Current value of [stat]</param>
    /// <param name="maxValue">Maximum value of x [stat]</param>
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

    public void Reset()
    {
        _content.fillAmount = 0;
    }
}
