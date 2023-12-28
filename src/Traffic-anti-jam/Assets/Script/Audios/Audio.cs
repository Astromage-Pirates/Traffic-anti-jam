using UnityEngine;
using UnityEngine.Audio;
using static SoundGroup;

public class Audio : MonoBehaviour
{
    [SerializeField]
    private AudioMixer audioMixer;

    private void Start()
    {
        Ambient.SetVolume(audioMixer);
        Master.SetVolume(audioMixer);
        Background.SetVolume(audioMixer);
        Sfx.SetVolume(audioMixer);
    }
}
