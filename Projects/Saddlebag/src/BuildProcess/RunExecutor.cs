using System.Diagnostics;

namespace Termule.Saddlebag;

[Executor("run")]
internal class RunExecutor
{
    internal RunExecutor()
    {
        BuildExecutor buildExecutor = new BuildExecutor();
        if (buildExecutor.succeeded)
        {
            Process process = Process.Start(buildExecutor.outputPath);

            // Wait until the process exits in order to avoid the console prompt reopening
            while (!process.HasExited) Thread.Sleep(1);
        }
    }
}