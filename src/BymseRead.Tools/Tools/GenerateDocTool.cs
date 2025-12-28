using System.Diagnostics;
using BymseRead.Tools.Helpers;

namespace BymseRead.Tools.Tools;

public static class GenerateDocTool
{
    public static async Task<bool> RunAsync()
    {
        try
        {
            Console.WriteLine("Generating OpenAPI documentation...");

            Console.WriteLine("Step 1: Compiling service...");
            var compileSuccess = await CompileServiceTool.RunAsync();
            if (!compileSuccess)
            {
                Console.Error.WriteLine("Failed to compile service");
                return false;
            }

            Console.WriteLine();

            Console.WriteLine("Step 2: Locating compiled service DLL...");
            var serviceDirectory = PathHelper.GetServiceDirectory();
            var serviceDllPath = Path.Combine(serviceDirectory, "bin", "Debug", Constants.Tools.DotNetFramework, "BymseRead.Service.dll");

            if (!File.Exists(serviceDllPath))
            {
                Console.Error.WriteLine($"Compiled service DLL not found at: {serviceDllPath}");
                return false;
            }

            Console.WriteLine($"Found service DLL at: {serviceDllPath}");

            Console.WriteLine("Step 3: Generating OpenAPI spec...");
            var clientDirectory = PathHelper.GetClientDirectory();
            var openApiDocPath = Path.Combine(clientDirectory, "openapi.json");

            var startInfo = new ProcessStartInfo
            {
                FileName = "dotnet",
                Arguments = $"tool run swagger tofile --output \"{openApiDocPath}\" \"{serviceDllPath}\" WebApi",
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            using var process = new Process();
            process.StartInfo = startInfo;

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
                Console.WriteLine($"OpenAPI documentation generated successfully at: {openApiDocPath}");
                return true;
            }
            else
            {
                Console.Error.WriteLine($"OpenAPI generation failed with exit code {process.ExitCode}");
                return false;
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"OpenAPI generation failed: {ex.Message}");
            return false;
        }
    }
}