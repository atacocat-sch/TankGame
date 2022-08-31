using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TankCursor : MonoBehaviour
{
    [SerializeField] SpriteRenderer cursor;
    [SerializeField] SpriteRenderer loadingCursor;

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
            cursor.gameObject.SetActive(true);
            loadingCursor.gameObject.SetActive(false);
        }
        else
        {
            cursor.gameObject.SetActive(false);
            loadingCursor.gameObject.SetActive(true);

            loadingCursor.sharedMaterial.SetFloat("_Value", 1.0f - (gun.NextFireTime - Time.time) / gun.FireDelay);
        }
    }

    private void OnDisable()
    {
        Cursor.visible = true;
    }
}
