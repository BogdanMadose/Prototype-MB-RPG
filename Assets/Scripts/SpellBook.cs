using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class SpellBook : MonoBehaviour
{
    [SerializeField] private Spell[] spells;
    [SerializeField] private Image castingBar;
    [SerializeField] private Text spellName;
    [SerializeField] private Image icon;
    [SerializeField] private Text castTime;
    [SerializeField] private CanvasGroup canvasGroup;
    private Coroutine spellRoutine;
    private Coroutine fadeRoutine;

    /// <summary>
    /// Handles casting of spells functionality
    /// </summary>
    /// <param name="spellIndex">Spell number</param>
    /// <returns>Type of spell at given number</returns>
    public Spell CastSpell(int spellIndex)
    {
        spellName.text = spells[spellIndex].MName;
        castingBar.color = spells[spellIndex].MBarColor;
        castingBar.fillAmount = 0;
        icon.sprite = spells[spellIndex].MIcon;
        spellRoutine = StartCoroutine(BarProgress(spellIndex));
        fadeRoutine = StartCoroutine(FadeBar());

        return spells[spellIndex];
    }

    /// <summary>
    /// Handles cast time bar fill amount progress
    /// </summary>
    /// <param name="spellIndex">Spell number</param>
    /// <returns>null - instant</returns>
    private IEnumerator BarProgress(int spellIndex)
    {
        float timePassed = Time.deltaTime;
        float rate = 1.0f / spells[spellIndex].MCastTime;
        float progress = 0.0f;

        while (progress <= 1.0f)
        {
            castingBar.fillAmount = Mathf.Lerp(0, 1, progress);
            progress += rate * Time.deltaTime;
            timePassed += Time.deltaTime;
            castTime.text = (spells[spellIndex].MCastTime - timePassed).ToString("F2");

            if (spells[spellIndex].MCastTime - timePassed < 0)
            {
                castTime.text = "0.00";
            }

            yield return null;
        }

        StopCasting();
    }

    /// <summary>
    /// Casting bar fade in function
    /// </summary>
    /// <returns>null - instant</returns>
    private IEnumerator FadeBar()
    {
        float rate = 1.0f / 0.25f;
        float progress = 0.0f;

        while (progress <= 1.0f)
        {
            canvasGroup.alpha = Mathf.Lerp(0, 1, progress);
            progress += rate * Time.deltaTime;
            yield return null;
        }
    }

    /// <summary>
    /// Hides casting bar when finished casting, or interrupted
    /// </summary>
    public void StopCasting()
    {
        if (fadeRoutine != null)
        {
            canvasGroup.alpha = 0;
            StopCoroutine(fadeRoutine);
            fadeRoutine = null;
        }
        if (spellRoutine != null)
        {
            StopCoroutine(spellRoutine);
            spellRoutine = null;
        }
    }
}
