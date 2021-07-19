using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Stat : MonoBehaviour
{
    #region Variables
    /// <summary>
    /// Reference to the (mana, health bar ) image component in editor
    /// </summary>
    private Image content;

    /// <summary>
    /// Reference to the text component under image object
    /// </summary>
    [SerializeField] private Text statValue;

    /// <summary>
    /// Variable to hold the fill amount
    /// </summary>
    private float currentFill;

    /// <summary>
    /// Variable to hold the current value (eg.: health, mana)
    /// </summary>
    private float currentValue;

    /// <summary>
    /// Lerp speed for display purposes, can be changed in editor
    /// </summary>
    [SerializeField] private float lerpSpeed;
    #endregion

    #region Properties
    /// <summary>
    /// Property to get and set the max allowed value (eg.: maxHealth, maxMana)
    /// </summary>
    public float MMaxValue { get; set; }

    /// <summary>
    /// Property to get and set the current vallue of both current and fill values accordingly
    /// </summary>
    public float MCurrentValue
    {
        get
        {
            return currentValue;
        }
        set
        {
            if (value > MMaxValue)
            {
                currentValue = MMaxValue;
            }
            else if (value < 0)
            {
                currentValue = 0;
            }
            else
            {
                currentValue = value;
            }

            currentFill = currentValue / MMaxValue;

            if (statValue != null)
            {
                statValue.text = currentValue + " / " + MMaxValue;
            }        
        }
    } 
    #endregion

    void Start()
    {
        content = GetComponent<Image>();    
    }

    void Update()
    {
        // Lerp functionality for smooth UI graphic transitions
        if (currentFill != content.fillAmount)
        {
            content.fillAmount = Mathf.Lerp(content.fillAmount, currentFill, Time.deltaTime * lerpSpeed);
        }
    }

    /// <summary>
    /// Initializes default character stats
    /// </summary>
    /// <param name="currentValue">Current value of x stat</param>
    /// <param name="maxValue">Maximum value of x stat</param>
    public void Initialize(float currentValue, float maxValue)
    {
        MMaxValue = maxValue;
        MCurrentValue = currentValue;
    }
}
