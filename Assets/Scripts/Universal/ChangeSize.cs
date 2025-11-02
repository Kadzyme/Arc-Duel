using UnityEngine;

public class ChangeSize : MonoBehaviour
{
    [SerializeField] private float scaleInPercent;

    private Vector3 additionalSize;

    private void Start()
    {
        additionalSize = transform.localScale * scaleInPercent;
    }

    private void FixedUpdate()
    {
        transform.localScale = transform.localScale + (additionalSize * Time.fixedDeltaTime);
    }
}
