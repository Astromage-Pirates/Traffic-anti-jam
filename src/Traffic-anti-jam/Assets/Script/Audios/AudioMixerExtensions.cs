using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using static SoundGroup;

/// <summary>
/// Enumeration of the mixer sound groups.
/// </summary>
public enum SoundGroup
{
    Master,
    Background,
    Ambient,
    Sfx,
}

/// <summary>
/// Extension class that add additional useful stuffs for <see cref="AudioMixer"/>.
/// </summary>
public static class AudioMixerExtensions
{
    private const float AmplitudeFactor = 20f;

    /// <summary>
    /// Helper property to get exposed parameter name based on <see cref="SoundGroup"/>
    /// </summary>
    public readonly static Dictionary<SoundGroup, string> MixerExposedParams = new Dictionary<
        SoundGroup,
        string
    >
    {
        { Background, "BackgroundVolume" },
        { Sfx, "SfxVolume" },
        { Ambient, "AmbientVolume" },
        { Master, "MasterVolume" },
    };

    public static float ConvertToMixerValue(this float value)
    {
        return Mathf.Log10(value) * AmplitudeFactor;
    }

    /// <summary>
    /// Set the volume of the <see cref="SoundGroup"/>.
    /// </summary>
    /// <param name="group">The <see cref="SoundGroup"/> to be set.</param>
    /// <param name="audioMixer">The <see cref="AudioMixer"/> that contains the provided <see cref="SoundGroup"/>.</param>
    public static void SetVolume(this SoundGroup group, AudioMixer audioMixer)
    {
        var key = MixerExposedParams[group];
        var volume = PlayerPrefs.GetFloat(key, 1f);

        audioMixer.SetFloat(key, volume.ConvertToMixerValue());
    }
}
