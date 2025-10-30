// PlayerBullet.cs
using UnityEngine;

public class PlayerBullet : MonoBehaviour
{
    [SerializeField] private float speed = 150.0f;

    public float damage = 10.0f;
    public Vector3 MoveDirection = Vector3.up;

    [Header("SFX")]
    [SerializeField] private AudioClip pewClip;
    [Range(0f, 1f)] public float pewVolume = 0.75f;

    // Skip SFX if we were enabled during the first frames of the scene
    const float BOOT_SILENCE = 0.15f;

    void OnEnable()
    {
        if (!Application.isPlaying) return;
        if (Time.timeSinceLevelLoad < BOOT_SILENCE) return; // <-- key line
        if (pewClip != null && Camera.main != null)
            AudioSource.PlayClipAtPoint(pewClip, Camera.main.transform.position, pewVolume);
    }

    void Update()
    {
        transform.position += MoveDirection * (speed * Time.deltaTime);
        Vector3 p = transform.position;
        if (!ViewportManager.Instance.IsInsideViewport(p, 1.0f))
            gameObject.SetActive(false);
    }
}
