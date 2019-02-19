#tool "nuget:?package=GitVersion.CommandLine&version=4.0.0"

//////////////////////////////////////////////////////////////////////
// ARGUMENTS
//////////////////////////////////////////////////////////////////////

var target = Argument("target", "Default");
var configuration = Argument("configuration", "Debug");
var solution = GetFiles("*.sln").First();

//////////////////////////////////////////////////////////////////////
// SETUP
//////////////////////////////////////////////////////////////////////


Setup(context =>
{
    if(AppVeyor.IsRunningOnAppVeyor)
    {
        // Pass version info to AppVeyor
        GitVersion(new GitVersionSettings{
            ArgumentCustomization = args => args.Append("-verbosity Warn"),
            OutputType = GitVersionOutput.BuildServer
        });
    }
});

//////////////////////////////////////////////////////////////////////
// TASKS
//////////////////////////////////////////////////////////////////////

Task("Clean")
    .Does(() =>
{
    CleanDirectories("**/bin");
    CleanDirectories("**/obj");
});

Task("Restore")
    .IsDependentOn("Clean")
    .Does(() =>
{
    NuGetRestore(solution);
});

Task("Build")
    .IsDependentOn("Restore")
    .Does(() =>
{
    MSBuild(solution, new MSBuildSettings {
        Configuration = configuration
    });
});

Task("Package")
    .IsDependentOn("Build")
    .Does(() =>
{
    Zip($"Output/{configuration}/net472", "Output/Bloop.zip");
});


//////////////////////////////////////////////////////////////////////
// TASK TARGETS
//////////////////////////////////////////////////////////////////////

Task("Default")
    .IsDependentOn("Package");

//////////////////////////////////////////////////////////////////////
// EXECUTION
//////////////////////////////////////////////////////////////////////

RunTarget(target);
