using UnityEngine;
using UnityEngine.EventSystems;

public class GameManager : MonoBehaviour
{
    [Tooltip("Player object")]
    [SerializeField] private Player player;
    private Enemy _currentTarget;

    void Update() => ClickTarget();

    /// <summary>
    /// Select / deselect interactable objects
    /// </summary>
    private void ClickTarget()
    {
        if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
        {
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, Mathf.Infinity, 512);
            if (hit.collider != null && hit.collider.tag == "Enemy")
            {
                if (_currentTarget != null)
                {
                    _currentTarget.Deselect();
                }
                _currentTarget = hit.collider.GetComponent<Enemy>();
                player.Target = _currentTarget.Select();
                UIManager.Instance.ShowTargetFrame(_currentTarget);
            }
            else
            {
                UIManager.Instance.HideTargetFrame();
                if (_currentTarget != null)
                {
                    _currentTarget.Deselect();
                }
                _currentTarget = null;
                player.Target = null;
            }
        }
        else if (Input.GetMouseButtonDown(1) && !EventSystem.current.IsPointerOverGameObject())
        {
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, Mathf.Infinity, 512);
            if (hit.collider != null && (hit.collider.tag == "Enemy" || hit.collider.tag == "Interactable") && hit.collider.gameObject.GetComponent<IInteractable>() == player.Interactable)
            {
                player.Interact();
            }
        }
    }
}
