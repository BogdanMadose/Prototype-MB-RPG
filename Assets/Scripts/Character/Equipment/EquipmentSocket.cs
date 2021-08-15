using UnityEngine;

public class EquipmentSocket : MonoBehaviour
{
    protected SpriteRenderer _spriteRenderer;
    private AnimatorOverrideController _overrideController;
    private Animator _playerAnimator;

    public Animator Animator { get; set; }

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _playerAnimator = GetComponentInParent<Animator>();
        Animator = GetComponent<Animator>();
        _overrideController = new AnimatorOverrideController(Animator.runtimeAnimatorController);
        Animator.runtimeAnimatorController = _overrideController;
    }

    /// <summary>
    /// Set animation facing direction
    /// </summary>
    /// <param name="x">Direction on X</param>
    /// <param name="y">Direction on Y</param>
    public virtual void SetDirection(float x, float y)
    {
        Animator.SetFloat("X", x);
        Animator.SetFloat("Y", y);
    }

    /// <summary>
    /// Activate equipment animation layers 
    /// </summary>
    /// <param name="layerName">Layer active</param>
    public void HandleEquipmentAnimLayer(string layerName)
    {
        for (int i = 0; i < Animator.layerCount; i++)
        {
            Animator.SetLayerWeight(i, 0);
        }
        Animator.SetLayerWeight(Animator.GetLayerIndex(layerName), 1);
    }

    /// <summary>
    /// Override player animation with equipment animation
    /// </summary>
    /// <param name="animations">Overridden animation</param>
    public void AnimateEquip(AnimationClip[] animations)
    {
        _spriteRenderer.color = Color.white;
        _overrideController["Wizard_Attack_Back"] = animations[0];
        _overrideController["Wizard_Attack_Front"] = animations[1];
        _overrideController["Wizard_Attack_left"] = animations[2];
        _overrideController["Wizard_Attack_Right"] = animations[3];
        _overrideController["Wizard_Idle_Back"] = animations[4];
        _overrideController["Wizard_Idle_Front"] = animations[5];
        _overrideController["Wizard_Idle_Left"] = animations[6];
        _overrideController["Wizard_Idle_Right"] = animations[7];
        _overrideController["Wizard_Walk_Back"] = animations[8];
        _overrideController["Wizard_Walk_Front"] = animations[9];
        _overrideController["Wizard_Walk_Left"] = animations[10];
        _overrideController["Wizard_Walk_Right"] = animations[11];
    }

    /// <summary>
    /// Revert player animations to default
    /// </summary>
    public void AnimateDequip()
    {
        _overrideController["Wizard_Attack_Back"] = null;
        _overrideController["Wizard_Attack_Front"] = null;
        _overrideController["Wizard_Attack_left"] = null;
        _overrideController["Wizard_Attack_Right"] = null;
        _overrideController["Wizard_Idle_Back"] = null;
        _overrideController["Wizard_Idle_Front"] = null;
        _overrideController["Wizard_Idle_Left"] = null;
        _overrideController["Wizard_Idle_Right"] = null;
        _overrideController["Wizard_Walk_Back"] = null;
        _overrideController["Wizard_Walk_Front"] = null;
        _overrideController["Wizard_Walk_Left"] = null;
        _overrideController["Wizard_Walk_Right"] = null;
        Color c = _spriteRenderer.color;
        c.a = 0;
        _spriteRenderer.color = c;
    }
}
