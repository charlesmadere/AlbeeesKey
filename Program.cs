﻿namespace AlbeeesKey;
using System.Diagnostics;
using System.IO;
using System.IO.Pipes;

class Program
{

    // these key values come from here:
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

    static void Main(string[] args)
    {
        var key = KEY_F8;

        var allProcesses = Process.GetProcesses();
        var bizHawkProcess = allProcesses.First(process => process.MainWindowTitle.Contains("BizHawk"));

        var stream = new NamedPipeClientStream(
            "localhost",
            "bizhawk-pid-" + bizHawkProcess.Id + "-IPCKeyInput",
            PipeDirection.Out,
            PipeOptions.None
        );

        stream.Connect();

        var bw = new BinaryWriter(stream);
        Thread.Sleep(96);
        bw.Write(key | 0x80000000);
        Thread.Sleep(96);
        bw.Write(key); // Doing this again is like we are releasing the key
    }

}
