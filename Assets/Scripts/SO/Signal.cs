using UnityEngine;

[CreateAssetMenu(fileName = "Scriptable Objects/Signal")]
public class Signal : ScriptableObject
{
    public event System.Action OnRaise;

    public void Raise ()
    {
        OnRaise?.Invoke();
    }
}
