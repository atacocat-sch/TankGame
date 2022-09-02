using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OffscreenIndicator : MonoBehaviour
{
    public float cameraRectShrinkage;
    public float scaleSmoothTime;

    float scalePercent;
    float scaleVelocity;

    private void LateUpdate()
    {
        Camera camera = Camera.main;

        Rect rect = new Rect
        {
            width = camera.orthographicSize * Screen.width / Screen.height * 2.0f - cameraRectShrinkage,
            height = camera.orthographicSize * 2.0f - cameraRectShrinkage,
            center = camera.transform.position,
        };

        transform.position = rect.ClosestPoint(transform.parent.position);

        bool onScreen = rect.Contains(transform.parent.position);
        scalePercent = Mathf.SmoothDamp(scalePercent, onScreen ? 0.0f : 1.0f, ref scaleVelocity, scaleSmoothTime);

        transform.localScale = Vector3.one * scalePercent;
        transform.rotation = Quaternion.identity;
    }
}
