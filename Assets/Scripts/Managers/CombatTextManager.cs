using UnityEngine;
using UnityEngine.UI;

public class CombatTextManager : MonoBehaviour
{
    private static CombatTextManager _instance;
    public static CombatTextManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<CombatTextManager>();
            }
            return _instance;
        }
    }

    [Tooltip("Combat text prefab object")]
    [SerializeField] private GameObject combatTextPref;

    public void GenerateText(Vector2 position, string text, ScrollTextType type, bool crit)
    {
        position.y += 0.6f;
        Text scrollText = Instantiate(combatTextPref, transform).GetComponent<Text>();
        scrollText.transform.position = position;
        string operation = string.Empty;
        string end = string.Empty;
        switch (type)
        {
            case ScrollTextType.Damage:
                operation = "-";
                scrollText.color = Color.red;
                break;
            case ScrollTextType.Heal:
                operation = "+";
                scrollText.color = Color.green;
                break;
            case ScrollTextType.XP:
                operation = "+";
                end = " XP";
                scrollText.color = Color.yellow;
                break;
        }
        scrollText.text = operation + text + end;
        if (crit)
        {
            scrollText.GetComponent<Animator>().SetBool("Crit", crit);
        }
    }
}