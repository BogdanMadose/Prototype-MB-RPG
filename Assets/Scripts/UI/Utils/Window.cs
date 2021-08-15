using UnityEngine;

public class Window : MonoBehaviour
{
    [Tooltip("Vendor window UI object")]
    [SerializeField] private CanvasGroup canvasGroup;
    private NPC _npc;

    /// <summary>
    /// Close window
    /// </summary>
    public virtual void CloseWindow()
    {
        _npc.IsInteracting = false;
        canvasGroup.alpha = 0;
        canvasGroup.blocksRaycasts = false;
        _npc = null;
    }

    /// <summary>
    /// Open window
    /// </summary>
    public virtual void OpenWindow(NPC npc)
    {
        _npc = npc;
        canvasGroup.alpha = 1;
        canvasGroup.blocksRaycasts = true;
    }
}
