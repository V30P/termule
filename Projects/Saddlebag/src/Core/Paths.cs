using System.Reflection;

namespace Termule.Saddlebag;

internal static class Paths
{
    static readonly string saddlebagPath = ConvertToUnixPath(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location));
    static readonly string sdkPath = ConvertToUnixPath(new DirectoryInfo(saddlebagPath).Parent.FullName);

    internal static readonly string libraryPath = $"{sdkPath}/Library/Termule.dll";
    internal static readonly string buildBootstrapperPath = $"{sdkPath}/Bootstrapper/build/TermuleBootstrapper.exe";
    internal static readonly string publishBootstrapperPath = $"{sdkPath}/Bootstrapper/publish/TermuleBootstrapper.exe";
    internal static readonly string templateDir = $"{saddlebagPath}/Template/";

    // JSON files don't like backslashes, so use this to be safe
    internal static string ConvertToUnixPath(string path) => path.Replace("\\", "/");
}