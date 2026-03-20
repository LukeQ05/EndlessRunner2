using System.Collections;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [System.Serializable]
    public class SpawnEntry
    {
        public GameObject prefab;
        [Range(0f, 1f)] public float weight = 1f;
    }

    [Header("Obstacle Types (need at least 3)")]
    public SpawnEntry[] obstaclePrefabs;   // assign 3+ obstacle prefabs

    [Header("Collectables")]
    public GameObject coinPrefab;
    public GameObject powerUpShieldPrefab;
    public GameObject powerUpSlowMoPrefab;
    public GameObject powerUpAutoCollectPrefab;

    [Header("Spawn Settings")]
    public Transform spawnPoint;            // right side of screen
    public float baseSpawnInterval  = 2.2f;
    public float minSpawnInterval   = 0.65f;
    public float coinGroupChance    = 0.35f; // probability of spawning a coin row
    public float powerUpChance      = 0.06f; // probability of spawning a power-up

    [Header("Coin Row")]
    public int   coinRowCount  = 5;
    public float coinSpacing   = 0.6f;
    public float coinHeight    = 1.8f;      // y position of coin row

    private float timer;
    private float currentInterval;

    void Start()
    {
        currentInterval = baseSpawnInterval;
        timer = currentInterval;
    }

    void Update()
    {
        if (GameManager.Instance == null || !GameManager.Instance.IsRunning) return;

        // Difficulty factor 0→1 over the game
        float d = GameManager.Instance.DifficultyFactor;
        currentInterval = Mathf.Lerp(baseSpawnInterval, minSpawnInterval, d);

        timer -= Time.deltaTime;
        if (timer <= 0f)
        {
            Spawn();
            timer = currentInterval + Random.Range(-0.15f, 0.25f); // slight jitter
        }
    }

    void Spawn()
    {
        float roll = Random.value;

        if (roll < powerUpChance)
        {
            SpawnPowerUp();
        }
        else if (roll < powerUpChance + coinGroupChance)
        {
            SpawnCoinRow();
        }
        else
        {
            SpawnObstacle();
        }
    }

    // ── Obstacles ────────────────────────────────────────────────

    void SpawnObstacle()
    {
        if (obstaclePrefabs == null || obstaclePrefabs.Length == 0) return;

        GameObject prefab = WeightedRandom(obstaclePrefabs);
        if (prefab == null) return;

        Vector3 pos = spawnPoint.position;
        Instantiate(prefab, pos, Quaternion.identity);
    }

    GameObject WeightedRandom(SpawnEntry[] entries)
    {
        float total = 0;
        foreach (var e in entries) total += e.weight;
        float r = Random.Range(0, total);
        foreach (var e in entries)
        {
            r -= e.weight;
            if (r <= 0) return e.prefab;
        }
        return entries[entries.Length - 1].prefab;
    }

    // ── Coins ────────────────────────────────────────────────────

    void SpawnCoinRow()
    {
        if (coinPrefab == null) return;

        float startX = spawnPoint.position.x;
        for (int i = 0; i < coinRowCount; i++)
        {
            Vector3 pos = new Vector3(startX + i * coinSpacing, coinHeight, 0);
            Instantiate(coinPrefab, pos, Quaternion.identity);
        }
    }

    // ── Power-ups ────────────────────────────────────────────────

    void SpawnPowerUp()
    {
        GameObject[] options = new GameObject[]
        {
            powerUpShieldPrefab,
            powerUpSlowMoPrefab,
            powerUpAutoCollectPrefab
        };

        // Filter nulls
        var valid = System.Array.FindAll(options, x => x != null);
        if (valid.Length == 0) return;

        GameObject chosen = valid[Random.Range(0, valid.Length)];
        Vector3 pos = spawnPoint.position + Vector3.up * 1.5f;
        Instantiate(chosen, pos, Quaternion.identity);
    }
}
