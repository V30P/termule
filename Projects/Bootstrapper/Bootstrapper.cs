using System.Reflection;
using Termule;

// If a dll path is passed load that, otherwise load the embedded game
Assembly game;
if (args.Length != 0)
{
    game = Assembly.LoadFrom(args[0]);
}
else
{
    using FileStream selfStream = new(Environment.ProcessPath, FileMode.Open, FileAccess.Read);

    // Get the game's length
    const int sizeBytesLength = 4;
    byte[] gameSizeBytes = new byte[sizeBytesLength];
    selfStream.Seek(-sizeBytesLength, SeekOrigin.End);
    selfStream.ReadExactly(gameSizeBytes, 0, sizeBytesLength);
    int projectlength = BitConverter.ToInt32(gameSizeBytes);

    // Load the game assembly
    byte[] gameBytes = new byte[projectlength];
    selfStream.Seek(-(projectlength + sizeBytesLength), SeekOrigin.End);
    selfStream.ReadExactly(gameBytes, 0, projectlength);
    game = Assembly.Load(gameBytes);
}

// Run the game
MethodInfo entryMethod = game
    .GetTypes()
    .SelectMany(type => type.GetMethods())
    .FirstOrDefault(method => method.GetCustomAttribute<EntryAttribute>() != null);

try
{
    entryMethod.Invoke(null, [args]);

    AppDomain.CurrentDomain.ProcessExit += (_, _) => Game.Stop();
    Console.CancelKeyPress += (_, _) => Game.Stop();

    Game.Run();
}
catch
{
    Game.Stop();
    throw;
}