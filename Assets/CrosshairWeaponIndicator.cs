using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrosshairWeaponIndicator : MonoBehaviour
{
    [SerializeField] GameObject target;
    [SerializeField] GameObject rendererObject;

    private void Update()
    {
        if (target.TryGetComponent(out IAttack attack))
        {
            rendererObject.SetActive(attack.Cooldown > 0.999f);
        }
    }
}
