using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatDisplayTime : StatDisplay
{
    protected override string Format(float value)
    {
        string timeFormated = string.Format("{0:N0}:{1:00}", value / 60.0f, value % 60.0f);
        return string.Format(textTemplate, timeFormated);
    }
}
