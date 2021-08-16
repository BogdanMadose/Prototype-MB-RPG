using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class CombatText : MonoBehaviour
{
    [Tooltip("Text scrolling speed")]
    [SerializeField] private float speed;
    [Tooltip("Fade duration")]
    [SerializeField] private float lifeTime;
    [Tooltip("Combat text UI reference")]
    [SerializeField] private Text combatText;
    [Tooltip("Combat text outline")]
    [SerializeField] private Outline combatTextOutline;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine("FadeOut");
    }

    // Update is called once per frame
    void Update()
    {
        ScrollText();
    }

    /// <summary>
    /// Move text up
    /// </summary>
    public void ScrollText()
    {
        transform.Translate(Vector2.up * speed * Time.deltaTime);
    }

    /// <summary>
    /// Fade out combat text
    /// </summary>
    /// <returns></returns>
    public IEnumerator FadeOut()
    {
        float startAlpha = combatText.color.a;
        float outStartAlpha = combatTextOutline.effectColor.a;
        float rate = 1.0f / lifeTime;
        float progress = 0.0f;
        while (progress < 1.0)
        {
            Color tmp = combatText.color;
            Color tmpOut = combatTextOutline.effectColor;
            tmp.a = Mathf.Lerp(startAlpha, 0, progress);
            tmpOut.a = Mathf.Lerp(outStartAlpha, 0, progress);
            combatText.color = tmp;
            combatTextOutline.effectColor = tmpOut;
            progress += rate * Time.deltaTime;
            yield return null;
        }
        Destroy(gameObject);
    }
}
