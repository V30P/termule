using Microsoft.Build.Evaluation;

namespace Termule.Saddlebag;

internal static class Builder
{
    internal static bool TryBuild(Project project, string outputPath)
    {
        // Build the project
        bool buildSucceeded = project.Build();
        if (!buildSucceeded) return false;

        using FileStream outputFileStream = File.Create(outputPath);

        // Duplicate the bootstrapper
        outputFileStream.Write(File.ReadAllBytes(Environment.config.bootstrapperPath));

        // Add the built project dll
        byte[] projectBytes = File.ReadAllBytes(project.GetPropertyValue("TargetPath"));
        outputFileStream.Write(projectBytes);

        // Add the length of the project so it can be loaded by the bootloader
        outputFileStream.Write(BitConverter.GetBytes(projectBytes.Length));

        return true;
    }
}