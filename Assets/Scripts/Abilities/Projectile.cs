using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
public class Projectile : MonoBehaviour
{
    [SerializeField] private float speed = 10f;
    [SerializeField] private LayerMask hitLayers;

    private Rigidbody2D rb;

    public void Init(Vector2 dir, GameObject owner)
    {
        rb = GetComponent<Rigidbody2D>();

        if (rb.bodyType != RigidbodyType2D.Static)
            rb.linearVelocity = dir.normalized * speed;

        StartCoroutine(RemoveIgnoreAfterDelay(owner.GetComponent<Collider2D>(), 0.5f));
    }

    private IEnumerator RemoveIgnoreAfterDelay(Collider2D userCol, float delay)
    {
        yield return new WaitForSeconds(delay);

        var collider = GetComponent<Collider2D>();

        if (collider != null)
            Physics2D.IgnoreCollision(collider, userCol, false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (((1 << collision.gameObject.layer) & hitLayers) != 0)
            Destroy(gameObject);
    }
}
