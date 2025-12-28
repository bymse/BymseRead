using System.Diagnostics;
using BymseRead.Tools.Helpers;

namespace BymseRead.Tools.Tools;

public static class CompileServiceTool
{
    public static async Task<bool> RunAsync()
    {
        try
        {
            Console.WriteLine("Compiling BymseRead.Service...");

            var projectPath = PathHelper.GetServiceProjectPath();
            Console.WriteLine($"Project path: {projectPath}");

            if (!File.Exists(projectPath))
            {
                Console.Error.WriteLine($"Project file not found: {projectPath}");
                return false;
            }

            var startInfo = new ProcessStartInfo
            {
                FileName = "dotnet",
                Arguments = $"build \"{projectPath}\" --configuration Debug",
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            using var process = new Process { StartInfo = startInfo };

            process.OutputDataReceived += (sender, args) =>
            {
                if (!string.IsNullOrEmpty(args.Data))
                {
                    Console.WriteLine(args.Data);
                }
            };

            process.ErrorDataReceived += (sender, args) =>
            {
                if (!string.IsNullOrEmpty(args.Data))
                {
                    Console.Error.WriteLine(args.Data);
                }
            };

            process.Start();
            process.BeginOutputReadLine();
            process.BeginErrorReadLine();

            await process.WaitForExitAsync();

            if (process.ExitCode == 0)
            {
                Console.WriteLine("Compilation completed successfully");
                return true;
            }
            else
            {
                Console.Error.WriteLine($"Compilation failed with exit code {process.ExitCode}");
                return false;
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Compilation failed: {ex.Message}");
            return false;
        }
    }
}