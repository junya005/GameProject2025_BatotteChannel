using UnityEngine;

/// <summary>継承するとそのシーンでのみ使用できるシングルトンになる</summary>
public class SceneSingleton<T> : MonoBehaviour where T : Component
{
    public static T Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this as T;
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
