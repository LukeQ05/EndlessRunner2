using UnityEngine;

/// <summary>
/// Attach to each obstacle prefab.
/// The Scroller component handles movement; this handles collision.
/// </summary>
[RequireComponent(typeof(Collider2D))]
public class Obstacle : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        PlayerController player = other.GetComponent<PlayerController>();
        if (player != null)
        {
            player.TakeDamage();
        }
    }
}
