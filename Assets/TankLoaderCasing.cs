using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TankLoaderCasing : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [SerializeField] Transform homePoint;
    [SerializeField] Transform detachParent;
    [SerializeField] int detachParentIndex;
    [SerializeField] float gravity;
    [SerializeField] float drag;
    [SerializeField] float inheritVelocity;

    bool freefall;
    Vector2 velocity;

    private void Update()
    {
        if (freefall)
        {
            transform.position += (Vector3)velocity * Time.deltaTime;
            velocity += Vector2.up * gravity;
            velocity -= velocity * drag * Time.deltaTime;
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        transform.SetParent(detachParent);
        transform.SetSiblingIndex(detachParentIndex);
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position += (Vector3)eventData.delta;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        velocity = eventData.delta * inheritVelocity / Time.deltaTime;
        freefall = true;

        GetComponentInParent<TankLoading>().CurrentChamberContents = TankLoading.ChamberContents.Empty;
    }

    [ContextMenu("Reset Position")]
    public void ResetCasing ()
    {
        RectTransform transform = this.transform as RectTransform;
        transform.SetParent(homePoint);
        transform.anchoredPosition = Vector2.zero;

        freefall = false;
    }
}

