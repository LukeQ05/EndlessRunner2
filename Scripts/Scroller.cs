using UnityEngine;

/// <summary>
/// Attach to any object that should scroll left with the world speed.
/// Works for background layers, ground tiles, obstacles, coins, power-ups.
/// </summary>
public class Scroller : MonoBehaviour
{
    [Tooltip("Multiplier so background layers can scroll slower (parallax).")]
    public float speedMultiplier = 1f;

    [Tooltip("If true this object destroys itself when off the left edge of screen.")]
    public bool destroyOffScreen = false;
    public float destroyX = -20f;

    void Update()
    {
        if (GameManager.Instance == null) return;

        float speed = GameManager.Instance.CurrentSpeed * speedMultiplier;
        transform.Translate(Vector3.left * speed * Time.deltaTime);

        if (destroyOffScreen && transform.position.x < destroyX)
            Destroy(gameObject);
    }
}
