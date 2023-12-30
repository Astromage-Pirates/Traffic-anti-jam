using AstroPirate.DesignPatterns;

/// <summary>
/// Event for when the <see cref="Audio"/> volume changed.
/// </summary>
public class AudioVolumeChanged : EventContext
{
    public SoundGroup SoundGroup;
}
