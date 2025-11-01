using UnityEngine;

public class LimitedLifetime : MonoBehaviour
{
    [SerializeField] private float lifetime = 3f;

    private void Update()
    {
        lifetime -= Time.deltaTime;
        if (lifetime <= 0f)
            Destroy(gameObject);
    }
}
