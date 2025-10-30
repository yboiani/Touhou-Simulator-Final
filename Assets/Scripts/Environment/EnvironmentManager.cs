using System.Collections;
using UnityEngine;

public class EnvironmentManager : MonoBehaviour
{
    [Header("Debug")]
    [SerializeField] private bool isDebug = true;
    [SerializeField] private float sphereRadiusSize = 0.5f;

    [Header("Asteroid")]
    [SerializeField] private GameObject astroid;                 // assign your ONE asteroid prefab
    [SerializeField] private float thresholdAstroidArea = 1.0f;  // how far above the top to spawn
    [SerializeField] private float astroidTime = 3.0f;           // seconds between spawns
    private Vector3 astroidAreaLeftSide;
    private Vector3 astroidAreaRightSide;
    private float astroidTimer = 0.0f;

    [Header("Laser Hazard")]
    [SerializeField] private GameObject warningSign;             // red “WARNING” sign prefab
    [SerializeField] private GameObject laser;                   // the horizontal laser prefab
    [SerializeField] private float thresholdLaserArea = 1.0f;    // how far left of screen the laser lives
    [SerializeField] private float laserTime = 5.0f;             // seconds between lasers
    private Vector3 laserAreaTopSide;
    private Vector3 laserAreaBottomSide;
    private float laserTimer = 0.0f;

    private float upperThreshold = 1.2f;

    void Start()
    {
        Camera cam = Camera.main;

        Vector3 upperLeft = cam.ViewportToWorldPoint(new Vector3(0f, 1f, 0f));
        Vector3 upperRight = cam.ViewportToWorldPoint(new Vector3(1f, 1f, 0f));
        Vector3 lowerLeft = cam.ViewportToWorldPoint(Vector3.zero);

        // Asteroid spawn line (just above the top of screen)
        astroidAreaLeftSide = upperLeft + new Vector3(0f, thresholdAstroidArea, 0f);
        astroidAreaRightSide = upperRight + new Vector3(0f, thresholdAstroidArea, 0f);

        // Laser X position (just off the left side), full vertical span
        laserAreaTopSide = upperLeft + new Vector3(-1f * thresholdLaserArea, 0f, 0f);
        laserAreaBottomSide = lowerLeft + new Vector3(-1f * thresholdLaserArea, 0f, 0f);
    }

    void Update()
    {
        astroidTimer += Time.deltaTime;
        laserTimer += Time.deltaTime;

        if (astroidTimer >= astroidTime)
        {
            SpawnAsteroid();
            astroidTimer = 0f;
        }

        if (laserTimer >= laserTime)
        {
            StartCoroutine(SpawnLaserSequence());
            laserTimer = 0f;
        }
    }

    void SpawnAsteroid()
    {
        float x = Random.Range(astroidAreaLeftSide.x + upperThreshold,
                               astroidAreaRightSide.x - upperThreshold);
        Instantiate(astroid, new Vector3(x, astroidAreaLeftSide.y, 0f), Quaternion.identity);
    }

    IEnumerator SpawnLaserSequence()
    {
        float y = Random.Range(laserAreaBottomSide.y + upperThreshold,
                               laserAreaTopSide.y - upperThreshold);

        // show warning a moment before the actual laser
        GameObject warn = Instantiate(
            warningSign,
            new Vector3(laserAreaTopSide.x + 3f * thresholdLaserArea, y, 0f),
            Quaternion.identity
        );

        yield return new WaitForSeconds(1.5f);
        Destroy(warn);

        GameObject beam = Instantiate(
            laser,
            new Vector3(laserAreaTopSide.x, y, 0f),
            laser.transform.rotation
        );

        yield return new WaitForSeconds(3.0f);
        Destroy(beam);
    }

    void OnDrawGizmos()
    {
        if (!isDebug) return;

        Gizmos.color = Color.green;
        Gizmos.DrawSphere(astroidAreaLeftSide, sphereRadiusSize);
        Gizmos.DrawSphere(astroidAreaRightSide, sphereRadiusSize);
        Gizmos.DrawSphere(laserAreaBottomSide, sphereRadiusSize);
        Gizmos.DrawSphere(laserAreaTopSide, sphereRadiusSize);
    }
}
