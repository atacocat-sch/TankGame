using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Healthbar : MonoBehaviour
{
    [SerializeField] Health target;
    [SerializeField] Image fill;
    [SerializeField] Image smoothedFill;
    [SerializeField] TMPro.TMP_Text healthText;
    [SerializeField] float smoothTime;

    float velocity;

    private void LateUpdate()
    {
        float healthPercent = target.CurrentHeath / target.MaxHealth;

        fill.fillAmount = healthPercent;
        smoothedFill.fillAmount = Mathf.SmoothDamp(smoothedFill.fillAmount, healthPercent, ref velocity, smoothTime);

        healthText.text = $"{Mathf.RoundToInt(target.CurrentHeath)}/{Mathf.RoundToInt(target.MaxHealth)}";
    }
}
