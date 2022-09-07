using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class TouchscreenStick : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public Vector2 parentPosition;
    public float startSoftZone;

    [Space]
    public UnityEvent<Vector2> ValueChangeEvent;
    public UnityEvent StickDownEvent;
    public UnityEvent StickUpEvent;

    Vector2 direction;

    new public RectTransform transform => base.transform as RectTransform;
    public RectTransform parent => transform.parent as RectTransform;

    private void Awake()
    {
        parentPosition = parent.anchoredPosition;
    }

    private void OnEnable()
    {
        parent.gameObject.SetActive(Application.platform == RuntimePlatform.Android);
    }

    private void Update()
    {
        ValueChangeEvent?.Invoke(direction);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if ((eventData.position - (Vector2)parent.position).sqrMagnitude < startSoftZone * startSoftZone)
        {
            parent.position = eventData.position;
        }
        else
        {
            transform.position = eventData.position;
        }

        StickDownEvent?.Invoke();
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = eventData.position;
        transform.anchoredPosition = Vector2.ClampMagnitude(transform.anchoredPosition, Mathf.Max(parent.rect.width, parent.rect.height) / 2.0f);

        direction = transform.anchoredPosition / parent.rect.size * 2.0f;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        parent.anchoredPosition = parentPosition;
        transform.anchoredPosition = Vector2.zero;

        direction = Vector2.zero;

        StickUpEvent?.Invoke();
    }
}
