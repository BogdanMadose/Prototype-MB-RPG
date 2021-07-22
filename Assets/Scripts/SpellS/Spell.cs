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

    public string Name { get => name; }
    public int Damage { get => damage; }
    public Sprite Icon { get => icon; }
    public float Speed { get => speed; }
    public float CastTime { get => castTime; }
    public GameObject SpellPrefab { get => spellPrefab; }
    public Color BarColor { get => barColor; }

    public void Use()
    {
        Player.Instance.CastSpell(Name);
    }
}