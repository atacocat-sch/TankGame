using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatDisplay : MonoBehaviour
{
    public TMPro.TMP_Text textElement;
    public string key;
    public string textTemplate;

    private void OnEnable()
    {
        Stats.ValueChangedEvent += OnValueChangedEvent;

        OnValueChangedEvent(key, Stats.GetValue(key));
    }

    private void OnDisable()
    {
        Stats.ValueChangedEvent -= OnValueChangedEvent;
    }

    private void OnValueChangedEvent(string key, float val)
    {
        if (this.key.ToLower() != key.ToLower()) return;

        textElement.text = Format(val);
    }

    protected virtual string Format (float value)
    {
        return string.Format(textTemplate, value.ToString());
    }
}
