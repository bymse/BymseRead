// See https://aka.ms/new-console-template for more information

using System.Diagnostics;

Process npmProcess;

while (true)
{
    npmProcess = GetNpmWatchProcess();
    await npmProcess.WaitForExitAsync();
    Console.WriteLine($"--> Process exited with code {npmProcess.ExitCode}. Restart");
    npmProcess.Dispose();
}


static Process GetNpmWatchProcess()
{
    var appFrontDir = Path.Combine(Environment.CurrentDirectory, "..", "..", "..", "BymseRead.App", "front");
    var npmProcess = new Process
    {
        StartInfo = new ProcessStartInfo
        {
            FileName = @"C:\Program Files\nodejs\npm.cmd",
            WorkingDirectory = appFrontDir,
            Arguments = "run watch",
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            RedirectStandardInput = true,
            UseShellExecute = false,
            CreateNoWindow = true
        },
    };

    npmProcess.OutputDataReceived += (sender, args) => Console.WriteLine(args.Data);
    npmProcess.ErrorDataReceived += (sender, args) => Console.WriteLine("ERROR -->" + args.Data);
    npmProcess.Start();

    npmProcess.BeginOutputReadLine();
    npmProcess.BeginErrorReadLine();

    return npmProcess;
}