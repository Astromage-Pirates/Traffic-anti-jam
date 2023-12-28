using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Helper class to prevent <see cref="Object"/> from being destroy when loading new <see cref="Scene"/>.
/// </summary>
public class DontDestroyOnLoad : MonoBehaviour
{
    private void Awake()
    {
        DontDestroyOnLoad(this);
    }
}
