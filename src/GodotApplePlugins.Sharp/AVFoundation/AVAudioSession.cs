using Godot;

namespace GodotApplePlugins.Sharp.AVFoundation;

/// <summary>
/// Audio session category options (bitmask flags).
/// </summary>
[Flags]
public enum AVAudioSessionCategoryOptions
{
    /// <summary>No options.</summary>
    None = 0,
    /// <summary>Mix audio with other applications.</summary>
    MixWithOthers = 1,
    /// <summary>Reduce other apps' audio volume.</summary>
    DuckOthers = 2,
    /// <summary>Enable Bluetooth output.</summary>
    AllowBluetooth = 4,
    /// <summary>Route audio to speaker by default.</summary>
    DefaultToSpeaker = 8,
    /// <summary>Interrupt spoken audio and mix with others.</summary>
    InterruptSpokenAudioAndMixWithOthers = 17,
    /// <summary>Allow Bluetooth A2DP.</summary>
    AllowBluetoothA2DP = 32,
    /// <summary>Allow AirPlay.</summary>
    AllowAirPlay = 64,
    /// <summary>Override muted microphone interruption.</summary>
    OverrideMutedMicrophoneInterruption = 128
}

/// <summary>
/// Route sharing policy for audio session.
/// </summary>
public enum AVAudioSessionRouteSharingPolicy
{
    /// <summary>Default route sharing.</summary>
    Default = 0,
    /// <summary>Long form audio route sharing.</summary>
    LongFormAudio = 1,
    /// <summary>Independent route sharing.</summary>
    Independent = 2,
    /// <summary>Long form route sharing.</summary>
    LongForm = 3
}

/// <summary>
/// Audio session category.
/// </summary>
public enum AVAudioSessionCategory
{
    /// <summary>Ambient audio category.</summary>
    Ambient = 0,
    /// <summary>Multi-route audio category.</summary>
    MultiRoute = 1,
    /// <summary>Play and record audio category.</summary>
    PlayAndRecord = 2,
    /// <summary>Playback audio category.</summary>
    Playback = 3,
    /// <summary>Record audio category.</summary>
    Record = 4,
    /// <summary>Solo ambient audio category.</summary>
    SoloAmbient = 5,
    /// <summary>Unknown audio category.</summary>
    Unknown = 6
}

/// <summary>
/// Audio session mode.
/// </summary>
public enum AVAudioSessionMode
{
    /// <summary>Default mode.</summary>
    Default = 0,
    /// <summary>Game chat mode.</summary>
    GameChat = 1,
    /// <summary>Measurement mode.</summary>
    Measurement = 2,
    /// <summary>Movie playback mode.</summary>
    MoviePlayback = 3,
    /// <summary>Spoken audio mode.</summary>
    SpokenAudio = 4,
    /// <summary>Video chat mode.</summary>
    VideoChat = 5,
    /// <summary>Voice chat mode.</summary>
    VoiceChat = 6,
    /// <summary>Voice prompt mode.</summary>
    VoicePrompt = 7
}

/// <summary>
/// C# wrapper for the AVAudioSession GDExtension class.
/// Controls the shared iOS/tvOS audio session from C#.
/// </summary>
public class AVAudioSession
{
    private readonly GodotObject _instance;

    /// <summary>
    /// Creates a new AVAudioSession wrapper.
    /// </summary>
    public AVAudioSession(GodotObject instance)
    {
        _instance = instance ?? throw new ArgumentNullException(nameof(instance));
    }

    /// <summary>
    /// Gets the underlying GDExtension object.
    /// </summary>
    public GodotObject Instance => _instance;

    /// <summary>
    /// Gets or sets the current audio session category.
    /// </summary>
    public AVAudioSessionCategory CurrentCategory
    {
        get => (AVAudioSessionCategory)_instance.Get(new StringName("current_category")).AsInt32();
        set => _instance.Set(new StringName("current_category"), (int)value);
    }

    /// <summary>
    /// Configures the shared audio session with specified parameters.
    /// </summary>
    /// <param name="category">The audio category.</param>
    /// <param name="mode">The audio mode.</param>
    /// <param name="policy">The route sharing policy.</param>
    /// <param name="options">The category options.</param>
    /// <returns>Error.Ok on success or Error.Failed if unsupported (e.g., on macOS).</returns>
    public Error SetCategory(
        AVAudioSessionCategory category,
        AVAudioSessionMode mode,
        AVAudioSessionRouteSharingPolicy policy,
        AVAudioSessionCategoryOptions options)
    {
        var result = _instance.Call(
            new StringName("set_category"),
            (int)category,
            (int)mode,
            (int)policy,
            (int)options);
        return (Error)result.AsInt32();
    }
}
