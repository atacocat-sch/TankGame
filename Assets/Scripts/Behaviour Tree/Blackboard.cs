using System.Collections.Generic;

public class Blackboard
{
    public Dictionary<string, object> values;

    public bool TryGet<T>(string key, out T value)
    {
        value = default;

        if (!values.ContainsKey(key)) return false;
        if (!(values[key] is T)) return false;

        value = (T)values[key];
        return true;
    }

    public void Set (string key, object value)
    {
        if (values.ContainsKey(key))
        {
            values.Add(key, value);
        }
        else values[key] = value;
    }
}
