using Nuke.Common;
using Nuke.Common.IO;
using Nuke.Common.Tooling;

partial class Build
{
    AbsolutePath OpenApiDoc => Solution.BymseRead_Service_Client.Directory / "openapi.json";
    
    AbsolutePath WebClientAppDir => Solution.Directory / "WebClientApp";

    [NuGetPackage(packageId: "Swashbuckle.AspNetCore.Cli",
        packageExecutable: "dotnet-swagger.dll",
        Framework = "net8.0")]
    Tool SwashbuckleCli;

    [NuGetPackage(
        packageId: "Microsoft.OpenApi.Kiota",
        packageExecutable: "kiota.dll",
        Framework = "net8.0")]
    Tool Kiota;

    Target GenerateDoc => target => target
        .DependsOn(CompileService)
        .Executes(() =>
        {
            var dllPath = Solution.BymseRead_Service.Directory / "bin" / Configuration / "net8.0" / "BymseRead.Service.dll";
            SwashbuckleCli($"tofile --output {OpenApiDoc} {dllPath} WebApi");
        });

    Target GenerateClients => target => target
        .DependsOn(GenerateDoc)
        .Executes(() =>
        {
            Kiota(
                $"generate -d {OpenApiDoc} -l CSharp -c BymseReadClient -n BymseRead.Service.Client -o {Solution.BymseRead_Service_Client.Directory / "Client"}");

            Kiota(
                $"generate -d {OpenApiDoc} -l typescript -c BymseReadClient -o {WebClientAppDir / "generated" / "api"}");
        });
}