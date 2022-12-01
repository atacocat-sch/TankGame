using UnityEngine;
using TMPro;
using System;

public class ScoreCounter : MonoBehaviour
{
    [TextArea]
    [SerializeField] string template;

    TMP_Text display;

    private void Awake()
    {
        display = GetComponent<TMP_Text>();
    }

    private void Update()
    {
        var timespan = TimeSpan.FromSeconds(Stats.Main.TimeAlive);
        display.text = string.Format(template, Stats.Main.TanksDestroyed, timespan.ToString("m\\:ss"));
    }
}
