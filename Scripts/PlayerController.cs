using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float jumpForce = 12f;
    public float doubleJumpForce = 10f;

    [Header("Audio")]
    public AudioClip jumpClip;
    public AudioClip hitClip;
    public AudioClip collectClip;

    [Header("Effects")]
    public GameObject hitParticlesPrefab;
    public GameObject collectParticlesPrefab;
    public GameObject shieldVisualPrefab;

    [HideInInspector] public bool isShielded = false;
    [HideInInspector] public bool isSlowMo = false;
    [HideInInspector] public bool isAutoCollect = false;
    [HideInInspector] public bool isDead = false;

    private Rigidbody2D rb;
    private Animator animator;
    private AudioSource audioSource;
    private bool isGrounded = false;
    private bool hasDoubleJump = false;
    private GameObject shieldVisualInstance;

    private static readonly int AnimRunning = Animator.StringToHash("isRunning");
    private static readonly int AnimJumping = Animator.StringToHash("isJumping");
    private static readonly int AnimDead = Animator.StringToHash("isDead");

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (isDead) return;

        bool jumpPressed = Input.GetKeyDown(KeyCode.Space)
                        || Input.GetKeyDown(KeyCode.UpArrow)
                        || Input.GetKeyDown(KeyCode.W)
                        || Input.GetMouseButtonDown(0);

        if (jumpPressed)
        {
            if (isGrounded)
            {
                Jump(jumpForce);
            }
            else if (hasDoubleJump)
            {
                Jump(doubleJumpForce);
                hasDoubleJump = false;
            }
        }

        if (animator != null)
        {
            animator.SetBool(AnimRunning, isGrounded);
            animator.SetBool(AnimJumping, !isGrounded);
        }
    }

    void Jump(float force)
    {
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, force);
        PlayClip(jumpClip);
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        isGrounded = true;
        hasDoubleJump = true;
    }

    void OnCollisionExit2D(Collision2D col)
    {
        isGrounded = false;
    }

    public void TakeDamage()
    {
        if (isDead) return;
        if (isShielded) { RemoveShield(); return; }
        isDead = true;
        if (animator != null) animator.SetTrigger(AnimDead);
        PlayClip(hitClip);
        SpawnEffect(hitParticlesPrefab);
        CameraShake.Instance?.Shake(0.3f, 0.25f);
        GameManager.Instance.GameOver();
    }

    public void OnCollect()
    {
        PlayClip(collectClip);
        SpawnEffect(collectParticlesPrefab);
        CameraShake.Instance?.Shake(0.1f, 0.08f);
    }

    public void ActivateShield(float duration)
    {
        if (shieldVisualInstance != null) Destroy(shieldVisualInstance);
        shieldVisualInstance = shieldVisualPrefab != null ? Instantiate(shieldVisualPrefab, transform) : null;
        isShielded = true;
        StopCoroutine(nameof(ShieldTimer));
        StartCoroutine(ShieldTimer(duration));
    }

    IEnumerator ShieldTimer(float duration)
    {
        yield return new WaitForSeconds(duration);
        RemoveShield();
    }

    void RemoveShield()
    {
        isShielded = false;
        if (shieldVisualInstance != null) Destroy(shieldVisualInstance);
    }

    public void ActivateSlowMo(float duration)
    {
        isSlowMo = true;
        Time.timeScale = 0.4f;
        Time.fixedDeltaTime = 0.02f * Time.timeScale;
        StartCoroutine(SlowMoTimer(duration));
    }

    IEnumerator SlowMoTimer(float duration)
    {
        yield return new WaitForSecondsRealtime(duration);
        Time.timeScale = 1f;
        Time.fixedDeltaTime = 0.02f;
        isSlowMo = false;
    }

    public void ActivateAutoCollect(float duration)
    {
        isAutoCollect = true;
        StartCoroutine(AutoCollectTimer(duration));
    }

    IEnumerator AutoCollectTimer(float duration)
    {
        yield return new WaitForSeconds(duration);
        isAutoCollect = false;
    }

    void PlayClip(AudioClip clip)
    {
        if (clip != null && audioSource != null)
            audioSource.PlayOneShot(clip);
    }

    void SpawnEffect(GameObject prefab)
    {
        if (prefab != null)
            Instantiate(prefab, transform.position, Quaternion.identity);
    }
}