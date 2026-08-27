using UnityEngine;

[CreateAssetMenu(fileName = "BlinkAbility", menuName = "Abilities/Blink Ability")]
public class BlinkAbility : Ability
{
    [Header("Blink Settings")]
    [SerializeField] private float blinkDistance = 5f;
    [SerializeField] private LayerMask obstacleLayer;
    [SerializeField] private bool checkObstacles = true;

    protected override void Use(GameObject user)
    {
        Vector2 startPosition = user.transform.position;

        Vector2 targetDirection = GetBlinkDirection(user);

        Vector2 targetPosition = startPosition + (targetDirection * blinkDistance);

        if (checkObstacles)
        {
            RaycastHit2D hit = Physics2D.Raycast(startPosition, targetDirection, blinkDistance, obstacleLayer);
            if (hit.collider != null)
            {
                targetPosition = hit.point - (targetDirection * 0.2f);
            }
        }
        if (user.TryGetComponent<Rigidbody2D>(out Rigidbody2D rb))
        {
            rb.MovePosition(targetPosition);
        }
        else
        {
            user.transform.position = targetPosition;
        }
    }

    private Vector2 GetBlinkDirection(GameObject user)
    {
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = (mouseWorldPos - user.transform.position);

        if (direction.sqrMagnitude < 0.1f)
        {
            float facingDirection = user.transform.localScale.x >= 0 ? 1f : -1f;
            return new Vector2(facingDirection, 0f);
        }

        return direction.normalized;
    }
}
