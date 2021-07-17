using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Character
{
    /// <summary>
    /// Component reference to player health
    /// </summary>
    [SerializeField] private Stat health;
    /// <summary>
    /// component reference to player mana
    /// </summary>
    [SerializeField] private Stat mana;
    /// <summary>
    /// Initial allowed player health
    /// </summary>
    [SerializeField] private float initHealth;
    /// <summary>
    /// Initial allowed player mana
    /// </summary>
    [SerializeField] private float initMana;

    protected override void Start()
    {
        // Initializing health and mana to values assigned in Editor
        health.Initialize(initHealth, initHealth);
        mana.Initialize(initMana, initMana);
        base.Start();
    }

    // Update is called once per frame
    protected override void Update()
    {
        GetInput();
        base.Update();
    }

    /// <summary>
    /// Read directional keys for the player input
    /// </summary>
    public void GetInput()
    {
        direction = Vector2.zero;

        // -----------DEBUGGING ---------------------

        if (Input.GetKeyDown(KeyCode.I))
        {
            health.MCurrentValue -= 10;
            mana.MCurrentValue -= 10;

        }
        if (Input.GetKeyDown(KeyCode.O))
        {
            health.MCurrentValue += 10;
            mana.MCurrentValue += 10;

        }

        // -----------------------------------------

        if (Input.GetKey(KeyCode.W))
        {
            direction += Vector2.up;
        }

        if (Input.GetKey(KeyCode.A))
        {
            direction += Vector2.left;
        }

        if (Input.GetKey(KeyCode.S))
        {
            direction += Vector2.down;
        }

        if (Input.GetKey(KeyCode.D))
        {
            direction += Vector2.right;
        }
    }
}
