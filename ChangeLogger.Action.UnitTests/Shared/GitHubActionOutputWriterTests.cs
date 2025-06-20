using ChangeLogger.Action.Shared;

namespace ChangeLogger.Action.UnitTests.Shared;

public class GitHubActionOutputWriterTests
{
    [Fact]
    public void WriteReleaseNotes_WhenGitHubOutputNotSet_DoesNothing()
    {
        // arrange
        Environment.SetEnvironmentVariable("GITHUB_OUTPUT", null);
        var sut = new GitHubActionOutputWriter();

        // act
        var action = () => sut.WriteReleaseNotes("test content");

        // assert
        action.Should().NotThrow();
    }

    [Fact]
    public void WriteReleaseNotes_WhenGitHubOutputIsEmpty_DoesNothing()
    {
        // arrange
        Environment.SetEnvironmentVariable("GITHUB_OUTPUT", "");
        var sut = new GitHubActionOutputWriter();

        // act
        var action = () => sut.WriteReleaseNotes("test content");

        // assert
        action.Should().NotThrow();
    }

    [Fact]
    public void WriteReleaseNotes_WhenGitHubOutputIsSet_WritesToFile()
    {
        // arrange
        var tempFile = Path.GetTempFileName();
        try
        {
            Environment.SetEnvironmentVariable("GITHUB_OUTPUT", tempFile);
            var sut = new GitHubActionOutputWriter();
            const string releaseNotes = """
                ### Added
                - New feature
                - Another feature
                """;

            // act
            sut.WriteReleaseNotes(releaseNotes);

            // assert
            var content = File.ReadAllText(tempFile);
            content.Should().Be($"release-notes={releaseNotes}\n");
        }
        finally
        {
            File.Delete(tempFile);
            Environment.SetEnvironmentVariable("GITHUB_OUTPUT", null);
        }
    }
}