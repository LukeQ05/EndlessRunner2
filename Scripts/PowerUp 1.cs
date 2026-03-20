using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public abstract class PowerUpBase : MonoBehaviour
{
    public float duration = 5f;
    public AudioClip collectClip;

    void OnTriggerEnter2D(Collider2D other)
    {
        PlayerController player = other.GetComponent<PlayerController>();
        if (player != null)
        {
            ApplyEffect(player);
            player.OnCollect();
            Destroy(gameObject);
        }
    }

    protected abstract void ApplyEffect(PlayerController player);
}