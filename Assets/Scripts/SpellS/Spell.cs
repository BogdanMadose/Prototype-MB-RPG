using System;
using UnityEngine;

[Serializable]
public class Spell : IUsable, IMovable, IDescribable
{
    [Tooltip("Spell name")]
    [SerializeField] private string name;
    [Tooltip("Spell damage (int value)")]
    [SerializeField] private int damage;
    [Tooltip("Spell icon")]
    [SerializeField] private Sprite icon;
    [Tooltip("Spell travel speed (float value)")]
    [SerializeField] private float speed;
    [Tooltip("Spell cast time (float value)")]
    [SerializeField] private float castTime;
    [Tooltip("Spell prefab")]
    [SerializeField] private GameObject spellPrefab;
    [Tooltip("Casting bar color")]
    [SerializeField] private Color barColor;
    [Tooltip("Spell description")]
    [SerializeField] private string description;

    public string Name => name;
    public int Damage => damage;
    public Sprite Icon => icon;
    public float Speed => speed;
    public float CastTime => castTime;
    public GameObject SpellPrefab => spellPrefab;
    public Color BarColor => barColor;

    public string GetDescription()
    {
        Color color = barColor;
        if (castTime == 1)
        {
            return string.Format("<color=#{4}>{0}</color>\n<color=#00ff00>Cast time: {1} second\nDamage: {3}\n{2}</color>", 
                name, castTime, description.Replace("#", Environment.NewLine), damage, ColorUtility.ToHtmlStringRGB(barColor));

        }
        else
        {
            return string.Format("<color=#{4}>{0}</color>\n<color=#00ff00>Cast time: {1} second\nDamage: {3}\n{2}</color>", 
                name, castTime, description.Replace("#", Environment.NewLine), damage, ColorUtility.ToHtmlStringRGB(barColor));
        }
    }

    public void Use() => Player.Instance.CastSpell(Name);
}