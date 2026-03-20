using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// Attach to a UI panel. Shows which power-ups are currently active.
/// Link the three indicator GameObjects in the inspector.
/// </summary>
public class PowerUpUI : MonoBehaviour
{
    public GameObject shieldIndicator;
    public GameObject slowMoIndicator;
    public GameObject autoCollectIndicator;

    private PlayerController player;

    void Start()
    {
        GameObject p = GameObject.FindGameObjectWithTag("Player");
        if (p != null) player = p.GetComponent<PlayerController>();
    }

    void Update()
    {
        if (player == null) return;

        if (shieldIndicator      != null) shieldIndicator.SetActive(player.isShielded);
        if (slowMoIndicator      != null) slowMoIndicator.SetActive(player.isSlowMo);
        if (autoCollectIndicator != null) autoCollectIndicator.SetActive(player.isAutoCollect);
    }
}
