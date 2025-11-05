using UnityEngine;

[CreateAssetMenu(menuName = "Abilities/Projectile Spawn")]
public class ProjectileSpawn : Ability
{
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private float maxDistance = 0;
    [SerializeField] private bool spawnAsChild = false;
    [SerializeField] private bool usePlayerDirection = true;

    protected override void Use(GameObject user)
    {
        if (projectilePrefab == null || user == null) return;

        Vector2 origin = user.transform.position;
        Vector2 targetPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = targetPos - origin;

        if (direction.magnitude > maxDistance)
            direction = direction.normalized * maxDistance;

        Vector2 spawnPos = origin + direction;

        Quaternion rotation;
        Vector2 projectileDirection;

        if (usePlayerDirection)
        {
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            float baseRotation = projectilePrefab.transform.rotation.eulerAngles.z;
            rotation = Quaternion.Euler(0, 0, angle + baseRotation);
            projectileDirection = direction.normalized;
        }
        else
        {
            rotation = projectilePrefab.transform.rotation;
            projectileDirection = projectilePrefab.transform.right.normalized;
        }

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
            proj.Init(projectileDirection, user);
    }
}
