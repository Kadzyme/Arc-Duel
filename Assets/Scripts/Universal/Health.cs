using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider2D))]
public class Health : MonoBehaviour
{
    [SerializeField] private float maxHealth = 100f;

    [HideInInspector] public UnityEvent onDamageTaken = new();

    private float currentHealth;

    private void Awake() => currentHealth = maxHealth;

    public void TakeDamage(float amount)
    {
        if (currentHealth <= 0) return;

        currentHealth -= amount;

        if (currentHealth > 0)
            onDamageTaken?.Invoke();
        else
            Destroy(gameObject);
    }

    public void Heal(float amount)
        => currentHealth = Mathf.Min(currentHealth + amount, maxHealth);

    public float CurrentHealth => currentHealth;
    public float MaxHealth => maxHealth;
}
