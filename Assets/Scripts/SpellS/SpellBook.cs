using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class SpellBook : MonoBehaviour
{
    private static SpellBook _instance;
    public static SpellBook Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<SpellBook>();
            }
            return _instance;
        }
    }

    [Tooltip("Number of spells")]
    [SerializeField] private Spell[] spells;
    [Tooltip("Casting bar UI sprite")]
    [SerializeField] private Image castingBar;
    [Tooltip("Currently casted spell text display")]
    [SerializeField] private Text currentSpell;
    [Tooltip("Currently casted spell sprite display")]
    [SerializeField] private Image icon;
    [Tooltip("Currently casted spell cast time text display")]
    [SerializeField] private Text castTime;
    [Tooltip("Casting bar UI object")]
    [SerializeField] private CanvasGroup canvasGroup;
    private Coroutine _spellRoutine;
    private Coroutine _fadeRoutine;

    /// <summary>
    /// Handles casting of spells functionality
    /// </summary>
    /// <param name="spellName">Spell name</param>
    /// <returns>Type of spell at given number</returns>
    public Spell CastSpell(string spellName)
    {
        Spell spell = Array.Find(spells, x => x.Name == spellName);
        currentSpell.text = spell.Name;
        castingBar.color = spell.BarColor;
        castingBar.fillAmount = 0;
        icon.sprite = spell.Icon;
        _spellRoutine = StartCoroutine(BarProgress(spell));
        _fadeRoutine = StartCoroutine(FadeBar());

        return spell;
    }

    /// <summary>
    /// Handles cast time bar fill amount progress
    /// </summary>
    /// <param name="spellIndex">Spell number</param>
    /// <returns>null - instant</returns>
    private IEnumerator BarProgress(Spell spell)
    {
        float timePassed = Time.deltaTime;
        float rate = 1.0f / spell.CastTime;
        float progress = 0.0f;

        while (progress <= 1.0f)
        {
            castingBar.fillAmount = Mathf.Lerp(0, 1, progress);
            progress += rate * Time.deltaTime;
            timePassed += Time.deltaTime;
            castTime.text = (spell.CastTime - timePassed).ToString("F2");

            if (spell.CastTime - timePassed < 0)
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
        if (_fadeRoutine != null)
        {
            canvasGroup.alpha = 0;
            StopCoroutine(_fadeRoutine);
            _fadeRoutine = null;
        }
        if (_spellRoutine != null)
        {
            StopCoroutine(_spellRoutine);
            _spellRoutine = null;
        }
    }

    /// <summary>
    /// Get spell from spellbook
    /// </summary>
    /// <param name="spellName">Name of the spell</param>
    /// <returns>Usable spell in action bar</returns>
    public Spell GetSpell(string spellName)
    {
        Spell spell = Array.Find(spells, x => x.Name == spellName);
        return spell;
    }
}
