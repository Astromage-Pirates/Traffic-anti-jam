using UnityEngine;

/// <summary>
/// Enumeration of the mixer sound groups.
/// </summary>
public enum SoundGroup
{
    MasterVolume,
    BackgroundVolume,
    AmbientVolume,
    SfxVolume,
}

/// <summary>
/// Extension class that add additional useful stuffs for <see cref="SoundGroup"/>.
/// </summary>
public static class SoundGroupExtensions
{
    /// <summary>
    /// Set the volume of the <see cref="SoundGroup"/>. This should get and set value from <see cref="PlayerPrefs"/> (if any) or default to max volume value.
    /// </summary>
    /// <param name="group">The <see cref="SoundGroup"/> to be set.</param>
    /// <param name="audioSource">The <see cref="AudioSource"/> that contains the provided <see cref="SoundGroup"/>.</param>
    public static void SetVolume(this SoundGroup group, AudioSource audioSource)
    {
        var key = group.ToString();
        var volume = PlayerPrefs.GetFloat(group.ToString(), 1f);

        audioSource.volume = volume;
    }
}
