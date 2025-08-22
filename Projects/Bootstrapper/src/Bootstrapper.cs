using System.Reflection;

namespace Termule;

static class Bootstrapper
{
    static void Main(string[] args)
    {
        Assembly project = LoadEmbeddedProject();
        MethodInfo projectStartMethod = GetStartMethod(project);

        projectStartMethod.Invoke(null, [new Game(), args]);
    }

    static Assembly LoadEmbeddedProject()
    {
        const int sizeBytesLength = 4;
        using FileStream exeStream = new FileStream(Environment.ProcessPath, FileMode.Open, FileAccess.Read);

        // Get the embedded project's length
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

    static MethodInfo GetStartMethod(Assembly assembly)
    {
        foreach (Type type in assembly.GetTypes())
        {
            if
            (
                type.GetMethod("Start", BindingFlags.Public | BindingFlags.Static) is MethodInfo method
                && method.GetParameters() is [{ ParameterType: Type p1Type }, { ParameterType: Type p2Type }]
                && p1Type == typeof(Game) && p2Type == typeof(string[])
            )
            {
                return method;
            }
        }

        return null;
    }
}