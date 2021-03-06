using UnityEngine;
using UnityEngine.EventSystems;

public delegate void KillConfirmed(Character character);

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    public static GameManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<GameManager>();
            }
            return _instance;
        }
    }

    public event KillConfirmed killConfirmedEvent;

    [Tooltip("Player object")]
    [SerializeField] private Player player;
    private Camera mainCamera;
    private Enemy _currentTarget;

    private void Start() => mainCamera = Camera.main;

    void Update() => ClickTarget();

    /// <summary>
    /// Select / deselect interactable objects
    /// </summary>
    private void ClickTarget()
    {
        if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
        {
            RaycastHit2D hit = Physics2D.Raycast(mainCamera.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, Mathf.Infinity, 512);
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
            RaycastHit2D hit = Physics2D.Raycast(mainCamera.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, Mathf.Infinity, 512);
            if (hit.collider != null && (hit.collider.tag == "Enemy" || hit.collider.tag == "Interactable") && hit.collider.gameObject.GetComponent<IInteractable>() == player.Interactable)
            {
                player.Interact();
            }
        }
    }

    public void OnKillConfirmed(Character character) => killConfirmedEvent?.Invoke(character);

    /// <summary>
    /// Quit game
    /// </summary>
    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
