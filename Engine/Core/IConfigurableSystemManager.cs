namespace Termule.Core;

/// <summary>
///     Provides configuration operations for installing, uninstalling and retrieving <see cref="System" />s.
/// </summary>
public interface IConfigurableSystemManager
{
    /// <summary>
    ///     Installs the given <paramref name="system" />, replacing any previously installed implementation of the same
    ///     system.
    /// </summary>
    /// <param name="system">The system instance to install.</param>
    /// <typeparam name="TSystem">The type of the system to install.</typeparam>
    void Install<TSystem>(TSystem system)
        where TSystem : System;

    /// <summary>
    ///     Uninstalls a previously installed system of the specified type, if present.
    /// </summary>
    /// <typeparam name="TSystem">The type of system to uninstall.</typeparam>
    void Uninstall<TSystem>()
        where TSystem : System;

    /// <summary>
    ///     Gets the installed <see cref="System" /> of the specified type, or <c>null</c> if none is installed.
    /// </summary>
    /// <typeparam name="TSystem">The type of system to look for.</typeparam>
    /// <returns>The installed system or <c>null</c>.</returns>
    TSystem Get<TSystem>()
        where TSystem : System;

    /// <summary>
    ///     Installs the default set of systems appropriate for the current operating system.
    /// </summary>
    void UseDefaults();
}