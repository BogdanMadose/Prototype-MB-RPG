using UnityEngine;
using UnityEngine.Tilemaps;

public class CameraFollow : MonoBehaviour
{
    [Tooltip("Tile layer on which character can move")]
    [SerializeField] private Tilemap tilemap;
    private Transform _target;
    private float _xMax, _xMin, _yMin, _yMax;
    private Player _player;

    // Start is called before the first frame update
    void Start()
    {
        _target = GameObject.FindGameObjectWithTag("Player").transform;
        _player = _target.GetComponent<Player>();
        Vector3 minTile = tilemap.CellToWorld(tilemap.cellBounds.min);
        Vector3 maxTile = tilemap.CellToWorld(tilemap.cellBounds.max);
        SetCameraLimits(minTile, maxTile);
        _player.SetPlayerLimits(minTile, maxTile);
    }

    private void LateUpdate() => transform.position = new Vector3(Mathf.Clamp(_target.position.x, _xMin, _xMax), Mathf.Clamp(_target.position.y, _yMin, _yMax), transform.position.z);

    /// <summary>
    /// Calculate boundry limits for camera
    /// </summary>
    /// <param name="minTile">Tile 1 of the game world</param>
    /// <param name="maxTile">Last tile of the game world</param>
    private void SetCameraLimits(Vector3 minTile, Vector3 maxTile)
    {
        Camera camera = Camera.main;
        float height = 2f * camera.orthographicSize;
        float width = height * camera.aspect;

        _xMin = minTile.x + (width / 2);
        _xMax = maxTile.x - (width / 2);
        _yMin = minTile.y + (height / 2);
        _yMax = maxTile.y - (height / 2);
    }
}
