using UnityEngine;

public class BagScript : MonoBehaviour
{
    [SerializeField] private GameObject slotPrefab;
    private CanvasGroup _canvasGroup;

    public bool IsOpen => _canvasGroup.alpha > 0;

    private void Awake()
    {
        _canvasGroup = GetComponent<CanvasGroup>();
    }

    public void AddSlots(int slotCount)
    {
        for (int i = 0; i < slotCount; i++)
        {
            Instantiate(slotPrefab, transform);
        }
    }

    public void OpenClose()
    {
        _canvasGroup.alpha = _canvasGroup.alpha > 0 ? 0 : 1;
        _canvasGroup.blocksRaycasts = _canvasGroup.blocksRaycasts == true ? false : true;
    }
}
