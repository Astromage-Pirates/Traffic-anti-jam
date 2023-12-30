using System.Collections;
using System.Collections.Generic;
using AstroPirate.DesignPatterns;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class SoundSystem : MonoBehaviour
{
    [SerializeField]
    private AudioSource audioSource;

    private IEventBus eventBus;

    private void Awake()
    {
        GlobalServiceContainer.Resolve(out eventBus);
        eventBus.Register<AudioPlayed>(ONAudioPlayed);
    }

    private void ONAudioPlayed(AudioPlayed audioPlayed)
    {
        audioSource.PlayOneShot(
            audioPlayed.audioClip,
            PlayerPrefs.GetFloat(SoundGroup.SfxVolume.ToString())
        );
    }

    private void OnDestroy()
    {
        eventBus.UnRegister<AudioPlayed>(ONAudioPlayed);
        eventBus.Register<AudioPlayed>(ONAudioPlayed);
    }
}
