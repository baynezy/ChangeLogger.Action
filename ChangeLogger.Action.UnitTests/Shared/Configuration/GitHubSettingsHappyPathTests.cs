using ChangeLogger.Action.Shared.Configuration;
using Xunit.OpenCategories;

namespace ChangeLogger.Action.UnitTests.Shared.Configuration;

[Collection("EnvironmentCollection")]
[LocalTest] // this is because these environment variables exist in GitHub Actions, but not locally
public sealed class GitHubSettingsHappyPathTests : IDisposable
{
    private readonly EnvironmentFixture _environmentFixture;
    private const string ExpectedRepoName = "ChangeLogger/ChangeLogger.Action";

    public GitHubSettingsHappyPathTests(EnvironmentFixture environmentFixture)
    {
        _environmentFixture = environmentFixture;
        _environmentFixture.SetEnvironmentVariables();
    }

    [Fact]
    public void RepositoryName_WhenEnvironmentIsSet_ReturnsExpectedValue()
    {
        // arrange
        var settings = new GitHubSettings();

        // act
        var repositoryName = settings.RepositoryName;

        // assert
        repositoryName.Should()
            .Be(ExpectedRepoName);
    }

    public void Dispose()
    {
        _environmentFixture.ClearEnvironmentVariables();
    }
}