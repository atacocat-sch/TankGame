using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine.SceneManagement;

[System.Serializable]
public class Stats
{
    int tanksDestroyed;
    float timeAlive;
    int shotsFired;
    float damageDelt;

    public event System.Action ValueChangedEvent;

    public static Stats _main;
    public static Stats Main
    {
        get
        {
            if (_main == null)
            {
                _main = new Stats();
            }
            return _main;
        }
    }

    public int TanksDestroyed
    {
        get => tanksDestroyed;
        set
        {
            tanksDestroyed = value;
            ValueChangedEvent?.Invoke();
        }
    }

    public float TimeAlive
    {
        get => timeAlive;
        set
        {
            timeAlive = value;
            ValueChangedEvent?.Invoke();
        }
    }

    public int ShotsFired
    {
        get => shotsFired;
        set
        {
            shotsFired = value;
            ValueChangedEvent?.Invoke();
        }
    }

    public float DamageDelt
    {
        get => damageDelt;
        set
        {
            damageDelt = value;
            ValueChangedEvent?.Invoke();
        }
    }

    public Stats ()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode loadMode)
    {
        if (loadMode == LoadSceneMode.Single)
        {
            Clear();
        }
    }

    private void Clear()
    {
        tanksDestroyed = 0;
        timeAlive = 0.0f;
        shotsFired = 0;
    }
}
