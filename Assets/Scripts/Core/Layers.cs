using UnityEngine;

public static class BLLayers
{
    public static int background = LayerMask.NameToLayer("Background");
    public static int player = LayerMask.NameToLayer("Player");
    public static int playerBullet = LayerMask.NameToLayer("PlayerBullet");
    public static int enemyBullet = LayerMask.NameToLayer("EnemyBullet");
    public static int enemy = LayerMask.NameToLayer("Enemy");
    public static int playerLaser = LayerMask.NameToLayer("PlayerLaser");
    public static int environmentLaser = LayerMask.NameToLayer("EnvironmentLaser");
    public static int pickup = LayerMask.NameToLayer("Pickups");
    public static int ballisticBullet = LayerMask.NameToLayer("BallisticBullet");
}
