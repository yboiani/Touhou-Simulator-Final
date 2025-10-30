using System;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float minSpeed = 10.0f;
    [SerializeField] private float boundaryThreshold = 0.69f;
    [SerializeField] private float speedIncDecSize = 0.5f;
    [SerializeField] private float firerate = 0.2f;

    [Header("Laser")]
    [SerializeField] private float laserTimeToAlive = 2.0f;
    [SerializeField] private Transform fireToLaser;
    [SerializeField] private Transform laserTransform;
    [SerializeField] private LineRenderer laserRenderer;
    [SerializeField] private BoxCollider2D laserCollider;

    private float laserElapsedTime = 0.0f;
    private Player _player;
    private float timestamp = Mathf.Infinity;
    private uint ballisticCounter = 0;
    public uint BallisticCounter => ballisticCounter;
    private uint playerLaserCounter = 0;
    public uint PlayerLaserCounter => playerLaserCounter;

    // Start is called before the first frame update
    void Start()
    {
        _player = GetComponent<Player>();
        laserRenderer.enabled = false;
        laserCollider.enabled = false;
    }

    void ControlSpeed()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            _player.Speed += speedIncDecSize;
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            _player.Speed = _player.Speed - speedIncDecSize < minSpeed ? minSpeed : _player.Speed - speedIncDecSize;
        }
    }

    Vector3 GetNewPosition(Vector3 currentPosition)
    {
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            currentPosition.x += (_player.Speed * Time.deltaTime);
        }

        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            currentPosition.x -= (_player.Speed * Time.deltaTime);
        }

        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
        {
            currentPosition.y += (_player.Speed * Time.deltaTime);
        }

        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
        {
            currentPosition.y -= (_player.Speed * Time.deltaTime);
        }

        ViewportManager.Instance.ClampInViewport(ref currentPosition, boundaryThreshold);

        return currentPosition;
    }

    void FireBallistics()
    {
        float angleStep = 360.0f / 5.0f;
        float angle = 0.0f;
        Vector3 pos = this.transform.position;

        for (int i = 0; i < 5; i++)
        {
            float bulletDirX = pos.x + Mathf.Sin((angle * Mathf.PI) / 180.0f);
            float bulletDirY = pos.y + Mathf.Cos((angle * Mathf.PI) / 180.0f);

            Vector3 bulletMoveVector = new Vector3(bulletDirX, bulletDirY, 0.0f);
            Vector3 bulletDirection = (bulletMoveVector - pos).normalized;

            Quaternion laserRotation = Quaternion.FromToRotation(this.transform.up, bulletDirection);

            PlayerBullet ballisticBullet = BulletPool.Instance.GetBallisticBullet();
            if (!ballisticBullet) return;

            ballisticBullet.transform.position = pos;
            ballisticBullet.transform.rotation = laserRotation;
            ballisticBullet.MoveDirection = bulletDirection;
            ballisticBullet.gameObject.SetActive(true);

            angle += angleStep;
        }
    }

    void FireLaser()
    {
        laserTransform.position = fireToLaser.position;
        laserRenderer.enabled = true;
        laserCollider.enabled = true;
    }

    void CheckFire()
    {
        if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space))
        {
            timestamp = Time.time + firerate;
        }

        if (Input.GetMouseButtonUp(0) || Input.GetKeyUp(KeyCode.Space))
        {
            timestamp = Mathf.Infinity;
        }

        if (Time.time >= timestamp)
        {
            PlayerBullet bullet = BulletPool.Instance.GetPlayerBullet();
            if (!bullet) return;
            bullet.transform.position = this.transform.position;
            bullet.gameObject.SetActive(true);
            timestamp = Time.time + firerate;
        }
    }

    void CheckActions()
    {
        if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space))
        {
            PlayerBullet bullet = BulletPool.Instance.GetPlayerBullet();
            if (!bullet) return;
            bullet.transform.position = this.transform.position;
            bullet.gameObject.SetActive(true);
            timestamp = Time.time + firerate;
        }

        CheckFire();

        if (Input.GetKeyDown(KeyCode.B))
        {
            if (ballisticCounter == 0)
            {
                return;
            }
            FireBallistics();
            ballisticCounter--;
        }

        if (Input.GetKeyDown(KeyCode.L))
        {
            if (playerLaserCounter == 0)
            {
                return;
            }
            FireLaser();
            playerLaserCounter--;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (laserRenderer.enabled)
        {
            laserElapsedTime += Time.deltaTime;

            if (_player.isDead)
            {
                laserRenderer.enabled = false;
                laserCollider.enabled = false;
                return;
            }

            if (laserElapsedTime < laserTimeToAlive)
            {
                return;
            }

            laserRenderer.enabled = false;
            laserCollider.enabled = false;
            laserElapsedTime = 0.0f;
        }

        CheckActions();
        // ControlSpeed();
        this.transform.position = GetNewPosition(this.transform.position);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        GameObject go = other.gameObject;

        if (go.layer != BLLayers.pickup)
        {
            return;
        }

        if (go.CompareTag("BallisticPickup"))
        {
            ballisticCounter++;
        }

        if (go.CompareTag("PlayerLaserPickup"))
        {
            playerLaserCounter++;
        }

        go.SetActive(false);
    }
}// end of PlayerController
