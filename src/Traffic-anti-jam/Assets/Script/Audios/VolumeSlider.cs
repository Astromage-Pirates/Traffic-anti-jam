using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

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
        volumePercentage = mixerGroup.SetVolume(audioMixer);
        slider.value = volumePercentage;
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
        var key = mixerGroup.ToString();

        PlayerPrefs.SetFloat(key, value);
        audioMixer.SetFloat(key, value.ConvertToMixerValue());
    }
}
