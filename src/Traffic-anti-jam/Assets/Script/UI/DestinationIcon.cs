using UnityEngine;

/// <summary>
/// Destination icon to display on <see cref="Path"/>.
/// </summary>
public class DestinationIcon : MonoBehaviour
{
    /// <summary>
    /// The <see cref="UnityEngine.Renderer"/> of this <see cref="GameObject"/>.
    /// </summary>
    [field: SerializeField]
    public Renderer Renderer { get; private set; }
}
