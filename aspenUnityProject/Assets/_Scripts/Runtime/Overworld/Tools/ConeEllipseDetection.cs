using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Finds objects inside a horizontal, ellipse-shaped cone and draws the matching gizmo.
/// </summary>
public static class ConeEllipseDetection
{
    private const float MinimumDirectionSqrMagnitude = 0.0001f;
    private const int GizmoSegments = 32;

    /// <summary>
    /// Finds distinct GameObjects whose transform positions fall inside the ellipse sector.
    /// </summary>
    /// <param name="origin">World-space center of the query.</param>
    /// <param name="direction">Forward direction; its vertical component is ignored.</param>
    /// <param name="forwardRadius">Ellipse radius along the forward direction.</param>
    /// <param name="lateralRadius">Ellipse radius perpendicular to the forward direction.</param>
    /// <param name="halfAngle">Maximum angle from forward, in degrees.</param>
    /// <param name="layers">Layers to include in the broadphase physics query.</param>
    /// <param name="ignoreHierarchyRoot">Optional transform whose descendants are excluded.</param>
    public static List<GameObject> FindObjects(
        Vector3 origin,
        Vector3 direction,
        float forwardRadius,
        float lateralRadius,
        float halfAngle,
        LayerMask layers,
        Transform ignoreHierarchyRoot = null)
    {
        var results = new List<GameObject>();
        Vector3 forward = Vector3.ProjectOnPlane(direction, Vector3.up);
        if (forwardRadius <= 0f || lateralRadius <= 0f || forward.sqrMagnitude < MinimumDirectionSqrMagnitude)
            return results;

        forward.Normalize();
        Vector3 right = Vector3.Cross(Vector3.up, forward).normalized;
        float broadphaseRadius = Mathf.Max(forwardRadius, lateralRadius);
        Collider[] hits = Physics.OverlapSphere(origin, broadphaseRadius, layers);
        if (hits == null || hits.Length == 0)
            return results;

        halfAngle = Mathf.Clamp(halfAngle, 0f, 180f);
        var seenObjects = new HashSet<GameObject>();
        foreach (Collider hit in hits)
        {
            if (hit == null || (ignoreHierarchyRoot != null && hit.transform.IsChildOf(ignoreHierarchyRoot)))
                continue;

            GameObject hitObject = hit.gameObject;
            if (!seenObjects.Add(hitObject))
                continue;

            Vector3 offset = Vector3.ProjectOnPlane(hitObject.transform.position - origin, Vector3.up);
            float forwardDistance = Vector3.Dot(offset, forward);
            float lateralDistance = Vector3.Dot(offset, right);
            float normalizedEllipseDistance =
                (forwardDistance * forwardDistance) / (forwardRadius * forwardRadius) +
                (lateralDistance * lateralDistance) / (lateralRadius * lateralRadius);

            if (normalizedEllipseDistance > 1f)
                continue;

            float angle = Mathf.Atan2(Mathf.Abs(lateralDistance), forwardDistance) * Mathf.Rad2Deg;
            if (angle <= halfAngle)
                results.Add(hitObject);
        }

        return results;
    }

    /// <summary>
    /// Draws the wireframe boundary of the same horizontal ellipse sector used by
    /// <see cref="FindObjects"/>. Call from an OnDrawGizmos callback.
    /// </summary>
    public static void DrawGizmos(
        Vector3 origin,
        Vector3 direction,
        float forwardRadius,
        float lateralRadius,
        float halfAngle)
    {
        Vector3 forward = Vector3.ProjectOnPlane(direction, Vector3.up);
        if (forwardRadius <= 0f || lateralRadius <= 0f || forward.sqrMagnitude < MinimumDirectionSqrMagnitude)
            return;

        forward.Normalize();
        Vector3 right = Vector3.Cross(Vector3.up, forward).normalized;
        halfAngle = Mathf.Clamp(halfAngle, 0f, 180f);

        Vector3 PointOnEllipse(float angle)
        {
            float radians = angle * Mathf.Deg2Rad;
            float forwardComponent = Mathf.Cos(radians);
            float lateralComponent = Mathf.Sin(radians);
            float radius = 1f / Mathf.Sqrt(
                (forwardComponent * forwardComponent) / (forwardRadius * forwardRadius) +
                (lateralComponent * lateralComponent) / (lateralRadius * lateralRadius));

            return origin + (forward * forwardComponent + right * lateralComponent) * radius;
        }

        Vector3 firstPoint = PointOnEllipse(-halfAngle);
        Vector3 previousPoint = firstPoint;
        for (int i = 1; i <= GizmoSegments; i++)
        {
            float angle = Mathf.Lerp(-halfAngle, halfAngle, i / (float)GizmoSegments);
            Vector3 nextPoint = PointOnEllipse(angle);
            Gizmos.DrawLine(previousPoint, nextPoint);
            previousPoint = nextPoint;
        }

        Gizmos.DrawLine(origin, firstPoint);
        Gizmos.DrawLine(origin, previousPoint);
    }
}
