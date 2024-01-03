namespace AlbeeesKey;

using System.Diagnostics;
using System.IO;
using System.IO.Pipes;

// A majority of the knowledge for writing this code came from this GitHub issue:
// https://github.com/TASEmulators/BizHawk/issues/477

class Program
{

    // These key values come from here:
    // https://github.com/AndersMalmgren/FreePIE/blob/master/FreePIE.Core.Plugins/KeyboardPlugin.cs
    private static readonly int KEY_F1 = 54;
    private static readonly int KEY_F2 = 55;
    private static readonly int KEY_F3 = 56;
    private static readonly int KEY_F4 = 57;
    private static readonly int KEY_F5 = 58;
    private static readonly int KEY_F6 = 59;
    private static readonly int KEY_F7 = 60;
    private static readonly int KEY_F8 = 61;
    private static readonly int KEY_F9 = 62;
    private static readonly int KEY_F10 = 63;
    private static readonly int KEY_F11 = 64;
    private static readonly int KEY_F12 = 65;
    private static readonly int KEY_F13 = 66;
    private static readonly int KEY_F14 = 67;
    private static readonly int KEY_F15 = 68;

    // This value can probably be tweaked some amount, but I don't really know what would
    // be considered the "minimum viable value". However in testing, it seems that 96 is
    // perfectly good.
    private static readonly int KEY_PRESS_DELAY_MILLISECONDS = 96;

    static void Main(string[] args)
    {
        var key = DetermineKey(args);
        SendKeyPressToBizHawk(key);
    }

    private static int DetermineKey(string[] args)
    {
        if (args == null || args.Length == 0)
        {
            return KEY_F8;
        }

        // TODO
        return KEY_F8;
    }

    private static void SendKeyPressToBizHawk(int key)
    {
        var allProcesses = Process.GetProcesses();

        // Find the BizHawk process, as we need its process ID (PID).
        var bizHawkProcess = allProcesses.First(process => process.MainWindowTitle.Contains("BizHawk"));

        // // This part comes from the GitHub issue:
        // https://github.com/TASEmulators/BizHawk/issues/477#issuecomment-131264972
        var pipeName = "bizhawk-pid-" + bizHawkProcess.Id + "-IPCKeyInput";

        var stream = new NamedPipeClientStream(
            "localhost",
            pipeName,
            PipeDirection.Out,
            PipeOptions.None
        );

        stream.Connect();

        var writer = new BinaryWriter(stream);
        Thread.Sleep(KEY_PRESS_DELAY_MILLISECONDS);
        writer.Write(key | 0x80000000);
        Thread.Sleep(KEY_PRESS_DELAY_MILLISECONDS);
        writer.Write(key); // Doing this again is like we are releasing the key
    }

}
