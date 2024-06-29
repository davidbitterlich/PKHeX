namespace PKHeX.WinForms.SoundPlayer;

public sealed class SystemSounds
{
    private SystemSounds()
    {

    }

    public static SystemSound Asterisk
    {
        get => new SystemSound("Asterisk");
    }

    public static SystemSound Beep
    {
        get => new SystemSound("Beep");
    }

    public static SystemSound Exclamation
    {
        get => new SystemSound("Hand");
    }

    public static SystemSound Question
    {
        get => new SystemSound("Question");
    }

    public static SystemSound Hand
    {
        get => new SystemSound("Hand");
    }
}
