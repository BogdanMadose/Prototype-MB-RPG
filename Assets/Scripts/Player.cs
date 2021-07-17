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

    /// <summary>
    /// Array that will hold spawned prefabs for projectile spells
    /// </summary>
    [SerializeField] private GameObject[] spellPrefab;

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

        // decrease health and mana
        if (Input.GetKeyDown(KeyCode.I))
        {
            health.MCurrentValue -= 10;
            mana.MCurrentValue -= 10;

        }
        // increase health and mana
        if (Input.GetKeyDown(KeyCode.O))
        {
            health.MCurrentValue += 10;
            mana.MCurrentValue += 10;

        }

        // -----------------------------------------

        // move up
        if (Input.GetKey(KeyCode.W))
        {
            direction += Vector2.up;
        }

        // move left
        if (Input.GetKey(KeyCode.A))
        {
            direction += Vector2.left;
        }

        // move down
        if (Input.GetKey(KeyCode.S))
        {
            direction += Vector2.down;
        }

        // move right
        if (Input.GetKey(KeyCode.D))
        {
            direction += Vector2.right;
        }

        // attack / cast
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (!isAttacking && !IsMoving) //check if able to attack
            {
                attackRoutine = StartCoroutine(Attack());
            }
        }
    }

    /// <summary>
    /// Starts an attack or cast event
    /// </summary>
    /// <returns>After x seconds stops attack coroutine</returns>
    private IEnumerator Attack()
    {
        isAttacking = true;
        mAnimator.SetBool("attack", isAttacking);

        yield return new WaitForSeconds(1); // DEBUGGING
        CastSpell();

        StopAttack();    // end attack 
    }

    /// <summary>
    /// Cast Spell
    /// </summary>
    public void CastSpell()
    {
        Instantiate(spellPrefab[0], transform.position, Quaternion.identity);
    }
}
