using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class CameraFollow : MonoBehaviour
{
    private Transform target;
    private float xMax, xMin, yMin, yMax;
    private Player player;
    [SerializeField] private Tilemap tilemap;

    // Start is called before the first frame update
    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;
        player = target.GetComponent<Player>();
        Vector3 minTile = tilemap.CellToWorld(tilemap.cellBounds.min);
        Vector3 maxTile = tilemap.CellToWorld(tilemap.cellBounds.max);
        SetCameraLimits(minTile, maxTile);
        player.SetPlayerLimits(minTile, maxTile);
    }

    private void LateUpdate()
    {
        transform.position = new Vector3(Mathf.Clamp(target.position.x, xMin, xMax), Mathf.Clamp(target.position.y, yMin, yMax), transform.position.z);
    }

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

        xMin = minTile.x + width / 2;
        xMax = maxTile.x - width / 2;
        yMin = minTile.y + height / 2;
        yMax = maxTile.y - height / 2;
    }
}
