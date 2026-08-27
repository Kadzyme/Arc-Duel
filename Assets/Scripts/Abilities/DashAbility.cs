using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "DashAbility", menuName = "Abilities/Dash Ability")]
public class DashAbility : Ability
{
    [Header("Dash Settings")]
    [SerializeField] private float dashDistance = 1f;
    [SerializeField] private float dashSpeed = 3f;
    [SerializeField] private LayerMask obstacleLayer;
    [SerializeField] private bool checkObstacles = true;

    protected override void Use(GameObject user)
    {
        Vector2 startPosition = user.transform.position;
        Vector2 targetDirection = GetDashDirection(user);
        Vector2 targetPosition = startPosition + (targetDirection * dashDistance);

        if (checkObstacles)
        {
            Vector2 characterSize = Vector2.one;

            Collider2D col = user.GetComponent<Collider2D>();
            if (col != null)
            {
                characterSize = col.bounds.size;
            }

            RaycastHit2D hit = Physics2D.BoxCast(
                startPosition,
                characterSize,
                0f,
                targetDirection,
                dashDistance,
                obstacleLayer
            );

            if (hit.collider != null)
            {
                float skinWidth = 0.05f;
                float offsetDistance = Mathf.Abs(targetDirection.x) > Mathf.Abs(targetDirection.y)
                    ? (characterSize.x / 2f) + skinWidth
                    : (characterSize.y / 2f) + skinWidth;

                targetPosition = hit.point - (targetDirection * offsetDistance);
            }
        }

        DashRunner runner = user.GetComponent<DashRunner>();
        if (runner == null)
        {
            runner = user.AddComponent<DashRunner>();
        }

        runner.StartDash(user, targetPosition, dashSpeed);
    }

    private Vector2 GetDashDirection(GameObject user)
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

    private class DashRunner : MonoBehaviour
    {
        public void StartDash(GameObject user, Vector2 targetPosition, float speed)
        {
            StopAllCoroutines();
            StartCoroutine(PerformDash(user, targetPosition, speed));
        }

        private IEnumerator PerformDash(GameObject user, Vector2 targetPosition, float speed)
        {
            Rigidbody2D rb = user.GetComponent<Rigidbody2D>();
            bool hasRb = rb != null;

            float originalGravity = hasRb ? rb.gravityScale : 0f;
            if (hasRb)
            {
                rb.gravityScale = 0f;
                rb.linearVelocity = Vector2.zero;
            }

            Vector2 startPos = user.transform.position;
            float distance = Vector2.Distance(startPos, targetPosition);

            if (distance <= 0.01f) yield break;

            float journeyDuration = distance / speed;
            float elapsedTime = 0f;

            while (elapsedTime < journeyDuration)
            {
                elapsedTime += Time.deltaTime;
                float t = Mathf.Clamp01(elapsedTime / journeyDuration);
                Vector2 newPos = Vector2.Lerp(startPos, targetPosition, t);

                if (hasRb)
                {
                    rb.MovePosition(newPos);
                }
                else
                {
                    user.transform.position = newPos;
                }

                yield return null;
            }

            if (hasRb)
            {
                rb.MovePosition(targetPosition);
                rb.gravityScale = originalGravity;
                rb.linearVelocity = Vector2.zero;
            }
            else
            {
                user.transform.position = targetPosition;
            }
        }
    }
}