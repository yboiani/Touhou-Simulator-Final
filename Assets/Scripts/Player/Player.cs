using System.Collections;
using UnityEngine;

[RequireComponent(typeof(PlayerController))]
public class Player : MonoBehaviour
{
    [ColorUsageAttribute(true,true)]
    [SerializeField] private Color damagedColor = Color.red;
    [ColorUsageAttribute(true,true)]
    [SerializeField] private Color defaultColor = Color.white;
    [SerializeField] float speed = 10.0f;
    [SerializeField] private float maxHealth = 100.0f;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private WanderTimer.TimerUtility timer;

    public HealthBar healthBar;

    public float Health = 100.0f;

    public bool isDead = false;

    private Animator animator;
    private static readonly int Explosion = Animator.StringToHash("expl");

    private void Awake()
    {
        Health = maxHealth;
    }

    public float Speed
    {
        get => speed;
        set => speed = value;
    }

    void CheckDeath()
    {
        if (Health <= 0.0f)
        {
            isDead = true;
            StartExplosion();
            StartCoroutine(Destroy());
        }
    }

    IEnumerator Destroy()
    {
        yield return new WaitForSeconds(0.2f);
        Destroy(this.gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        if (healthBar)
        {
            healthBar.SetMaxHealth(maxHealth);
        }

        animator = GetComponent<Animator>();
        timer.ForceDone();
    }

    void StartExplosion()
    {
        if (!animator)
        {
            animator = GetComponent<Animator>();
        }
        animator.SetBool(Explosion, true);
    }

    // Update is called once per frame
    void Update()
    {
        CheckDeath();

        if (healthBar)
        {
            healthBar.SetHealth(Health);
        }

        spriteRenderer.color = Color.Lerp(damagedColor, defaultColor, timer.NormalizedTime * timer.NormalizedTime);
        timer.Update(Time.deltaTime);
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        GameObject go = other.gameObject;
        if (go.layer == BLLayers.environmentLaser)
        {
            Health -= 0.3f;
        }
    }

    public void ChangeColorOnDamageDealt()
    {
        timer.Restart();
    }
}
