using NUnit.Framework;
using UnityEngine;
using UnityEngine.Audio;

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
/// Extension class that add additional useful stuffs for <see cref="AudioMixer"/>.
/// </summary>
public static class SoundGroupExtensions
{
    private const float MinSliderValue = 0.0001f;
    private const float MaxSliderValue = 1f;
    private const float AmplitudeFactor = 20f;

    public static float ConvertToMixerValue(this float value)
    {
        Assert.IsTrue(
            value >= MinSliderValue & value <= MaxSliderValue,
            $"Value must be in range ({MinSliderValue}, {MaxSliderValue})"
        );

        return Mathf.Log10(value) * AmplitudeFactor;
    }

    /// <summary>
    /// Set the volume of the <see cref="SoundGroup"/>.
    /// </summary>
    /// <param name="group">The <see cref="SoundGroup"/> to be set.</param>
    /// <param name="audioMixer">The <see cref="AudioMixer"/> that contains the provided <see cref="SoundGroup"/>.</param>
    public static void SetVolume(this SoundGroup group, AudioMixer audioMixer)
    {
        var key = group.ToString();
        var volume = PlayerPrefs.GetFloat(key, 1f);

        audioMixer.SetFloat(key, volume.ConvertToMixerValue());
    }
}
