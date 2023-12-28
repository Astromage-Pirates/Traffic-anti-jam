using UnityEngine;
using UnityEngine.Audio;
using static SoundGroup;

public class Audio : MonoBehaviour
{
    [SerializeField]
    private AudioMixer audioMixer;

    private void Start()
    {
        AmbientVolume.SetVolume(audioMixer);
        MasterVolume.SetVolume(audioMixer);
        BackgroundVolume.SetVolume(audioMixer);
        SfxVolume.SetVolume(audioMixer);
    }
}
