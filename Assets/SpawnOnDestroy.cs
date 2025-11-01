using UnityEngine;
using System.Collections;

public class SpawnOnDestroy : MonoBehaviour
{
    [SerializeField] private GameObject spawnPrefab;
    [SerializeField] private bool inheritDirection = false;
    [SerializeField] private float delay = 0f;

    private bool isQuitting;

    private void OnApplicationQuit() => isQuitting = true;

    private void OnDestroy()
    {
        if (isQuitting || spawnPrefab == null)
            return;

        if (delay > 0f)
            StartCoroutine(SpawnWithDelay());
        else
            SpawnNow();
    }

    private void SpawnNow()
    {
        Quaternion rotation = Quaternion.identity;

        if (inheritDirection)
        {
            Vector2 dir = transform.right.normalized;
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            rotation = Quaternion.Euler(0f, 0f, angle);
        }

        Instantiate(spawnPrefab, transform.position, rotation);
    }

    private IEnumerator SpawnWithDelay()
    {
        yield return new WaitForSeconds(delay);
        SpawnNow();
    }
}
