using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
public class Projectile : MonoBehaviour
{
    [SerializeField] private float speed = 10f;
    [SerializeField] private LayerMask hitLayers;
    [SerializeField] private bool ignoreOwner = true;

    private Rigidbody2D rb;
    private GameObject owner;

    public void Init(Vector2 dir, GameObject owner)
    {
        this.owner = owner;

        rb = GetComponent<Rigidbody2D>();

        rb.linearVelocity = dir.normalized * speed;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (ignoreOwner && collision.gameObject == owner)
            return;

        if (((1 << collision.gameObject.layer) & hitLayers) != 0)
            Destroy(gameObject);
    }
}
