using UnityEngine;
using UnityEngine.EventSystems;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Player player;
    private NPC _currentTarget;

    void Update() => ClickTarget();

    /// <summary>
    /// Handles selecting and deselecting of gameObjects
    /// </summary>
    private void ClickTarget()
    {
        if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
        {
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, Mathf.Infinity, 512);

            if (hit.collider != null)
            {
                if (_currentTarget != null)
                {
                    _currentTarget.Deselect();
                }

                _currentTarget = hit.collider.GetComponent<NPC>();
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
    }
}
