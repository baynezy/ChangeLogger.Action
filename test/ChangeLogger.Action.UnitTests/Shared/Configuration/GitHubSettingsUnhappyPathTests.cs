using ChangeLogger.Action.Shared.Configuration;
using Xunit.OpenCategories;

namespace ChangeLogger.Action.UnitTests.Shared.Configuration;

[Collection("EnvironmentCollection")]
[LocalTest] // this is because these environment variables exist in GitHub Actions, but not locally
public class GitHubSettingsUnhappyPathTests
{
    [Fact]
    public void RepositoryName_WhenEnvironmentIsSetNot_InvalidOperationException()
    {
        // arrange
        var settings = new GitHubSettings();

        // act
        var action = () => settings.RepositoryName;

        // assert
        action.Should()
            .Throw<InvalidOperationException>();
    }
}