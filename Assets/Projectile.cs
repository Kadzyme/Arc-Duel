using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float speed = 5f;
    [SerializeField] private float lifetime = 5f;
    [SerializeField] private LayerMask destroyOnCollisionLayers;
    [SerializeField] private float damage = 1f;

    private Vector2 direction;
    private float spawnTime;

    public void Init(Vector2 dir)
    {
        direction = dir.normalized;
        spawnTime = Time.time;
    }

    private void Update()
    {
        transform.position += (Vector3)(speed * Time.deltaTime * direction);

        if (Time.time - spawnTime >= lifetime)
            Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (((1 << collision.gameObject.layer) & destroyOnCollisionLayers) != 0)
        {
            Destroy(gameObject);
        }
    }
}
