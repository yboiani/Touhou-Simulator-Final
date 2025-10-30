using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class AISpawnManager : MonoBehaviour
{
    // Only keep the two types you want
    enum AIType { LASERAI, HEMISPHEREAI }

    [Header("Debug")]
    [SerializeField] private bool isDebug = true;
    [SerializeField] private float sphereRadiusSize = 0.5f;

    [Header("AI Variables")]
    [SerializeField] private GameObject laserAI;
    [SerializeField] private bool spawnLaserAI = true;
    [SerializeField] private GameObject fireHemisphereAI;
    [SerializeField] private bool spawnHemisphereAI = true;

    [Header("Boss")]
    [SerializeField] private GameObject bossPrefab;
    [SerializeField] private int deadCountToBossSpawn = 20;

    [Header("Variables")]
    [SerializeField] private Transform parent;          // drag "Enemies" here
    [SerializeField] private Transform playerTransform; // drag Player here
    [SerializeField] private float thresholdForBounds = 1.0f;
    [SerializeField] private float aiTime = 3.0f;

    [Header("After Boss Defeated")]
    [SerializeField] private bool reloadSceneOnBossDeath = true;
    [SerializeField] private string sceneToLoadOnBossDeath = "Level_01";

    private Vector3 bottomLeft, upperLeft, upperRight, bottomRight;
    private float aiTimer = 0.0f;
    private GameObject boss;
    private AIType aiType = AIType.LASERAI;
    private bool isBossSpawn = false;

    void Start()
    {
        Camera mainCamera = Camera.main;
        Vector3 leftThreshold = new Vector3(-1.0f * thresholdForBounds, thresholdForBounds, 0.0f);
        Vector3 rightThreshold = new Vector3(thresholdForBounds, thresholdForBounds, 0.0f);

        bottomLeft = mainCamera.ViewportToWorldPoint(new Vector3(0f, 0f, 0f)) + leftThreshold;
        upperLeft = mainCamera.ViewportToWorldPoint(new Vector3(0f, 1f, 0f)) + leftThreshold;
        upperRight = mainCamera.ViewportToWorldPoint(new Vector3(1f, 1f, 0f)) + rightThreshold;
        bottomRight = mainCamera.ViewportToWorldPoint(new Vector3(1f, 0f, 0f)) + rightThreshold;
    }

    Vector3 GetPosition()
    {
        float x = 0f, y = 0f;

        if (aiType == AIType.LASERAI)
        {
            float side = Random.Range(0f, 100f);
            if (side < 33f)           // left
            {
                x = bottomLeft.x;
                float height = upperLeft.y - bottomLeft.y;
                y = Random.Range(bottomLeft.y + (height * 0.7f), upperLeft.y);
            }
            else if (side < 66f)      // top
            {
                x = Random.Range(upperLeft.x, upperRight.x);
                y = upperLeft.y;
            }
            else                      // right
            {
                x = upperRight.x;
                float height = upperRight.y - bottomRight.y;
                y = Random.Range(bottomRight.y + (height * 0.7f), upperRight.y);
            }
        }
        else // HEMISPHEREAI
        {
            // spawn from top
            x = Random.Range(upperLeft.x + 1.5f, upperRight.x - 1.5f);
            y = upperLeft.y;
        }

        return new Vector3(x, y, 0f);
    }

    GameObject GetAI()
    {
        // 0–59: Laser, 60–99: Hemisphere (adjust weights as you like)
        float r = Random.Range(0f, 100f);

        if (r < 60f)
        {
            aiType = AIType.LASERAI;
            return spawnLaserAI ? laserAI : null;
        }
        else
        {
            aiType = AIType.HEMISPHEREAI;
            return spawnHemisphereAI ? fireHemisphereAI : null;
        }
    }

    void AISpawn()
    {
        if (!playerTransform) return;

        // Try a few times to get a valid AI based on toggles
        GameObject ai = null;
        for (int i = 0; i < 6 && ai == null; i++)
            ai = GetAI();
        if (ai == null) return;

        Vector3 aiPosition = GetPosition();
        Vector3 direction = playerTransform.position - aiPosition;

        GameObject genAI = Instantiate(ai, aiPosition, ai.transform.rotation, parent);

        if (aiType == AIType.LASERAI)
        {
            genAI.transform.Rotate(new Vector3(0f, 0f, 1f), 180f);
            genAI.transform.rotation = Quaternion.FromToRotation(genAI.transform.up, direction);
        }
    }

    bool CheckIfBossCanSpawn()
    {
        if (parent == null) return false;

        int childSize = parent.childCount;
        int deadCounter = 0;
        for (int i = 0; i < childSize; i++)
        {
            if (!parent.GetChild(i).gameObject.activeInHierarchy)
                deadCounter++;
        }
        return deadCounter >= deadCountToBossSpawn;
    }

    void Update()
    {
        if (CheckIfBossCanSpawn() && !isBossSpawn)
        {
            isBossSpawn = true;
            boss = bossPrefab ? Instantiate(bossPrefab) : null;
        }

        if (isBossSpawn && (boss == null || !boss.activeInHierarchy))
        {
            if (reloadSceneOnBossDeath)
                SceneManager.LoadScene(sceneToLoadOnBossDeath);
            else
                isBossSpawn = false;
            return;
        }

        if (!isBossSpawn)
        {
            aiTimer += Time.deltaTime;
            if (aiTimer >= aiTime)
            {
                AISpawn();
                aiTimer = 0f;
            }
        }
    }

    private void OnDrawGizmos()
    {
        if (!isDebug) return;

        Gizmos.color = Color.cyan;
        Gizmos.DrawSphere(bottomLeft, sphereRadiusSize);
        Gizmos.DrawSphere(upperLeft, sphereRadiusSize);
        Gizmos.DrawSphere(upperRight, sphereRadiusSize);
        Gizmos.DrawSphere(bottomRight, sphereRadiusSize);
    }
}
