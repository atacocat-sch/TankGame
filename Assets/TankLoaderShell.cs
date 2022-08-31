using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TankLoaderShell : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [SerializeField] Transform magazinePoint;
    [SerializeField] Transform loaderPoint;
    [SerializeField] Transform homeParent;
    [SerializeField] float smoothTime;

    [Space]
    [SerializeField] CanvasGroup canvasGroup;
    [SerializeField] Vector2 resetOffset;
    [SerializeField] float resetTime;
    [SerializeField] AnimationCurve resetInterpolation;

    Vector2 velocity;
    bool landingState;
    bool dragging;
    bool resetting;

    private void Update()
    {
        if (resetting) return;

        if (!dragging)
        {
            transform.position = Vector2.SmoothDamp(transform.position, (landingState ? loaderPoint : magazinePoint).position, ref velocity, smoothTime);
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (resetting) return;

        homeParent.SetParent(homeParent);
        dragging = true;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (resetting) return;

        transform.localPosition += (Vector3)eventData.delta;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (resetting) return;

        TankLoading controller = GetComponentInParent<TankLoading>();
        if (controller.CurrentChamberContents == TankLoading.ChamberContents.Empty)
        {
            if ((loaderPoint.position - transform.position).sqrMagnitude < (magazinePoint.position - transform.position).sqrMagnitude)
            {
                transform.SetParent(loaderPoint);
                controller.CurrentChamberContents = TankLoading.ChamberContents.Shell;
                landingState = true;
            }
            else landingState = false;
        }
        else
        {
            if (controller.CurrentChamberContents == TankLoading.ChamberContents.Shell)
            {
                controller.CurrentChamberContents = TankLoading.ChamberContents.Empty;
            }
            landingState = false;
        }

        dragging = false;
    }

    public void ResetShell ()
    {
        StartCoroutine(FadeInRoutine());
    }

    private IEnumerator FadeInRoutine()
    {
        resetting = true;
        landingState = false;

        transform.SetParent(homeParent);
        transform.position = (Vector2)magazinePoint.position + resetOffset;

        float percent = 0.0f;
        while (percent < 1.0f)
        {
            canvasGroup.alpha = resetInterpolation.Evaluate(percent);
            transform.position = (Vector2)magazinePoint.position + resetOffset * (1.0f - resetInterpolation.Evaluate(percent));

            percent += Time.deltaTime / resetTime;
            yield return null;
        }

        resetting = false;
    }
}
