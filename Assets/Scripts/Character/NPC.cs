using UnityEngine;

public class NPC : MonoBehaviour, IInteractable
{
    [Tooltip("Window script reference")]
    [SerializeField] private Window window;

    public bool IsInteracting { get; set; }

    public virtual void Interact()
    {
        if (!IsInteracting)
        {
            IsInteracting = true;
            window.OpenWindow(this);
        }
    }

    public virtual void StopInteracting()
    {
        if (IsInteracting)
        {
            IsInteracting = false;
            window.CloseWindow();
        }
    }
}
