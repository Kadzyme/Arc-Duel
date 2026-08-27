using UnityEngine;

public class LimitedLifetime : MonoBehaviour
{
    [SerializeField] private float lifetime = 3f;

    public void SetLifeTime(float newLifetime)
        => lifetime = newLifetime;

    private void Update()
    {
        lifetime -= Time.deltaTime;
        if (lifetime <= 0f)
            Destroy(gameObject);
    }

    public void DestroySelf()
        => Destroy(gameObject);
}
