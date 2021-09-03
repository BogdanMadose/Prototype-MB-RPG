using System;
using UnityEngine;

[Serializable]
public class PlayerData
{
    public PlayerData(int level, float xp, float maxXp, float health, float maxHealth, float mana, float maxMana, Vector2 position)
    {
        Level = level;
        XP = xp;
        MaxXP = maxXp;
        Health = health;
        MaxHealth = maxHealth;
        Mana = mana;
        MaxMana = maxMana;
        Y = position.y;
        X = position.x;
    }

    public int Level { get; set; }
    public float XP { get; set; }
    public float MaxXP { get; set; }
    public float Health { get; set; }
    public float MaxHealth { get; set; }
    public float Mana { get; set; }
    public float MaxMana { get; set; }
    public float X { get; set; }
    public float Y { get; set; }

}
