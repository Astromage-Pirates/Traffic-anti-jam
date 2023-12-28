using AstroPirate.DesignPatterns;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

/// <summary>
/// A <see cref="Slider"/> to control volume values.
/// </summary>
public class VolumeSlider : MonoBehaviour
{
    [SerializeField]
    private SoundGroup soundGroup;

    [SerializeField]
    private Slider slider;

    private IEventBus eventBus;

    private void Awake()
    {
        GlobalServiceContainer.Resolve(out eventBus);
    }

    private void Start()
    {
        var volume = PlayerPrefs.GetFloat(soundGroup.ToString(), 1f);
        SetVolume(volume);
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
        eventBus.Send(new AudioVolumeChanged());
    }

    private void SetVolume(float value)
    {
        var key = soundGroup.ToString();

        PlayerPrefs.SetFloat(key, value);
        slider.value = value;
    }
}
