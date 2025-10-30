using UnityEngine;

[RequireComponent(typeof(Enemy))]
public class LaserEnemy : MonoBehaviour
{
    [SerializeField] private float damage = 10.0f;
    [SerializeField] private float firerate = 1.0f;

    private Enemy _enemyBase;

    private float elapsedTime = 0.0f;
    // Start is called before the first frame update
    void Start()
    {
        _enemyBase = GetComponent<Enemy>();
    }

    void Fire()
    {
        if (_enemyBase.isDead)
        {
            return;
        }
        
        Vector3 rotationEuler = this.transform.rotation.eulerAngles;
        Quaternion laserRotation = Quaternion.Euler(rotationEuler.x, rotationEuler.y, rotationEuler.z - 270);

        EnemyBullet enemyBullet = BulletPool.Instance.GetEnemyLaserBullet();
        if (!enemyBullet) return;
        
        enemyBullet.transform.position = this.transform.position;
        enemyBullet.transform.rotation = laserRotation;
        enemyBullet.MoveDirection = this.transform.up;
        enemyBullet.damage = damage;
        enemyBullet.gameObject.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        elapsedTime += Time.deltaTime;

        if (elapsedTime > firerate)
        {
            Fire();
            elapsedTime = 0.0f;
        }
    }
}
