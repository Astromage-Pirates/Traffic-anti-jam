using AstroPirate.DesignPatterns;
using UnityEngine;

/// <summary>
/// A components used to control audio.
/// </summary>
public class Audio : MonoBehaviour
{
    [SerializeField]
    private SoundGroup soundGroup;

    [SerializeField]
    private AudioSource audioSource;

    private IEventBus eventBus;

    private void Awake()
    {
        GlobalServiceContainer.Resolve(out eventBus);
        eventBus.Register<AudioVolumeChanged>(OnAudioVolumeChanged);
    }

    private void OnDestroy()
    {
        eventBus.UnRegister<AudioVolumeChanged>(OnAudioVolumeChanged);
    }

    private void Start()
    {
        soundGroup.SetVolume(audioSource);
    }

    private void OnAudioVolumeChanged(AudioVolumeChanged audioVolume)
    {
        soundGroup.SetVolume(audioSource);
    }
}
