using System.Collections.Generic;
using System.IO;
using System.Linq;

using dosymep.Nuke.RevitVersions;

using Nuke.Common;
using Nuke.Common.IO;
using Nuke.Common.Tooling;
using Nuke.Common.Tools.DotNet;

using static Nuke.Common.Tools.DotNet.DotNetTasks;

class Build : NukeBuild {
    /// Support plugins are available for:
    ///   - JetBrains ReSharper        https://nuke.build/resharper
    ///   - JetBrains Rider            https://nuke.build/rider
    ///   - Microsoft VisualStudio     https://nuke.build/visualstudio
    ///   - Microsoft VSCode           https://nuke.build/vscode
    public static int Main() => Execute<Build>(x => x.Compile);

    [Parameter("Configuration to build - Default is 'Debug' (local) or 'Release' (server)")]
    readonly Configuration Configuration = IsLocalBuild ? Configuration.Debug : Configuration.Release;

    [Parameter] readonly AbsolutePath Output = RootDirectory / "bin";
    [Parameter] readonly string DocsOutput = Path.Combine("docs", "_site");
    [Parameter] readonly string DocsConfig = Path.Combine("docs", "docfx.json");

    /// <summary>
    /// Min Revit version.
    /// </summary>
    [Parameter("Min Revit version.")]
    readonly RevitVersion MinVersion = RevitVersion.Rv2020;

    /// <summary>
    /// Max Revit version.
    /// </summary>
    [Parameter("Max Revit version.")]
    readonly RevitVersion MaxVersion = RevitVersion.Rv2024;

    [Parameter("Build Revit versions.")] 
    readonly RevitVersion[] RevitVersions = new RevitVersion[0];

    IEnumerable<RevitVersion> BuildRevitVersions;

    protected override void OnBuildInitialized() {
        base.OnBuildInitialized();
        BuildRevitVersions = RevitVersions.Length > 0
            ? RevitVersions
            : RevitVersion.GetRevitVersions(MinVersion, MaxVersion);
    }

    Target Clean => _ => _
        .Executes(() => {
            Output.CreateOrCleanDirectory();
            (RootDirectory / DocsOutput).CreateOrCleanDirectory();
            RootDirectory.GlobDirectories("**/bin", "**/obj")
                .Where(item => item != (RootDirectory / "build" / "bin"))
                .Where(item => item != (RootDirectory / "build" / "obj"))
                .DeleteDirectories();
        });

    Target Compile => _ => _
        .DependsOn(Clean)
        .Executes(() => {
            DotNetBuild(s => s
                .EnableForce()
                .DisableNoRestore()
                .SetConfiguration(Configuration)
                .When(IsServerBuild, _ => _
                    .EnableContinuousIntegrationBuild())
                .CombineWith(BuildRevitVersions, (settings, version) => {
                    return settings
                        .SetOutputDirectory(Output / version)
                        .SetProperty("RevitVersion", (int) version);
                }));
        });

    Target DocsCompile => _ => _
        .DependsOn(Compile)
        .Executes(() => {
            // В nuget.org лежит старая версия
            ProcessTasks.StartProcess(
                "dotnet",
                "tool install -g docfx");

            ProcessTasks.StartProcess(
                "docfx",
                DocsConfig
                + (IsLocalBuild
                    ? " --serve"
                    : string.Empty),
                RootDirectory).WaitForExit();

            // DocFXBuild(s => s
            //     .EnableForceRebuild()
            //     .SetServe(IsLocalBuild)
            //     .SetOutputFolder(DocsOutput)
            //     .SetProcessWorkingDirectory(RootDirectory)
            // );
        });
}