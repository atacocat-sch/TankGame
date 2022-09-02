using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackCooldownUI : MonoBehaviour
{
    [SerializeField] GameObject target;
    [SerializeField] RectTransform sliderFill;

    private void Update()
    {
        if (target.TryGetComponent(out IAttack attack))
        {
            Rect containingRect = ((RectTransform)sliderFill.parent).rect;
            sliderFill.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, containingRect.width * attack.Cooldown);
        }
    }
}
