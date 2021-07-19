using System.Collections;
using UnityEngine;

public class Player : Character
{
    #region Variables
    /// <summary>
    /// Component reference to player mana
    /// </summary>
    [Tooltip("Player mana")]
    [SerializeField] private Stat mana;

    /// <summary>
    /// Initial allowed player mana
    /// </summary>
    [SerializeField] private float initMana;

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

    private SpellBook spellBook;
    #endregion

    /// <summary>
    /// Reference to clickable target
    /// </summary>
    public Transform MTarget { get; set; }

    protected override void Start()
    {
        spellBook = GetComponent<SpellBook>();
        mana.Initialize(initMana, initMana);

        base.Start();
    }

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
    }

    /// <summary>
    /// Starts an attack or cast event
    /// </summary>
    /// /// <param name="spellIndex">Number of the spell selected</param>
    /// <returns>After x seconds casts spell / attacks then ends coroutine</returns>
    private IEnumerator Attack(int spellIndex)
    {
        Transform currentTarget = MTarget;

        Spell newSpell = spellBook.CastSpell(spellIndex);
        isAttacking = true;
        mAnimator.SetBool("attack", isAttacking);
        yield return new WaitForSeconds(newSpell.MCastTime);

        if (currentTarget != null && InLineOfSight())
        {
            SpellScript spell = Instantiate(newSpell.MSpellPrefab, exitPoints[exitIndex].position, Quaternion.identity).GetComponent<SpellScript>();
            spell.Initialize(currentTarget, newSpell.MDamage);
        }

        StopAttack();
    }

    /// <summary>
    /// Cast Spell ( Instantiate spell projectile prefab )
    /// </summary>
    /// /// <param name="spellIndex">Number of the spell selected</param>
    public void CastSpell(int spellIndex)
    {
        BlockView();

        if (MTarget != null && !isAttacking && !IsMoving && InLineOfSight())
        {
            attackRoutine = StartCoroutine(Attack(spellIndex));
        }
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
        if (MTarget != null)
        {
            Vector3 targetDirection = (MTarget.transform.position - transform.position).normalized;
            RaycastHit2D hit = Physics2D.Raycast(transform.position, targetDirection, Vector2.Distance(transform.position, MTarget.transform.position), 256);

            if (hit.collider == null)
            {
                return true;
            }
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

        blocks[exitIndex].Activate();
    }

    public override void StopAttack()
    {
        spellBook.StopCasting();
        base.StopAttack();
    }
}
