using UnityEngine;

public static class Extensions
{
    public static Vector2 ClosestPoint (this Rect rect, Vector2 point)
    {
        return new Vector2(Mathf.Clamp(point.x, rect.xMin, rect.xMax), Mathf.Clamp(point.y, rect.yMin, rect.yMax));
    }
}
