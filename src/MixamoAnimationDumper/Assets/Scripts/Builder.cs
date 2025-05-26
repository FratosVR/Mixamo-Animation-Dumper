using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEngine;

/// <summary>
/// Class in order to make the build.
/// </summary>
public class CustomBuilder
{
    /// <summary>
    /// Initiate the Windows 64-bit build.
    /// </summary>
    private static void BuildStandaloneWindows64()
    {
        // Setup build options (e.g., scenes, build output location)
        var options = new BuildPlayerOptions
        {
            // Specify the scenes to be included in the build
            scenes = new[]
            {
                Application.dataPath + "/Scenes/SampleScene.unity",
            },
            // Specify the output path for the build
            locationPathName = "../Build/WindowsBuild",
            options = BuildOptions.CleanBuildCache | BuildOptions.StrictMode,
            target = BuildTarget.StandaloneWindows64
        };

        // Run the build and capture the report
        var report = BuildPipeline.BuildPlayer(options);

        if (report.summary.result == BuildResult.Succeeded)
        {
            Debug.Log($"Build successful - Build written to {options.locationPathName}");
        }
        else if (report.summary.result == BuildResult.Failed)
        {
            Debug.LogError("Build failed");
        }
    }

    /// <summary>
    /// Entry point for the build process.
    /// </summary>
    [MenuItem("Assets/BuildWindows")]
    public static void Build()
    {
        // Initiate the Windows 64-bit build
        BuildStandaloneWindows64();
    }
}
