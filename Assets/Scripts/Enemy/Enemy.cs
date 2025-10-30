using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float speed = 10.0f;
    [SerializeField] private float maxHealth = 100.0f;
    [SerializeField] [Range(0.0f, 100.0f)] private float percentageSpeed = 1.0f;
    [SerializeField] private bool isBoss = false;
    [SerializeField] private float viewportThreshold = 1.0f;

    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private WanderTimer.TimerUtility timer;
    [SerializeField] private Color damagedColor = Color.red;
    [SerializeField] private Color defaultColor = Color.white;

    private float health = 100.0f;

    private Animator animator;
    private static readonly int Explosion = Animator.StringToHash("expl");

    [HideInInspector] public bool isDead = false;

    private void Awake()
    {
        health = maxHealth;
        animator = GetComponent<Animator>();
        timer.ForceDone();
    }

    void CheckDeath()
    {
        if (health <= 0.0f)
        {
            isDead = true;
            StartCoroutine(TimedDeactivate());
            StartExplosion();
        }

        if (!ViewportManager.Instance.IsInsideViewport(this.transform.position, 10.0f))
        {
            Destroy(this.gameObject);
        }
    }

    IEnumerator TimedDeactivate()
    {
        yield return new WaitForSeconds(0.5f);
        this.gameObject.SetActive(false);
    }
    
    void GetInViewport()
    {
        Vector3 position = this.transform.position;
        if (ViewportManager.Instance.IsInsideViewport(position, -1.0f * viewportThreshold))
        {
            if (!isBoss)
            {
                transform.position += transform.up * (speed * (percentageSpeed / 100.0f) * Time.deltaTime);
            }
            return;
        }
        
        transform.position += transform.up * (speed * Time.deltaTime);
    }

    // Update is called once per frame
    void Update()
    {
        CheckDeath();
        GetInViewport();

        spriteRenderer.color = Color.Lerp(damagedColor, defaultColor, timer.NormalizedTime);
        timer.Update(Time.deltaTime);

        if (isBoss)
        {
            Debug.Log(health);
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        GameObject colGO = col.gameObject;
        if (colGO.layer == BLLayers.playerBullet || colGO.layer == BLLayers.ballisticBullet)
        {
            health -= colGO.GetComponent<PlayerBullet>().damage;
            this.ChangeColorOnDamageDealt();
            col.gameObject.SetActive(false);
        }
    }

    private void OnTriggerStay2D(Collider2D col)
    {
        if (col.gameObject.layer == BLLayers.playerLaser)
        {
            ChangeColorOnDamageDealt();
            health -= 3.0f;
        }
    }

    public void ChangeColorOnDamageDealt()
    {
        timer.Restart();
    }

    void StartExplosion()
    {
        if (!animator)
        {
            animator = GetComponent<Animator>();
        }
        animator.SetBool(Explosion, true);
    }
}
