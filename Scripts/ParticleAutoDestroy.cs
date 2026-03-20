using UnityEngine;

/// <summary>
/// Attach to any particle system prefab so it auto-destroys after playing.
/// </summary>
public class ParticleAutoDestroy : MonoBehaviour
{
    private ParticleSystem ps;

    void Awake() => ps = GetComponent<ParticleSystem>();

    void Update()
    {
        if (ps != null && !ps.IsAlive())
            Destroy(gameObject);
    }
}
