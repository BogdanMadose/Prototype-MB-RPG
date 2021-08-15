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
    private Image _content;

    public float MaxValue { get; set; }

    public float CurrentValue
    {
        get => _currentValue;
        set
        {
            _currentValue = value > MaxValue ? MaxValue : value < 0 ? 0 : value;

            _currentFill = _currentValue / MaxValue;

            if (statValue != null)
            {
                statValue.text = _currentValue + " / " + MaxValue;
            }
        }
    }

    void Start() => _content = GetComponent<Image>();

    void Update()
    {
        if (_currentFill != _content.fillAmount)
        {
            _content.fillAmount = Mathf.Lerp(_content.fillAmount, _currentFill, Time.deltaTime * lerpSpeed);
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
}
