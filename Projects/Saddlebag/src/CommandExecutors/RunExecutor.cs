using System.Diagnostics;

namespace Termule.Saddlebag;

[Executor("run")]
public class RunExecutor
{
    public RunExecutor()
    {
        BuildExecutor buildExecutor = new BuildExecutor();
        if (buildExecutor.succeeded)
        {
            Process process = Process.Start(Paths.buildBootstrapperPath, buildExecutor.outputPath);
            process.WaitForExit();
        }
    }
}