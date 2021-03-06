using UnityEngine;
using UnityEngine.EventSystems;

public class SpellButton : MonoBehaviour, IPointerClickHandler
{
    [Tooltip("Name of the spell")]
    [SerializeField] private string spellName;
    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            HandScript.Instance.TakeMovable(SpellBook.Instance.GetSpell(spellName));
        }
    }
}
