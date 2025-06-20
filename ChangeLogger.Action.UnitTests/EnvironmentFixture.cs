using ChangeLogger.Action.Shared.Configuration;
using JetBrains.Annotations;

namespace ChangeLogger.Action.UnitTests;

[UsedImplicitly]
public sealed class EnvironmentFixture : IDisposable
{
    public void SetEnvironmentVariables()
    {
        Environment.SetEnvironmentVariable(GitHubSettings.GitHubRepositoryNameKey, "ChangeLogger/ChangeLogger.Action");
    }
    
    public void Dispose()
    {
        ClearEnvironmentVariables();
    }

    public void ClearEnvironmentVariables()
    {
        Environment.SetEnvironmentVariable(GitHubSettings.GitHubRepositoryNameKey, null);
    }
}