using UnityEngine;

/// <summary>
/// Place on a parent object that holds 2-3 ground tile children.
/// Tiles recycle from left to right for infinite ground.
/// </summary>
public class GroundTiler : MonoBehaviour
{
    public float tileWidth = 20f;   // width of one ground tile in world units
    public float resetX   = -20f;  // x position at which a tile gets teleported right
    public float rightX   = 20f;   // x position to teleport the tile to

    Transform[] tiles;

    void Awake()
    {
        tiles = new Transform[transform.childCount];
        for (int i = 0; i < tiles.Length; i++)
            tiles[i] = transform.GetChild(i);
    }

    void Update()
    {
        foreach (var tile in tiles)
        {
            if (tile.position.x < resetX)
            {
                // Find the rightmost tile and place this one just after it
                float maxX = float.MinValue;
                foreach (var t in tiles)
                    if (t.position.x > maxX) maxX = t.position.x;

                tile.position = new Vector3(maxX + tileWidth, tile.position.y, tile.position.z);
            }
        }
    }
}
