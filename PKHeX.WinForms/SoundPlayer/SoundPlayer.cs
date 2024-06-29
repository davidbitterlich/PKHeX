using System;
using System.IO;
using OpenTK.Audio.OpenAL;

namespace PKHeX.WinForms.SoundPlayer;

public class SoundPlayer
{
    private static int _buffer;
    private static int _source;
    private static bool _isInitialized;
    private static ALContext ctx;

    static SoundPlayer()
    {
        Initialize();
    }


    private static void Initialize()
    {
        if (_isInitialized) return;

        ALDevice device = ALC.OpenDevice(null);
        ALContext context = ALC.CreateContext(device, (int[])null!);
        ctx = context;
        ALC.MakeContextCurrent(context);

        _buffer = AL.GenBuffer();
        _source = AL.GenSource();
        _isInitialized = true;
    }
    public static void PlaySound(byte[] soundBytes)
    {
        if (soundBytes == null || soundBytes.Length == 0)
            throw new ArgumentException("Sound data is empty or null.");

        byte[] soundData;
        int channels, bits_per_sample, sample_rate;

        using (var stream = new MemoryStream(soundBytes))
        {
            using (var reader = new BinaryReader(stream))
            {
                // Read WAV header (assuming it's PCM)
                reader.ReadChars(4); // chunkID
                reader.ReadInt32(); // fileSize
                reader.ReadChars(4); // riffType

                reader.ReadChars(4); // fmtID
                reader.ReadInt32(); // fmtSize
                reader.ReadInt16(); // fmtCode
                channels = reader.ReadInt16();
                sample_rate = reader.ReadInt32();
                reader.ReadInt32(); // fmtAvgBPS
                reader.ReadInt16(); // fmtBlockAlign
                bits_per_sample = reader.ReadInt16();

                reader.ReadChars(4); // dataID
                int dataSize = reader.ReadInt32();

                soundData = reader.ReadBytes(dataSize);
            }
        }

        ALFormat format = channels == 1 ? ALFormat.Mono16 : ALFormat.Stereo16;
        IntPtr soundDataPtr = IntPtr.Zero;

        try
        {
            soundDataPtr = System.Runtime.InteropServices.Marshal.UnsafeAddrOfPinnedArrayElement(soundData, 0);
            AL.BufferData(_buffer, format, soundDataPtr, soundData.Length, sample_rate);
        }
        finally
        {
            // Ensure the pointer is freed, if needed
        }

        AL.Source(_source, ALSourcei.Buffer, _buffer);
        AL.SourcePlay(_source);
    }
    public static void PlaySound(string soundFilePath)
    {
        var curDir = Directory.GetCurrentDirectory();
        if (!File.Exists(soundFilePath))
            throw new FileNotFoundException("Sound file not found.", soundFilePath);

        byte[] soundData;
        int channels, bits_per_sample, sample_rate;

        using (var stream = File.Open(soundFilePath, FileMode.Open))
        {
            using (var reader = new BinaryReader(stream))
            {
                // Read WAV header (assuming it's PCM)
                reader.ReadChars(4); // chunkID
                reader.ReadInt32(); // fileSize
                reader.ReadChars(4); // riffType

                reader.ReadChars(4); // fmtID
                reader.ReadInt32(); // fmtSize
                reader.ReadInt16(); // fmtCode
                channels = reader.ReadInt16();
                sample_rate = reader.ReadInt32();
                reader.ReadInt32(); // fmtAvgBPS
                reader.ReadInt16(); // fmtBlockAlign
                bits_per_sample = reader.ReadInt16();

                reader.ReadChars(4); // dataID
                int dataSize = reader.ReadInt32();

                soundData = reader.ReadBytes(dataSize);
            }
        }

        ALFormat format = channels == 1 ? ALFormat.Mono16 : ALFormat.Stereo16;
        IntPtr soundDataPtr = IntPtr.Zero;

        try
        {
            soundDataPtr = System.Runtime.InteropServices.Marshal.UnsafeAddrOfPinnedArrayElement(soundData, 0);
            AL.BufferData(_buffer, format, soundDataPtr, soundData.Length, sample_rate);
        }
        finally
        {
            // Ensure the pointer is freed, if needed
        }

        AL.Source(_source, ALSourcei.Buffer, _buffer);
        AL.SourcePlay(_source);
    }

    public static void Cleanup()
    {
        AL.SourceStop(_source);
        AL.DeleteSource(_source);
        AL.DeleteBuffer(_buffer);

        ALDevice currentDevice = ALC.GetContextsDevice(ctx);
        ALC.MakeContextCurrent(ALContext.Null);
        ALC.DestroyContext(ctx);
        ALC.CloseDevice(currentDevice);

        _isInitialized = false;
    }

    public static void Stop()
    {
        AL.SourceStop(_source);
    }

}
