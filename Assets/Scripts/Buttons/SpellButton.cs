using UnityEngine;
using UnityEngine.EventSystems;

public class SpellButton : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private string spellName;
    public void OnPointerClick(PointerEventData eventData)
    {
        if(eventData.button == PointerEventData.InputButton.Left)
        {
            HandScript.MInstance.TakeMovable(SpellBook.MInstance.GetSpell(spellName));
        }
    }
}
