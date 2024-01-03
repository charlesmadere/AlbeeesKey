namespace AlbeeesKey;
using System.Diagnostics;
using System.IO;
using System.IO.Pipes;

class Program
{

    static void Main(string[] args)
    {
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
        bw.Write(61 | 0x80000000); // 61 corresponds to the F8 key
        Thread.Sleep(96);
        bw.Write(61); // Doing this again is like we are releasing the F8 key
    }

}
