using System;
using UnityEngine;

[Serializable]
public class Spell : IUsable, IMovable
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

    public string Name => name;
    public int Damage => damage;
    public Sprite Icon => icon;
    public float Speed => speed;
    public float CastTime => castTime;
    public GameObject SpellPrefab => spellPrefab;
    public Color BarColor => barColor;

    public void Use() => Player.Instance.CastSpell(Name);
}