using System.Collections;
using UnityEngine;

public class Player : Character
{
    #region Variables
    /// <summary>
    /// Component reference to player health
    /// </summary>
    [SerializeField] private Stat health;
    /// <summary>
    /// Component reference to player mana
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

    /// <summary>
    /// Array that holds the exact points from where casted spells start
    /// </summary>
    [SerializeField] private Transform[] exitPoints;

    /// <summary>
    /// Holds the physical invisible raycaster objects
    /// </summary>
    [SerializeField] private Block[] blocks;

    /// <summary>
    /// exitPoints array index (Default = 2 ( facing down [South])) (cardinal directions)
    /// </summary>
    private int exitIndex = 2; 
    #endregion

    // TESTING DEBUGGING
    private Transform target; 

    protected override void Start()
    {
        // Initializing health and mana to values assigned in Editor
        health.Initialize(initHealth, initHealth);
        mana.Initialize(initMana, initMana);

        target = GameObject.Find("Target").transform; 

        base.Start();
    }

    // Update is called once per frame
    protected override void Update()
    {
        GetInput();

        // Get "Block" actual true layer mask
        // Debug.Log(LayerMask.GetMask("Block"));

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
        //if (Input.GetKeyDown(KeyCode.I))
        //{
        //    health.MCurrentValue -= 10;
        //    mana.MCurrentValue -= 10;

        //}
        // increase health and mana
        //if (Input.GetKeyDown(KeyCode.O))
        //{
        //    health.MCurrentValue += 10;
        //    mana.MCurrentValue += 10;

        //}

        // -----------------------------------------

        // move up
        if (Input.GetKey(KeyCode.W))
        {
            exitIndex = 0;
            direction += Vector2.up;
        }

        // move left
        if (Input.GetKey(KeyCode.A))
        {
            exitIndex = 3;
            direction += Vector2.left;
        }

        // move down
        if (Input.GetKey(KeyCode.S))
        {
            exitIndex = 2;
            direction += Vector2.down;
        }

        // move right
        if (Input.GetKey(KeyCode.D))
        {
            exitIndex = 1;
            direction += Vector2.right;
        }

        // attack / cast
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // Raycast blockers will be activated oposite the player facing direction
            BlockView();

            if (!isAttacking && !IsMoving && InLineOfSight()) //check if able to attack and in sight
            {
                attackRoutine = StartCoroutine(Attack());
            }
        }
    }

    #region Attack & Cast Spell Functionality
    /// <summary>
    /// Starts an attack or cast event
    /// </summary>
    /// <returns>After x seconds casts spell / attacks then ends coroutine</returns>
    private IEnumerator Attack()
    {
        isAttacking = true;
        mAnimator.SetBool("attack", isAttacking);

        yield return new WaitForSeconds(1); // Not final value, hardcoded value
        CastSpell();      // cast spell

        StopAttack();    // end attack 
    }

    /// <summary>
    /// Cast Spell ( Instantiate spell projectile prefab )
    /// </summary>
    public void CastSpell()
    {
        Instantiate(spellPrefab[0], exitPoints[exitIndex].position, Quaternion.identity);
    }

    /// <summary>
    /// Check if targeted object is visible to the player
    /// </summary>
    /// <returns>
    /// <para>TRUE - if visible (raycast blockers are not in sight)</para>
    /// <para>FALSE - if not visible (raycast blockers are in sight blocking the view)</para>
    /// </returns>
    private bool InLineOfSight()
    {
        // Calculate direction towards target
        Vector3 targetDirection = (target.transform.position - transform.position).normalized;

        // Raycasting from player to targeted object only on world layer 256
        RaycastHit2D hit = Physics2D.Raycast(transform.position, targetDirection, Vector2.Distance(transform.position, target.transform.position), 256);

        if (hit.collider == null)
        {
            return true;
        }

        return false;
    }

    /// <summary>
    /// Function which deactivates all raycast blockers, then activates them according to the direction the player is facing
    /// </summary>
    private void BlockView()
    {
        foreach (Block b in blocks)
        {
            b.Deactivate();
        }

        // activate 2 at a time based on cardinal directions from exitIndex
        blocks[exitIndex].Activate();
    } 
    #endregion
}
