using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathMenu : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Player player;
    [SerializeField] private GameObject deathMenuUI;

    [Header("SFX")]
    [SerializeField] private AudioClip deathClip;          // assign death.wav
    [Range(0f, 1f)][SerializeField] private float deathVolume = 0.9f;
    [SerializeField] private bool pauseOtherAudioOnDeath = true;

    private AudioSource uiSfx;        // dedicated, 2D, ignores listener pause
    private bool shown;               // UI shown once per death
    private bool soundPlayed;         // SFX played once per death

    void Awake()
    {
        // Autowire
        if (player == null) player = FindObjectOfType<Player>();
        if (deathMenuUI == null)
        {
            var go = GameObject.Find("DeathMenuUI");
            if (go != null) deathMenuUI = go;
        }
        if (deathMenuUI != null) deathMenuUI.SetActive(false);

        // Audio source just for UI SFX
        uiSfx = gameObject.AddComponent<AudioSource>();
        uiSfx.playOnAwake = false;
        uiSfx.spatialBlend = 0f;          // 2D
        uiSfx.ignoreListenerPause = true; // still plays if we pause other audio

        shown = false;
        soundPlayed = false;
    }

    void Update()
    {
        if (player == null) return;

        if (!shown && player.Health <= 0f)
        {
            ShowDeathUIAndSound();
        }
    }

    private void ShowDeathUIAndSound()
    {
        shown = true;

        if (pauseOtherAudioOnDeath)
            AudioListener.pause = true; // pauses all other AudioSources

        if (deathMenuUI != null)
            deathMenuUI.SetActive(true);

        if (!soundPlayed && deathClip != null)
        {
            uiSfx.PlayOneShot(deathClip, deathVolume);
            soundPlayed = true;
        }
    }

    public void Restart()
    {
        // Ensure game audio resumes for the new scene
        AudioListener.pause = false;
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void ReturnToMenu()
    {
        AudioListener.pause = false;
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
    }
}
