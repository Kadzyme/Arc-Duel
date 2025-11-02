using UnityEngine;

[CreateAssetMenu(menuName = "Abilities/Projectile Spawn")]
public class ProjectileSpawn : Ability
{
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private float maxDistance = 0;
    [SerializeField] private bool spawnAsChild = false;

    protected override void Use(GameObject user)
    {
        if (projectilePrefab == null || user == null) return;

        Vector2 origin = user.transform.position;
        Vector2 targetPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = targetPos - origin;

        if (direction.magnitude > maxDistance)
            direction = direction.normalized * maxDistance;

        Vector2 spawnPos = origin + direction;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        float baseRotation = projectilePrefab.transform.rotation.eulerAngles.z;
        Quaternion rotation = Quaternion.Euler(0, 0, angle + baseRotation);

        GameObject projectile = Instantiate(
            projectilePrefab,
            spawnPos,
            rotation,
            spawnAsChild ? user.transform : null
        );

        Collider2D userCol = user.GetComponent<Collider2D>();
        Collider2D projCol = projectile.GetComponent<Collider2D>();
        if (userCol && projCol)
            Physics2D.IgnoreCollision(projCol, userCol);

        Projectile proj = projectile.GetComponent<Projectile>();
        if (proj != null)
            proj.Init(direction.normalized, user);
    }
}
