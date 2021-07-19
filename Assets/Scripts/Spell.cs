using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Spell
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

    public string MName { get => name; }
    public int MDamage { get => damage; }
    public Sprite MIcon { get => icon; }
    public float MSpeed { get => speed; }
    public float MCastTime { get => castTime; }
    public GameObject MSpellPrefab { get => spellPrefab; }
    public Color MBarColor { get => barColor; }
}
