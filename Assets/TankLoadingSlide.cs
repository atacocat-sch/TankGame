using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TankLoadingSlide : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    [SerializeField] float smoothTime;
    [SerializeField] GameObject lockedImage;
    [SerializeField] GameObject unlockedImage;

    bool locked;
    bool dragging;
    bool landingState;
    Vector2 velocity;

    private void Start()
    {
        landingState = false;

        RectTransform parent = transform.parent as RectTransform;
        transform.localPosition = Vector2.up * parent.rect.yMax;

        Lock();
    }

    private void LateUpdate()
    {
        if (!dragging)
        {
            RectTransform transform = this.transform as RectTransform;
            RectTransform parent = transform.parent as RectTransform;

            Vector2 start = Vector2.up * parent.rect.yMin;
            Vector2 end = Vector2.up * parent.rect.yMax;
            transform.localPosition = Vector2.SmoothDamp(transform.localPosition, landingState ? start : end, ref velocity, smoothTime);
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (locked) return;

        RectTransform parent = transform.parent as RectTransform;
        transform.localPosition += (Vector3)eventData.delta;
        transform.localPosition = parent.rect.ClosestPoint(transform.localPosition);

        dragging = true;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (locked) return;

        RectTransform transform = this.transform as RectTransform;

        landingState = transform.anchoredPosition.y < 0.0f;
        dragging = false;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        
    }

    public void Lock ()
    {
        RectTransform parent = transform.parent as RectTransform;
        Vector2 end = Vector2.up * parent.rect.yMax;

        if (landingState) return;
        if (Mathf.Abs(transform.localPosition.y - end.y) > 1.0f) return;

        locked = true;

        lockedImage.SetActive(true);
        unlockedImage.SetActive(false);

        GetComponentInParent<TankLoading>().Locked = true;
    }

    public void Unlock ()
    {
        locked = false;

        lockedImage.SetActive(false);
        unlockedImage.SetActive(true);

        GetComponentInParent<TankLoading>().Locked = false;
    }
}
