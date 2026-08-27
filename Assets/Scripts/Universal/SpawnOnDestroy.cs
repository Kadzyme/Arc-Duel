using UnityEngine;
using System.Collections;

public class SpawnOnDestroy : MonoBehaviour
{
    [SerializeField] private GameObject spawnPrefab;
    [SerializeField] private bool inheritDirection = false;
    [SerializeField] private bool inheritVelocity = false;

    private bool isQuitting;
    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void OnApplicationQuit() => isQuitting = true;

    private void OnDestroy()
    {
        if (isQuitting || spawnPrefab == null)
            return;

        SpawnNow();
    }

    private void SpawnNow()
    {
        Vector2 fireballVelocity = Vector2.zero;
        if (rb != null)
        {
            fireballVelocity = rb.linearVelocity;
        }

        Quaternion rotation = Quaternion.identity;
        if (inheritDirection)
        {
            Vector2 dir = (inheritVelocity && fireballVelocity.sqrMagnitude > 0.01f)
                ? fireballVelocity.normalized
                : transform.right.normalized;

            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            rotation = Quaternion.Euler(0f, 0f, angle);
        }

        GameObject spawnedObj = Instantiate(spawnPrefab, transform.position, rotation);

        if (inheritVelocity)
        {
            ParticleSystem ps = spawnedObj.GetComponent<ParticleSystem>();
            if (ps != null)
            {
                var main = ps.main;
                main.simulationSpace = ParticleSystemSimulationSpace.World;

                var velOverLife = ps.velocityOverLifetime;
                velOverLife.enabled = true;

                velOverLife.x = new ParticleSystem.MinMaxCurve(fireballVelocity.x * 0.5f);
                velOverLife.y = new ParticleSystem.MinMaxCurve(fireballVelocity.y * 0.5f);
            }
        }
    }
}