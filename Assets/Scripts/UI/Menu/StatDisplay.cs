using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;

public class StatDisplay : MonoBehaviour
{
    public TMPro.TMP_Text textElement;
    public string key;
    public string textTemplate;

    private void OnEnable()
    {
        Stats.Main.ValueChangedEvent += OnValueChangedEvent;

        OnValueChangedEvent();
    }

    private void OnDisable()
    {
        Stats.Main.ValueChangedEvent -= OnValueChangedEvent;
    }

    private void OnValueChangedEvent()
    {
        object val = typeof(Stats).GetField(key, BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.IgnoreCase).GetValue(Stats.Main);

        textElement.text = Format(val);
    }

    protected virtual string Format (object value)
    {
        return string.Format(textTemplate, value.ToString());
    }
}
