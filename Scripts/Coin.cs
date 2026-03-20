using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Coin : MonoBehaviour
{
    public int scoreValue = 10;
    public float autoCollectRadius = 5f;  // used when auto-collect power-up is active

    private Transform playerTransform;
    private PlayerController player;
    private bool collected = false;

    void Start()
    {
        GameObject p = GameObject.FindGameObjectWithTag("Player");
        if (p != null)
        {
            playerTransform = p.transform;
            player = p.GetComponent<PlayerController>();
        }
    }

    void Update()
    {
        if (collected || player == null) return;

        // Auto-collect magnet effect
        if (player.isAutoCollect)
        {
            float dist = Vector2.Distance(transform.position, playerTransform.position);
            if (dist < autoCollectRadius)
            {
                // Fly toward player
                transform.position = Vector2.MoveTowards(
                    transform.position,
                    playerTransform.position,
                    20f * Time.deltaTime);
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (collected) return;
        PlayerController p = other.GetComponent<PlayerController>();
        if (p != null)
        {
            collected = true;
            GameManager.Instance.Score += scoreValue; // direct add
            p.OnCollect();
            Destroy(gameObject);
        }
    }
}
