namespace ChangeLogger.Action.Shared.Configuration;

public interface IGitHubSettings
{
    string RepositoryName { get; }
}

public class GitHubSettings : IGitHubSettings
{
    public const string GitHubRepositoryNameKey = "GITHUB_REPOSITORY";

    public string RepositoryName => Environment.GetEnvironmentVariable(GitHubRepositoryNameKey) ??
                                    throw new InvalidOperationException(
                                        "GITHUB_REPOSITORY environment variable is not set.");
}