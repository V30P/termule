using System.Reflection;

namespace Termule;

static class Bootstrapper
{
    static void Main(string[] args)
    {
        Assembly project = args.Length == 0 ? LoadEmbeddedProject() : Assembly.LoadFrom(args[0]);
        MethodInfo projectStartMethod = GetStartMethod(project);

        try
        {
            projectStartMethod.Invoke(null, [args]);
            AppDomain.CurrentDomain.ProcessExit += (_, _) => Game.Stop();
            Game.Run();
        }
        catch 
        {
            Game.Stop();
            throw;
        }
    }

    static Assembly LoadEmbeddedProject()
    {
        using FileStream exeStream = new FileStream(Environment.ProcessPath, FileMode.Open, FileAccess.Read);

        // Get the embedded project's length
        const int sizeBytesLength = 4;
        byte[] projectSizeBytes = new byte[sizeBytesLength];
        exeStream.Seek(-sizeBytesLength, SeekOrigin.End);
        exeStream.ReadExactly(projectSizeBytes, 0, sizeBytesLength);
        int projectlength = BitConverter.ToInt32(projectSizeBytes);

        // Load the embedded project
        byte[] projectBytes = new byte[projectlength];
        exeStream.Seek(-(projectlength + sizeBytesLength), SeekOrigin.End);
        exeStream.ReadExactly(projectBytes, 0, projectlength);

        return Assembly.Load(projectBytes);
    }

    static MethodInfo GetStartMethod(Assembly assembly) =>
    assembly.GetTypes()
    .SelectMany(type => type.GetMethods())
    .Where
    (
        method => method.Name == "Start"
        && method.GetParameters() is [{ ParameterType: Type paramType }] && paramType == typeof(string[])
    )
    .FirstOrDefault();
}