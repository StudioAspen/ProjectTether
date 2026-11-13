using System.Collections.Generic;
using UnityEngine;

public static class CapsuleDetection
{
    public static bool TryGetObjectInCapsule<TObject>(
        Vector3 origin, 
        Vector3 direction, 
        float distance, 
        float radius, 
        out (TObject obj, Vector3 hitPoint)? outputObject)
    {
        List<(TObject powerSource, Vector3 hitPoint)> validHits = new();

        RaycastHit[] hits = Physics.SphereCastAll(
            origin + direction * radius, 
            radius, 
            direction, 
            distance
        );
        foreach (RaycastHit hit in hits)
        {
            TObject obj = hit.collider.GetComponentInParent<TObject>();
            if (obj == null)
                continue;

            validHits.Add((obj, hit.collider.ClosestPoint(origin)));
        }

        if (validHits.Count == 0)
        {
            outputObject = null;
            return false;
        }
            
        // Get closest hitpoint
        float closestSqrMagnitude = float.MaxValue;
        (TObject obj, Vector3 hitPoint)? closestHit = null;
        foreach (var hit in validHits)
        {
            float sqrMagnitude = (hit.hitPoint - origin).sqrMagnitude;
            if (sqrMagnitude < closestSqrMagnitude)
            {
                closestSqrMagnitude = sqrMagnitude;
                closestHit = hit;
            }
        }
            
        outputObject = closestHit;
        return true;
    }
    
    public static void DrawGizmos(Vector3 origin, Vector3 direction, float distance, float radius, Color color, float railCount = 8)
    {
        direction.Normalize();
        origin += direction * radius;
        Vector3 end = origin + direction * distance;

        Gizmos.color = color;
        Gizmos.DrawWireSphere(origin, radius);
        Gizmos.DrawWireSphere(end, radius);
        Gizmos.DrawLine(origin, end);

        // Draw rails around the sphere's path.
        Vector3 right = Vector3.Cross(direction, Vector3.up);
        if (right.sqrMagnitude < 0.001f)
            right = Vector3.Cross(direction, Vector3.right);

        right.Normalize();
        Vector3 up = Vector3.Cross(right, direction).normalized;
        
        for (int i = 0; i < railCount; i++)
        {
            float angle = i * Mathf.PI * 2f / railCount;
            Vector3 offset = (right * Mathf.Cos(angle) + up * Mathf.Sin(angle)) * radius;

            Gizmos.DrawLine(origin + offset, end + offset);
        }
    }
}