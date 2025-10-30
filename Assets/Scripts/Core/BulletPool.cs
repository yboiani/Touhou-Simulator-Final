using System.Collections.Generic;
using UnityEngine;

public class BulletPool : MonoBehaviour
{
    public static BulletPool Instance;

    [SerializeField] private Transform parent;
    
    [SerializeField] private PlayerBullet playerBullet;
    [SerializeField] private int playerBulletAmount = 8;
    [SerializeField] private PlayerBullet ballisticBullet;
    [SerializeField] private int ballisticBulletAmount = 16;
    [SerializeField] private EnemyBullet enemyLaserBullet;
    [SerializeField] private int enemyLaserAmount = 8;
    [SerializeField] private EnemyBullet enemyCircleBullet;
    [SerializeField] private int enemyCircleAmount = 8;
    
    private List<PlayerBullet> playerBulletObjects = new List<PlayerBullet>();
    private List<PlayerBullet> ballisticBulletObjects = new List<PlayerBullet>();
    private List<EnemyBullet> enemyLaserObjects = new List<EnemyBullet>();
    private List<EnemyBullet> enemyCircleObjects = new List<EnemyBullet>();

    private void Awake()
    {
        Instance = this;
    }

    void CreatePlayerBullets()
    {
        for (int i = 0; i < playerBulletAmount; i++)
        {
            PlayerBullet temp = Instantiate(playerBullet, parent);
            temp.gameObject.SetActive(false);
            playerBulletObjects.Add(temp);
        }

        for (int i = 0; i < ballisticBulletAmount; i++)
        {
            PlayerBullet temp = Instantiate(ballisticBullet, parent);
            temp.gameObject.SetActive(false);
            ballisticBulletObjects.Add(temp);
        }
    }

    void CreateEnemyBullets()
    {
        for (int i = 0; i < enemyLaserAmount; i++)
        {
            EnemyBullet temp = Instantiate(enemyLaserBullet, parent);
            temp.gameObject.SetActive(false);
            enemyLaserObjects.Add(temp);
        }

        for (int i = 0; i < enemyCircleAmount; i++)
        {
            EnemyBullet temp = Instantiate(enemyCircleBullet, parent);
            temp.gameObject.SetActive(false);
            enemyCircleObjects.Add(temp);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        CreatePlayerBullets();
        CreateEnemyBullets();
    }

    public PlayerBullet GetPlayerBullet()
    {
        foreach (PlayerBullet pb in playerBulletObjects)
        {
            if (!pb.gameObject.activeInHierarchy)
            {
                return pb;
            }
        }

        return null;
    }

    public PlayerBullet GetBallisticBullet()
    {
        foreach (PlayerBullet pb in ballisticBulletObjects)
        {
            if (!pb.gameObject.activeInHierarchy)
            {
                return pb;
            }
        }

        return null;
    }

    public EnemyBullet GetEnemyLaserBullet()
    {
        foreach (EnemyBullet eb in enemyLaserObjects)
        {
            if (!eb.gameObject.activeInHierarchy)
            {
                return eb;
            }
        }

        return null;
    }

    public EnemyBullet GetEnemyCircleBullet()
    {
        foreach (EnemyBullet eb in enemyCircleObjects)
        {
            if (!eb.gameObject.activeInHierarchy)
            {
                return eb;
            }
        }

        return null;
    }
}
