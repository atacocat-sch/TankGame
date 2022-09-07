using System.Collections.Generic;
using UnityEngine.SceneManagement;

public static class Stats
{
    static bool initalized = false;

    static Dictionary<string, float> values;

    public static event System.Action<string, float> ValueChangedEvent;

    private static void Initalize()
    {
        values = new Dictionary<string, float>();

        SceneManager.sceneLoaded += OnSceneLoaded;

        initalized = true;
    }

    private static void OnSceneLoaded(Scene scene, LoadSceneMode loadMode)
    {
        if (loadMode == LoadSceneMode.Single)
        {
            values.Clear();
        }
    }

    public static void SetValue (string key, float val)
    {
        if (!initalized) Initalize();

        if (values.ContainsKey(key))
        {
            values[key] = val;
        }
        else
        {
            values.Add(key, val);
        }

        ValueChangedEvent?.Invoke(key, val);
    }

    public static void IncrementValue(string key, float delta)
    {
        if (!initalized) Initalize();

        if (values.ContainsKey(key))
        {
            values[key] += delta;
        }
        else
        {
            values.Add(key, delta);
        }

        ValueChangedEvent?.Invoke(key, delta);
    }

    public static float GetValue (string key, float exept = 0.0f)
    {
        if (!initalized) Initalize();

        if (values.ContainsKey(key))
        {
            return values[key];
        }
        else
        {
            return exept;
        }
    }
    public static bool TryGetValue (string key, out float val)
    {
        if (!initalized) Initalize();

        if (values.ContainsKey(key))
        {
            val = values[key];
            return true;
        }
        else
        {
            val = -1.0f;
            return false;
        }
    }
}
