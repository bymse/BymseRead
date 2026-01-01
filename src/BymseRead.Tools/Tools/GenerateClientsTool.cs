using System.Diagnostics;
using BymseRead.Tools.Helpers;

namespace BymseRead.Tools.Tools;

public static class GenerateClientsTool
{
    public static async Task<bool> RunAsync()
    {
        try
        {
            Console.WriteLine("Generating API clients...");

            Console.WriteLine("Step 1: Generating OpenAPI documentation...");
            var docSuccess = await GenerateDocTool.RunAsync();
            if (!docSuccess)
            {
                Console.Error.WriteLine("Failed to generate OpenAPI documentation");
                return false;
            }

            Console.WriteLine();

            Console.WriteLine("Step 2: Preparing to generate clients...");
            var clientDirectory = PathHelper.GetClientDirectory();
            var openApiDocPath = Path.Combine(clientDirectory, "openapi.json");

            if (!File.Exists(openApiDocPath))
            {
                Console.Error.WriteLine($"OpenAPI document not found at: {openApiDocPath}");
                return false;
            }

            Console.WriteLine($"Using OpenAPI document: {openApiDocPath}");

            Console.WriteLine("Step 3: Generating C# client...");
            var csharpOutputPath = Path.Combine(clientDirectory, "Client");
            var csharpSuccess = await RunKiotaAsync(
                openApiDocPath,
                "CSharp",
                "BymseReadClient",
                "BymseRead.Service.Client",
                csharpOutputPath
            );

            if (!csharpSuccess)
            {
                Console.Error.WriteLine("Failed to generate C# client");
                return false;
            }

            Console.WriteLine($"C# client generated at: {csharpOutputPath}");
            Console.WriteLine();

            Console.WriteLine("Step 4: Generating TypeScript client...");
            var webClientAppPath = PathHelper.GetWebClientAppPath();
            var typescriptOutputPath = Path.Combine(webClientAppPath, "generated", "api");
            var typescriptSuccess = await RunKiotaAsync(
                openApiDocPath,
                "typescript",
                "BymseReadClient",
                null,
                typescriptOutputPath
            );

            if (!typescriptSuccess)
            {
                Console.Error.WriteLine("Failed to generate TypeScript client");
                return false;
            }

            Console.WriteLine($"TypeScript client generated at: {typescriptOutputPath}");
            Console.WriteLine();

            Console.WriteLine("API clients generated successfully");
            return true;
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Client generation failed: {ex.Message}");
            return false;
        }
    }

    private static async Task<bool> RunKiotaAsync(
        string openApiDoc,
        string language,
        string className,
        string? namespaceName,
        string outputPath)
    {
        var args = $"tool run kiota generate -d \"{openApiDoc}\" -l {language} -c {className} -o \"{outputPath}\" --exclude-backward-compatible --clean-output";

        if (!string.IsNullOrEmpty(namespaceName))
        {
            args += $" -n {namespaceName}";
        }

        var startInfo = new ProcessStartInfo
        {
            FileName = "dotnet",
            Arguments = args,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
            CreateNoWindow = true
        };

        using var process = new Process();
        process.StartInfo = startInfo;

        process.OutputDataReceived += (sender, eventArgs) =>
        {
            if (!string.IsNullOrEmpty(eventArgs.Data))
            {
                Console.WriteLine(eventArgs.Data);
            }
        };

        process.ErrorDataReceived += (sender, eventArgs) =>
        {
            if (!string.IsNullOrEmpty(eventArgs.Data))
            {
                Console.Error.WriteLine(eventArgs.Data);
            }
        };

        process.Start();
        process.BeginOutputReadLine();
        process.BeginErrorReadLine();

        await process.WaitForExitAsync();

        return process.ExitCode == 0;
    }
}