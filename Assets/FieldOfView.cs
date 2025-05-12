using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldOfView : MonoBehaviour
{
    public float radius = 5f;
    [Range(1, 360)] public float angle = 45f;
    public LayerMask targetEnemy;
    public LayerMask obstructionLayer;
    public List<Transform> visibleTargets = new List<Transform>();
    public bool CanSeeEnemy { get; private set; }

    private float playerBaseAngle = 135f;
    // Update is called once per frame (no need for a Coroutine for this check frequency)
    void Update()
    {
        FindVisibleTargets();
        CanSeeEnemy = visibleTargets.Count > 0;
    }

    void FindVisibleTargets()
    {
        visibleTargets.Clear();
        Collider2D[] rangeCheck = Physics2D.OverlapCircleAll(transform.position, radius, targetEnemy);

        foreach (Collider2D targetCollider in rangeCheck)
        {
            Transform target = targetCollider.transform;
            Vector2 directionToTarget = (target.position - transform.position).normalized;

            Vector2 playerForwardDirection = DirectionFromAngle(playerBaseAngle);

            // Use the object's forward direction (assuming it's correctly oriented)
            if (Vector2.Angle(playerForwardDirection, directionToTarget) < angle / 2)
            {
                float distanceToTarget = Vector2.Distance(transform.position, target.position);

                if (!Physics2D.Raycast(transform.position, directionToTarget, distanceToTarget, obstructionLayer))
                {
                    visibleTargets.Add(target);
                }
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        UnityEditor.Handles.DrawWireDisc(transform.position, Vector3.forward, radius);

        // Calculate the view angles based on the player's rotation
        Vector3 viewAngleA = DirectionFromAngle(playerBaseAngle - angle / 2);
        Vector3 viewAngleB = DirectionFromAngle(playerBaseAngle + angle / 2);

        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position, transform.position + (Vector3)viewAngleA * radius);
        Gizmos.DrawLine(transform.position, transform.position + (Vector3)viewAngleB * radius);

        // Draw lines to all visible targets
        Gizmos.color = Color.green;
        foreach (Transform target in visibleTargets)
        {
            Gizmos.DrawLine(transform.position, target.position);
        }
    }

    private Vector3 DirectionFromAngle(float angleInDegrees)
    {
        angleInDegrees += transform.eulerAngles.z; // Adjust for the object's current rotation
        return new Vector3(Mathf.Cos(angleInDegrees * Mathf.Deg2Rad), Mathf.Sin(angleInDegrees * Mathf.Deg2Rad));
    }
}