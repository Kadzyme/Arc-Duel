using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [Header("÷ель")]
    [SerializeField] private Transform target;

    [Header("—глаживание")]
    [SerializeField] private float smoothTime = 0.2f;
    [SerializeField] private Vector3 offset = new(0, 0.2f, -10);

    [Header("Dead Zone")]
    [SerializeField] private Vector2 deadZone = new(0.125f, 0.05f);

    private Vector3 velocity = Vector3.zero;

    private void LateUpdate()
    {
        if (target == null) return;

        Vector3 targetPos = target.position + offset;

        Vector3 delta = targetPos - transform.position;

        if (Mathf.Abs(delta.x) < deadZone.x) delta.x = 0;
        else delta.x -= Mathf.Sign(delta.x) * deadZone.x;

        if (Mathf.Abs(delta.y) < deadZone.y) delta.y = 0;
        else delta.y -= Mathf.Sign(delta.y) * deadZone.y;

        Vector3 desiredPosition = transform.position + delta;
        transform.position = Vector3.SmoothDamp(transform.position, desiredPosition, ref velocity, smoothTime);
    }
}
