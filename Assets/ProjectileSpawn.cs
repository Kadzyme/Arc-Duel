using UnityEngine;

[CreateAssetMenu(menuName = "Abilities/Projectile Spawn")]
public class ProjectileSpawn : Ability
{
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private float maxDistance = 5f;

    [SerializeField] private Vector2 spawnOffset = Vector2.zero;

    protected override void Use(GameObject user)
    {
        if (projectilePrefab == null || user == null) return;

        Vector2 origin = (Vector2)user.transform.position + spawnOffset;

        Vector2 targetPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = targetPos - origin;

        if (direction.magnitude > maxDistance)
            direction = direction.normalized * maxDistance;

        Vector2 spawnPos = origin + direction;

        GameObject projectile = Instantiate(projectilePrefab, spawnPos, Quaternion.identity);

        Projectile proj = projectile.GetComponent<Projectile>();
        if (proj != null)
            proj.Init(direction.normalized);
    }
}
