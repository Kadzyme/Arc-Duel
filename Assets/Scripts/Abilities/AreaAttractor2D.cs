using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CircleCollider2D))]
public class AreaAttractor2D : MonoBehaviour
{
    public float pullForce = 200f;
    public float spinForce = 50f;
    public float minDistance = 0.3f;
    public float radius = 3f;
    public LayerMask affectedLayers = ~0;
    public bool ignoreKinematic = true;

    private readonly HashSet<Rigidbody2D> _bodies = new();
    private CircleCollider2D _trigger;

    void Reset()
    {
        var col = GetComponent<CircleCollider2D>();
        col.isTrigger = true;
        col.radius = radius;
    }

    void Awake()
    {
        _trigger = GetComponent<CircleCollider2D>();
        _trigger.isTrigger = true;
        _trigger.radius = radius;
    }

    void FixedUpdate()
    {
        if (_bodies.Count == 0) return;

        Vector2 center = transform.position;
        foreach (var rb in _bodies)
        {
            if (rb == null) continue;
            if (ignoreKinematic && rb.bodyType == RigidbodyType2D.Kinematic) continue;

            Vector2 toCenter = center - rb.position;
            float distance = toCenter.magnitude;
            if (distance < 0.0001f) distance = minDistance;
            if (distance > radius) continue;

            float t = 1f - Mathf.Clamp01(distance / radius);
            float forceMagnitude = pullForce * t;

            Vector2 radialForce = toCenter.normalized * forceMagnitude * Time.fixedDeltaTime;
            rb.AddForce(radialForce, ForceMode2D.Force);

            Vector2 tangent = new Vector2(-toCenter.y, toCenter.x).normalized;
            Vector2 spin = tangent * spinForce * t * Time.fixedDeltaTime;
            rb.AddForce(spin, ForceMode2D.Force);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (((1 << other.gameObject.layer) & affectedLayers) == 0) return;
        var rb = other.attachedRigidbody;
        if (rb != null)
            _bodies.Add(rb);
    }

    void OnTriggerExit2D(Collider2D other)
    {
        var rb = other.attachedRigidbody;
        if (rb != null)
            _bodies.Remove(rb);
    }

    void OnDisable()
    {
        _bodies.Clear();
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(0.2f, 0.6f, 1f, 0.25f);
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
