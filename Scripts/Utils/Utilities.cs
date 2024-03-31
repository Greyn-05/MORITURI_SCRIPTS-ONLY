using UnityEngine;

public class Utilities
{
    public static T GetOrAddComponent<T>(GameObject go) where T : Component
    {
        return go.GetComponent<T>() ?? go.AddComponent<T>();
    }
}