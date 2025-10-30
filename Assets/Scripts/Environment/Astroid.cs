using UnityEngine;

public class Astroid : MonoBehaviour
{
    void Update()
    {
        if (!ViewportManager.Instance.IsInsideViewport(this.transform.position, 5.0f))
        {
            Destroy(this.gameObject);
        }
    }
    
    private void OnTriggerEnter2D(Collider2D col)
    {
        GameObject colGO = col.gameObject;
        
        if (colGO.layer == BLLayers.player)
        {
            colGO.GetComponent<Player>().Health -= 300.0f;
        }
        
        if (colGO.layer == BLLayers.playerBullet || colGO.layer == BLLayers.enemyBullet)
        {
            colGO.SetActive(false);
        }
    }
}
