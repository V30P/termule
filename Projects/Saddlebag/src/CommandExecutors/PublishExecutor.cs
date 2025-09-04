namespace Termule.Saddlebag;

[Executor("publish")]
internal class PublishExecutor
{
    internal readonly string publishPath;
    internal bool succeeded => publishPath != null;

    public PublishExecutor()
    {
        BuildExecutor buildExecutor = new BuildExecutor();
        if (buildExecutor.succeeded)
        {
            string publishDirectory = ProjectManager.project.GetPropertyValue("PublishDir") ?? buildExecutor.outputPath;
            publishPath = $"{publishDirectory}{Path.GetFileNameWithoutExtension(ProjectManager.project.FullPath)}.exe";
            
            Directory.CreateDirectory(publishDirectory);
            using FileStream outputFileStream = File.Create(publishPath);

            // Duplicate the bootstrapper
            outputFileStream.Write(File.ReadAllBytes(Paths.publishBootstrapperPath));

            // Add the built project dll
            byte[] projectBytes = File.ReadAllBytes(buildExecutor.outputPath);
            outputFileStream.Write(projectBytes);

            // Add the length of the project so it can be loaded by the bootloader
            outputFileStream.Write(BitConverter.GetBytes(projectBytes.Length));

            Console.WriteLine($"Publish succeeded -> {publishPath}");
        }
    }
}