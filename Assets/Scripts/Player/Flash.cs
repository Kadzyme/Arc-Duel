using UnityEngine;
using System.Collections;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Health))]
public class Flash : MonoBehaviour
{
    [SerializeField] private float flashDuration = 0.2f;
    [SerializeField] private int flashCount = 1;  

    private Material matInstance;
    private Coroutine flashRoutine;

    private void Awake()
    {
        var sr = GetComponent<SpriteRenderer>();
        matInstance = Instantiate(sr.material);
        sr.material = matInstance;

        var health = GetComponent<Health>();
        if (health != null)
            health.onDamageTaken.AddListener(DoFlash);
    }

    private void DoFlash()
    {
        if (flashRoutine != null)
            StopCoroutine(flashRoutine);

        flashRoutine = StartCoroutine(FlashRoutine());
    }

    private IEnumerator FlashRoutine()
    {
        for (int i = 0; i < flashCount; i++)
        {
            float timer = 0f;

            while (timer < flashDuration)
            {
                timer += Time.deltaTime;
                float t = timer / flashDuration;
                matInstance.SetFloat("_FlashAmount", Mathf.Lerp(0f, 1f, t));
                yield return null;
            }

            timer = 0f;

            while (timer < flashDuration)
            {
                timer += Time.deltaTime;
                float t = timer / flashDuration;
                matInstance.SetFloat("_FlashAmount", Mathf.Lerp(1f, 0f, t));
                yield return null;
            }
        }

        matInstance.SetFloat("_FlashAmount", 0f);
    }
}
