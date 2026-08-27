using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider2D))]
public class Health : MonoBehaviour
{
    [SerializeField] private float maxHealth = 100f;

    [HideInInspector] public UnityEvent onDamageTaken = new();

    private float currentHealth;

    private void Awake() 
        => currentHealth = maxHealth;

    public void TakeDamage(float amount)
    {
        currentHealth -= amount;

        Debug.Log($"Current HP: {currentHealth}");

        onDamageTaken?.Invoke();

        if (currentHealth < 0)
            Global.SafeDestroyProjectile(gameObject);
    }

    public void Heal(float amount)
        => currentHealth = Mathf.Min(currentHealth + amount, maxHealth);
}
