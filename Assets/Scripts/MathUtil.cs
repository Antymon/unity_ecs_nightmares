using UnityEngine;

public class MathUtil
{
    public static float GetDirectedAngleBetweenVectors(Vector2 v1, Vector2 v2)
    {
        return GetDirectedAngleBetweenVectors((Vector3)v1, (Vector3)v2);
    }

    public static float GetDirectedAngleBetweenVectors(Vector3 v1, Vector3 v2)
    {
        float angle = Mathf.Atan2(v1.x, v1.y) - Mathf.Atan2(v2.x, v2.y);
        angle *= Mathf.Rad2Deg;
        angle = NormilizeDegreesAngle(angle);

        return angle;
    }

    /* -180, 180 */
    public static float NormilizeDegreesAngle(float angle)
    {
        angle %= 360f;
        angle = angle < 0f ? angle + 360f : angle;
        angle = angle > 180f ? angle - 360f : angle;
        return angle;
    }
}