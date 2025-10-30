using UnityEngine;

public class EnemyDeathSFX : MonoBehaviour
{
    [Header("Clip")]
    [SerializeField] private AudioClip explodeClip;
    [Range(0f, 1f)] public float volume = 1f;

    [Header("Filter (optional)")]
    [SerializeField] private bool onlyForBoss = false;
    [SerializeField] private string bossTag = "Boss";

    bool alreadyPlayed;

    void TryPlay()
    {
        if (alreadyPlayed) return;
        if (onlyForBoss && !CompareTag(bossTag)) return;
        if (SfxBus.I != null && explodeClip != null)
            SfxBus.I.Play(explodeClip, volume);
        alreadyPlayed = true;
    }

    // Covers pooled (SetActive(false)) and destroyed objects
    void OnDisable()
    {
        // Only play if the scene is valid (prevents prefab edit window from firing)
        if (gameObject.scene.IsValid() && gameObject.scene.isLoaded) TryPlay();
    }

    void OnDestroy()
    {
        TryPlay();
    }
}
