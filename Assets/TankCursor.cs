using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TankCursor : MonoBehaviour
{
    [SerializeField] SpriteRenderer m_Cursor;
    [SerializeField] SpriteRenderer m_LoadingCursor;

    TankGun gun;

    private void Awake()
    {
        gun = GetComponentInParent<TankGun>();
    }

    private void OnEnable()
    {
        Cursor.visible = false;
    }

    private void LateUpdate()
    {
        transform.position = (Vector2)Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        transform.rotation = Quaternion.identity;

        if (Time.time > gun.NextFireTime)
        {
            m_Cursor.gameObject.SetActive(true);
            m_LoadingCursor.gameObject.SetActive(false);
        }
        else
        {
            m_Cursor.gameObject.SetActive(false);
            m_LoadingCursor.gameObject.SetActive(true);

            m_LoadingCursor.sharedMaterial.SetFloat("_Value", 1.0f - (gun.NextFireTime - Time.time) / gun.FireDelay);
        }
    }

    private void OnDisable()
    {
        Cursor.visible = true;
    }
}
