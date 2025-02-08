namespace AlbeeesKey;

using System.Diagnostics;
using System.IO;
using System.IO.Pipes;

// A majority of the knowledge for writing this code came from this GitHub issue:
// https://github.com/TASEmulators/BizHawk/issues/477

public class Program
{

    // The following key values come from here:
    // https://github.com/AndersMalmgren/FreePIE/blob/master/FreePIE.Core.Plugins/KeyboardPlugin.cs
    private static readonly int BIZHAWK_KEY_D0 = 0;
    private static readonly int BIZHAWK_KEY_D1 = 1;
    private static readonly int BIZHAWK_KEY_D2 = 2;
    private static readonly int BIZHAWK_KEY_D3 = 3;
    private static readonly int BIZHAWK_KEY_D4 = 4;
    private static readonly int BIZHAWK_KEY_D5 = 5;
    private static readonly int BIZHAWK_KEY_D6 = 6;
    private static readonly int BIZHAWK_KEY_D7 = 7;
    private static readonly int BIZHAWK_KEY_D8 = 8;
    private static readonly int BIZHAWK_KEY_D9 = 9;
    private static readonly int BIZHAWK_KEY_A = 10;
    private static readonly int BIZHAWK_KEY_B = 11;
    private static readonly int BIZHAWK_KEY_C = 12;
    private static readonly int BIZHAWK_KEY_D = 13;
    private static readonly int BIZHAWK_KEY_E = 14;
    private static readonly int BIZHAWK_KEY_F = 15;
    private static readonly int BIZHAWK_KEY_G = 16;
    private static readonly int BIZHAWK_KEY_H = 17;
    private static readonly int BIZHAWK_KEY_I = 18;
    private static readonly int BIZHAWK_KEY_J = 19;
    private static readonly int BIZHAWK_KEY_K = 20;
    private static readonly int BIZHAWK_KEY_L = 21;
    private static readonly int BIZHAWK_KEY_M = 22;
    private static readonly int BIZHAWK_KEY_N = 23;
    private static readonly int BIZHAWK_KEY_O = 24;
    private static readonly int BIZHAWK_KEY_P = 25;
    private static readonly int BIZHAWK_KEY_Q = 26;
    private static readonly int BIZHAWK_KEY_R = 27;
    private static readonly int BIZHAWK_KEY_S = 28;
    private static readonly int BIZHAWK_KEY_T = 29;
    private static readonly int BIZHAWK_KEY_U = 30;
    private static readonly int BIZHAWK_KEY_V = 31;
    private static readonly int BIZHAWK_KEY_W = 32;
    private static readonly int BIZHAWK_KEY_X = 33;
    private static readonly int BIZHAWK_KEY_Y = 34;
    private static readonly int BIZHAWK_KEY_Z = 35;
    private static readonly int BIZHAWK_KEY_COMMA = 47;
    private static readonly int BIZHAWK_KEY_PERIOD = 111;
    private static readonly int BIZHAWK_KEY_BACKSPACE = 42;
    private static readonly int BIZHAWK_KEY_RETURN = 117;
    private static readonly int BIZHAWK_KEY_SPACE = 126;
    private static readonly int BIZHAWK_KEY_ARROW_DOWN = 50;
    private static readonly int BIZHAWK_KEY_ARROW_LEFT = 76;
    private static readonly int BIZHAWK_KEY_ARROW_RIGHT = 118;
    private static readonly int BIZHAWK_KEY_ARROW_UP = 132;
    private static readonly int BIZHAWK_KEY_F1 = 54;
    private static readonly int BIZHAWK_KEY_F2 = 55;
    private static readonly int BIZHAWK_KEY_F3 = 56;
    private static readonly int BIZHAWK_KEY_F4 = 57;
    private static readonly int BIZHAWK_KEY_F5 = 58;
    private static readonly int BIZHAWK_KEY_F6 = 59;
    private static readonly int BIZHAWK_KEY_F7 = 60;
    private static readonly int BIZHAWK_KEY_F8 = 61;
    private static readonly int BIZHAWK_KEY_F9 = 62;
    private static readonly int BIZHAWK_KEY_F10 = 63;
    private static readonly int BIZHAWK_KEY_F11 = 64;
    private static readonly int BIZHAWK_KEY_F12 = 65;
    private static readonly int BIZHAWK_KEY_F13 = 66;
    private static readonly int BIZHAWK_KEY_F14 = 67;
    private static readonly int BIZHAWK_KEY_F15 = 68;

    private enum Key {
        D0,
        D1,
        D2,
        D3,
        D4,
        D5,
        D6,
        D7,
        D8,
        D9,
        A,
        B,
        C,
        D,
        E,
        F,
        G,
        H,
        I,
        J,
        K,
        L,
        M,
        N,
        O,
        P,
        Q,
        R,
        S,
        T,
        U,
        V,
        W,
        X,
        Y,
        Z,
        COMMA,
        PERIOD,
        BACKSPACE,
        RETURN,
        SPACE,
        ARROW_DOWN,
        ARROW_LEFT,
        ARROW_RIGHT,
        ARROW_UP,
        F1,
        F2,
        F3,
        F4,
        F5,
        F6,
        F7,
        F8,
        F9,
        F10,
        F11,
        F12,
        F13,
        F14,
        F15,
    }

    private record KeyData(Key key, int bizhawkValue, string humanName);

    private static readonly List<KeyData> KEYS = [
        new(Key.D0, BIZHAWK_KEY_D0, "0"),
        new(Key.D1, BIZHAWK_KEY_D1, "1"),
        new(Key.D2, BIZHAWK_KEY_D2, "2"),
        new(Key.D3, BIZHAWK_KEY_D3, "3"),
        new(Key.D4, BIZHAWK_KEY_D4, "4"),
        new(Key.D5, BIZHAWK_KEY_D5, "5"),
        new(Key.D6, BIZHAWK_KEY_D6, "6"),
        new(Key.D7, BIZHAWK_KEY_D7, "7"),
        new(Key.D8, BIZHAWK_KEY_D8, "8"),
        new(Key.D9, BIZHAWK_KEY_D9, "9"),
        new(Key.A, BIZHAWK_KEY_A, "A"),
        new(Key.B, BIZHAWK_KEY_B, "B"),
        new(Key.C, BIZHAWK_KEY_C, "C"),
        new(Key.D, BIZHAWK_KEY_D, "D"),
        new(Key.E, BIZHAWK_KEY_E, "E"),
        new(Key.F, BIZHAWK_KEY_F, "F"),
        new(Key.G, BIZHAWK_KEY_G, "G"),
        new(Key.H, BIZHAWK_KEY_H, "H"),
        new(Key.I, BIZHAWK_KEY_I, "I"),
        new(Key.J, BIZHAWK_KEY_J, "J"),
        new(Key.K, BIZHAWK_KEY_K, "K"),
        new(Key.L, BIZHAWK_KEY_L, "L"),
        new(Key.M, BIZHAWK_KEY_M, "M"),
        new(Key.N, BIZHAWK_KEY_N, "N"),
        new(Key.O, BIZHAWK_KEY_O, "O"),
        new(Key.P, BIZHAWK_KEY_P, "P"),
        new(Key.Q, BIZHAWK_KEY_Q, "Q"),
        new(Key.R, BIZHAWK_KEY_R, "R"),
        new(Key.S, BIZHAWK_KEY_S, "S"),
        new(Key.T, BIZHAWK_KEY_T, "T"),
        new(Key.U, BIZHAWK_KEY_U, "U"),
        new(Key.V, BIZHAWK_KEY_V, "V"),
        new(Key.W, BIZHAWK_KEY_W, "W"),
        new(Key.X, BIZHAWK_KEY_X, "X"),
        new(Key.Y, BIZHAWK_KEY_Y, "Y"),
        new(Key.Z, BIZHAWK_KEY_Z, "Z"),
        new(Key.COMMA, BIZHAWK_KEY_COMMA, "comma"),
        new(Key.PERIOD, BIZHAWK_KEY_PERIOD, "period"),
        new(Key.BACKSPACE, BIZHAWK_KEY_BACKSPACE, "backspace"),
        new(Key.RETURN, BIZHAWK_KEY_RETURN, "return"),
        new(Key.SPACE, BIZHAWK_KEY_SPACE, "space"),
        new(Key.ARROW_DOWN, BIZHAWK_KEY_ARROW_DOWN, "arrowdown"),
        new(Key.ARROW_LEFT, BIZHAWK_KEY_ARROW_LEFT, "arrowleft"),
        new(Key.ARROW_RIGHT, BIZHAWK_KEY_ARROW_RIGHT, "arrowright"),
        new(Key.ARROW_UP, BIZHAWK_KEY_ARROW_UP, "arrowup"),
        new(Key.F1, BIZHAWK_KEY_F1, "F1"),
        new(Key.F2, BIZHAWK_KEY_F2, "F2"),
        new(Key.F3, BIZHAWK_KEY_F3, "F3"),
        new(Key.F4, BIZHAWK_KEY_F4, "F4"),
        new(Key.F5, BIZHAWK_KEY_F5, "F5"),
        new(Key.F6, BIZHAWK_KEY_F6, "F6"),
        new(Key.F7, BIZHAWK_KEY_F7, "F7"),
        new(Key.F8, BIZHAWK_KEY_F8, "F8"),
        new(Key.F9, BIZHAWK_KEY_F9, "F9"),
        new(Key.F10, BIZHAWK_KEY_F10, "F10"),
        new(Key.F11, BIZHAWK_KEY_F11, "F11"),
        new(Key.F12, BIZHAWK_KEY_F12, "F12"),
        new(Key.F13, BIZHAWK_KEY_F13, "F13"),
        new(Key.F14, BIZHAWK_KEY_F14, "F14"),
        new(Key.F15, BIZHAWK_KEY_F15, "F15"),
    ];

    // This value can probably be tweaked some amount, but I don't really know what would
    // be considered the "minimum viable value". However in testing, it seems that 96 is
    // perfectly good.
    private static readonly int KEY_PRESS_DELAY_MILLISECONDS = 96;

    public static void Main(string[]? args)
    {
        var keyPress = DetermineKeyPress(args);
        var timeMillis = DetermineKeyPressTime(args);
        SendKeyPressToBizHawk(keyPress, timeMillis);
    }

    private static KeyData DefaultKeyPress() {
        var defaultKey = KEYS.First(key =>
            key.key == Key.F8
        );

        if (defaultKey == null) {
            throw new Exception($"Unable to find default F8 BizHawk key: \"{defaultKey}\"");
        }

        return defaultKey;
    }

    private static KeyData DetermineKeyPress(string[]? args)
    {
        if (args == null || args.Length == 0)
        {
            return DefaultKeyPress();
        }

        var keyArgument = args[0];

        if (string.IsNullOrWhiteSpace(keyArgument))
        {
            return DefaultKeyPress();
        }

        var keyPress = KEYS.First(
            key => string.Equals(key.humanName, keyArgument, StringComparison.OrdinalIgnoreCase)
        );

        if (keyPress == null) {
            throw new Exception($"Unable to determine which BizHawk key applies to the given key argument: \"{keyPress}\"");
        }

        return keyPress;
    }

    private static int DetermineKeyPressTime(string[]? args)
    {
        if (args == null || args.Length <= 1)
        {
            return KEY_PRESS_DELAY_MILLISECONDS;
        }

        var timeMillisArgument = args[1];

        if (string.IsNullOrWhiteSpace(timeMillisArgument))
        {
            return KEY_PRESS_DELAY_MILLISECONDS;
        }

        try
        {
            return int.Parse(timeMillisArgument);
        }
        catch
        {
            throw new Exception($"Unable to parse the given time argument into milliseconds: \"{timeMillisArgument}\"");
        }
    }

    private static void SendKeyPressToBizHawk(KeyData keyPress, int timeMillis)
    {
        var allProcesses = Process.GetProcesses();

        // find the BizHawk process, as we need its process ID (PID)
        var bizHawkProcess = allProcesses.First(process =>
            process.MainWindowTitle.Contains("BizHawk") && process.ProcessName.Contains("EmuHawk")
        );

        // this part comes from the GitHub issue:
        // https://github.com/TASEmulators/BizHawk/issues/477#issuecomment-131264972
        var pipeName = "bizhawk-pid-" + bizHawkProcess.Id + "-IPCKeyInput";

        var stream = new NamedPipeClientStream(
            "localhost",
            pipeName,
            PipeDirection.Out,
            PipeOptions.None
        );

        stream.Connect();

        // the below code segment sends the actual key press to BizHawk
        var writer = new BinaryWriter(stream);
        Thread.Sleep(timeMillis);
        writer.Write(keyPress.bizhawkValue | 0x80000000);
        Thread.Sleep(timeMillis);
        writer.Write(keyPress.bizhawkValue); // doing this again is like we are releasing the key
        writer.Close();
    }

}
