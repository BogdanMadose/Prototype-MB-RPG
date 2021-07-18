using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Spell
{
    [SerializeField] private string name;
    [SerializeField] private int damage;
    [SerializeField] private Sprite icon;
    [SerializeField] private float speed;
    [SerializeField] private float castTime;
    [SerializeField] private GameObject spellPrefab;
    [SerializeField] private Color barColor;

    public string MName { get => name; }
    public int MDamage { get => damage; }
    public Sprite MIcon { get => icon; }
    public float MSpeed { get => speed; }
    public float MCastTime { get => castTime; }
    public GameObject MSpellPrefab { get => spellPrefab; }
    public Color MBarColor { get => barColor; }
}
