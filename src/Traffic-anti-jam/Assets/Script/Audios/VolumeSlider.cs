using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using static AudioMixerExtensions;

/// <summary>
/// A <see cref="Slider"/> to control volume values.
/// </summary>
public class VolumeSlider : MonoBehaviour
{
    [SerializeField]
    private SoundGroup mixerGroup;

    [SerializeField]
    private AudioMixer audioMixer;

    [SerializeField]
    private Slider slider;

    private float volumePercentage;

    private void Start()
    {
        volumePercentage = PlayerPrefs.GetFloat(MixerExposedParams[mixerGroup], 1f);

        SetVolume(volumePercentage);
    }

    private void OnEnable()
    {
        slider.onValueChanged.AddListener(OnSliderValueChanged);
    }

    private void OnDisable()
    {
        slider.onValueChanged.RemoveListener(OnSliderValueChanged);
    }

    private void OnSliderValueChanged(float value)
    {
        SetVolume(value);
    }

    private void SetVolume(float value)
    {
        var key = MixerExposedParams[mixerGroup];

        PlayerPrefs.SetFloat(key, value);
        slider.value = value;
        audioMixer.SetFloat(key, value.ConvertToMixerValue());
    }
}
