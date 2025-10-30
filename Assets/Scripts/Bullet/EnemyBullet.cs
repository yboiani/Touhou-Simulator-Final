using System.Collections;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    [SerializeField] private float speed = 100.0f;

    public float damage = 10.0f;
    public Vector3 MoveDirection = Vector3.down;
    

    void CheckOutside()
    {
        Vector3 position = this.transform.position;
        if (!ViewportManager.Instance.IsInsideViewport(position, 1.0f))
        {
            this.gameObject.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.position += MoveDirection * (speed * Time.deltaTime);
        CheckOutside();
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.layer == BLLayers.playerBullet)
        {
            col.gameObject.SetActive(false);
            this.gameObject.SetActive(false);
        }

        if (col.gameObject.layer == BLLayers.player)
        {
            col.gameObject.GetComponent<Player>().Health -= damage;
            col.gameObject.GetComponent<Player>().ChangeColorOnDamageDealt();
            this.gameObject.SetActive(false);
        }
    }
}