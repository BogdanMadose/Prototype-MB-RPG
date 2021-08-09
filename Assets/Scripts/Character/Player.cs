using System.Collections;
using UnityEngine;

public class Player : Character
{
    private static Player _instance;
    public static Player Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<Player>();
            }
            return _instance;
        }
    }

    #region Variables
    [Tooltip("Player set mana")]
    [SerializeField] private Stat mana;
    [Tooltip("Player inital mana")]
    [SerializeField] private float initMana;
    [Tooltip("Spell exit points")]
    [SerializeField] private Transform[] exitPoints;
    [Tooltip("Raycaster blocker array")]
    [SerializeField] private Block[] blocks;
    [Tooltip("Animated equipment reference on player")]
    [SerializeField] private EquipmentSocket[] equipmentSockets;
    private IInteractable _interactable;
    private int _exitIndex = 2;
    private Vector3 _min, _max;
    #endregion

    public int Gold { get; set; }
    public IInteractable Interactable { get; set; }

    protected override void Start()
    {
        Gold = 100;
        mana.Initialize(initMana, initMana);
        base.Start();
    }

    protected override void Update()
    {
        GetInput();
        transform.position = new Vector3(Mathf.Clamp(transform.position.x, _min.x, _max.x), Mathf.Clamp(transform.position.y, _min.y, _max.y), transform.position.z);
        base.Update();
    }

    /// <summary>
    /// Read directional keys for the player input
    /// </summary>
    public void GetInput()
    {
        Direction = Vector2.zero;

        // -----------DEBUGGING ---------------------

        // decrease health and mana
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            Health.CurrentValue -= 10;
            mana.CurrentValue -= 10;

        }
        // increase health and mana
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            Health.CurrentValue += 10;
            mana.CurrentValue += 10;

        }

        // -----------------------------------------

        // move up
        if (Input.GetKey(KeyBindManager.Instance.Keybinds["UP"]))
        {
            _exitIndex = 0;
            Direction += Vector2.up;
        }

        // move left
        if (Input.GetKey(KeyBindManager.Instance.Keybinds["LEFT"]))
        {
            _exitIndex = 3;
            Direction += Vector2.left;
        }

        // move down
        if (Input.GetKey(KeyBindManager.Instance.Keybinds["DOWN"]))
        {
            _exitIndex = 2;
            Direction += Vector2.down;
        }

        // move right
        if (Input.GetKey(KeyBindManager.Instance.Keybinds["RIGHT"]))
        {
            _exitIndex = 1;
            Direction += Vector2.right;
        }

        if (IsMoving)
        {
            StopAttack();
        }

        foreach (string action in KeyBindManager.Instance.ActionBinds.Keys)
        {
            if (Input.GetKeyDown(KeyBindManager.Instance.ActionBinds[action]))
            {
                UIManager.Instance.UseActionButton(action);
            }
        }
    }

    /// <summary>
    /// Set world boundries for player
    /// </summary>
    /// <param name="min">Lowest boundry value</param>
    /// <param name="max">Highest boundry value</param>
    public void SetPlayerLimits(Vector3 min, Vector3 max)
    {
        this._min = min;
        this._max = max;
    }

    /// <summary>
    /// Starts an attack or cast event
    /// </summary>
    /// /// <param name="spellName">Name of the spell selected</param>
    /// <returns>After x seconds casts spell / attacks then ends coroutine</returns>
    private IEnumerator Attack(string spellName)
    {
        Transform currentTarget = Target;

        Spell newSpell = SpellBook.Instance.CastSpell(spellName);
        IsAttacking = true;
        Animator.SetBool("attack", IsAttacking);
        foreach (EquipmentSocket e in equipmentSockets)
        {
            e.Animator.SetBool("attack", IsAttacking);
        }
        yield return new WaitForSeconds(newSpell.CastTime);

        if (currentTarget != null && InLineOfSight())
        {
            SpellScript spell = Instantiate(newSpell.SpellPrefab, exitPoints[_exitIndex].position, Quaternion.identity).GetComponent<SpellScript>();
            spell.Initialize(currentTarget, newSpell.Damage, transform);
        }

        StopAttack();
    }

    /// <summary>
    /// Cast Spell ( Instantiate spell projectile prefab )
    /// </summary>
    /// /// <param name="spellName">Name of the spell selected</param>
    public void CastSpell(string spellName)
    {
        BlockView();

        if (Target != null && Target.GetComponentInParent<Character>().IsAlive && !IsAttacking && !IsMoving && InLineOfSight())
        {
            _attackRoutine = StartCoroutine(Attack(spellName));
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
        if (Target != null)
        {
            Vector3 targetDirection = (Target.transform.position - transform.position).normalized;
            RaycastHit2D hit = Physics2D.Raycast(transform.position, targetDirection, Vector2.Distance(transform.position, Target.transform.position), 256);

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

        blocks[_exitIndex].Activate();
    }

    /// <summary>
    /// Function to stop attacking.
    /// 
    /// <para>Sets both animator bool and isAttacking to false and stops the attack coroutine</para>
    /// </summary>
    public void StopAttack()
    {
        SpellBook.Instance.StopCasting();
        IsAttacking = false;
        Animator.SetBool("attack", IsAttacking);
        foreach (EquipmentSocket e in equipmentSockets)
        {
            e.Animator.SetBool("attack", IsAttacking);
        }
        if (_attackRoutine != null)
        {
            StopCoroutine(_attackRoutine);
        }
    }

    public override void HandleLayers()
    {
        base.HandleLayers();
        if (IsMoving)
        {
            foreach (EquipmentSocket e in equipmentSockets)
            {
                e.SetDirection(Direction.x, Direction.y);
            }
        }
    }

    public override void ActivateLayer(string layerName)
    {
        base.ActivateLayer(layerName);
        foreach (EquipmentSocket e in equipmentSockets)
        {
            e.HandleEquipmentAnimLayer(layerName);
        }
    }

    public void Interact()
    {
        if (Interactable != null)
        {
            Interactable.Interact();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Enemy" || collision.tag == "Interactable")
        {
            Interactable = collision.GetComponent<IInteractable>();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Enemy" || collision.tag == "Interactable")
        {
            if (Interactable != null)
            {
                Interactable.StopInteracting();
                Interactable = null;
            }
        }
    }
}