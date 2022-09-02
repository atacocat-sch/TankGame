using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Healthbar : MonoBehaviour
{
    public Health target;
    public Image fill;
    public Image smoothedFill;
    public TMPro.TMP_Text healthText;
    public float smoothTime;

    float velocity;

    private void LateUpdate()
    {
        float healthPercent = target.CurrentHeath / target.MaxHealth;

        fill.fillAmount = healthPercent;
        smoothedFill.fillAmount = Mathf.SmoothDamp(smoothedFill.fillAmount, healthPercent, ref velocity, smoothTime);

        healthText.text = $"{Mathf.RoundToInt(target.CurrentHeath)}/{Mathf.RoundToInt(target.MaxHealth)}";
    }
}
