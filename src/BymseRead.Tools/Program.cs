using BymseRead.Tools.Tools;

if (args.Length == 0)
{
    Console.WriteLine("Specify tool to run");
    return 0;
}

var toolName = args[0];

try
{
    var success = toolName.ToLowerInvariant() switch
    {
        "setupstorage" => await SetupStorageTool.RunAsync(),
        "setupkeycloak" => await SetupKeycloakTool.RunAsync(),
        "setupdevconfig" => await SetupDevConfigTool.RunAsync(),
        "compileservice" => await CompileServiceTool.RunAsync(),
        "generatedoc" => await GenerateDocTool.RunAsync(),
        "generateclients" => await GenerateClientsTool.RunAsync(),
        _ => throw new ArgumentException($"Unknown tool: {toolName}")
    };

    return success ? 0 : 2;
}
catch (ArgumentException ex)
{
    Console.Error.WriteLine($"Error: {ex.Message}");
    Console.Error.WriteLine();
    return 1;
}
catch (Exception ex)
{
    Console.Error.WriteLine($"Error executing tool '{toolName}': {ex.Message}");
    Console.Error.WriteLine(ex.StackTrace);
    return 2;
}