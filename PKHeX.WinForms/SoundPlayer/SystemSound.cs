using System;

namespace PKHeX.WinForms.SoundPlayer;

public class SystemSound
{
    private string resource;

    internal SystemSound(string tag)
    {
        resource = "./Resources/sounds/" + tag + ".wav";
    }

    public void Play()
    {
        try
        {
            SoundPlayer.PlaySound(resource);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
    }
}
