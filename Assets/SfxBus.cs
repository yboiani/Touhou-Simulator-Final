using UnityEngine;

public class SfxBus : MonoBehaviour
{
    public static SfxBus I;
    AudioSource src;

    void Awake()
    {
        if (I != null) { Destroy(gameObject); return; }
        I = this;
        DontDestroyOnLoad(gameObject);

        src = gameObject.AddComponent<AudioSource>();
        src.playOnAwake = false;
        src.spatialBlend = 0f;   // 2D sound
        src.volume = 1f;
    }

    public void Play(AudioClip clip, float volume = 1f)
    {
        if (clip != null) src.PlayOneShot(clip, volume);
    }
}
