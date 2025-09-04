using System.Text;

namespace Termule.Saddlebag;

[Executor("project new")]
internal class ProjectNewExecutor
{
    public ProjectNewExecutor(string projectName)
    {
        Dictionary<string, string> substitutions = new Dictionary<string, string>()
        {
            { "__CSPROJ__", "csproj" },
            { "__PROJECT_NAME__", projectName },
            { "__LIBRARY_PATH__", Paths.libraryPath },
            { "__BUILD_BOOTSTRAPPER_PATH__", Paths.buildBootstrapperPath },
            { "__PUBLISH_BOOTSTRAPPER_PATH__", Paths.publishBootstrapperPath}
        };

        void GenerateDirectoryFromTemplate(string templatePath, string destinationPath)
        {
            DirectoryInfo directory = new DirectoryInfo(templatePath);
            Directory.CreateDirectory(destinationPath);

            // Duplicate and apply the template to subdirectories
            foreach (DirectoryInfo subdirectory in directory.GetDirectories())
            {
                GenerateDirectoryFromTemplate(subdirectory.FullName, $"{destinationPath}//{ApplySubstitutions(subdirectory.Name)}");
            }

            // Duplicate and apply the template to files
            foreach (FileInfo file in directory.GetFiles())
            {
                string destinationFilePath = $"{destinationPath}//{ApplySubstitutions(file.Name)}";
                file.CopyTo(destinationFilePath);

                byte[] fileBytes = File.ReadAllBytes(destinationFilePath);
                try
                {
                    string fileText = Encoding.UTF8.GetString(fileBytes);
                    File.WriteAllText(destinationFilePath, ApplySubstitutions(fileText));
                }
                catch (DecoderFallbackException) { } // File is not a text file
            }
        }

        string ApplySubstitutions(string input)
        {
            foreach (KeyValuePair<string, string> substitution in substitutions)
            {
                input = input.Replace(substitution.Key, substitution.Value);
            }

            return input;
        }

        GenerateDirectoryFromTemplate(Paths.templateDir, $"{Environment.CurrentDirectory}/{projectName}");
    }
}