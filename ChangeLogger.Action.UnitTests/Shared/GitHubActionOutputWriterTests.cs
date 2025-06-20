using ChangeLogger.Action.Shared;

namespace ChangeLogger.Action.UnitTests.Shared;

public class GitHubActionOutputWriterTests : IDisposable
{
    private readonly GitHubActionOutputWriter sut;

    public GitHubActionOutputWriterTests()
    {
        sut = new GitHubActionOutputWriter();
    }

    [Fact]
    public void WriteReleaseNotes_WhenGitHubOutputNotSet_DoesNothing()
    {
        // arrange
        Environment.SetEnvironmentVariable("GITHUB_OUTPUT", null);

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
            const string releaseNotes = """
                ### Added
                - New feature
                - Another feature
                """;

            // act
            sut.WriteReleaseNotes(releaseNotes);

            // assert
            var content = File.ReadAllText(tempFile);
            content.Should().Be($"release-notes<<EOF\n{releaseNotes}\nEOF\n");
        }
        finally
        {
            File.Delete(tempFile);
        }
    }

    [Fact]
    public void WriteReleaseNotes_WithSingleLineValue_UsesSimpleFormat()
    {
        // arrange
        var tempFile = Path.GetTempFileName();
        try
        {
            Environment.SetEnvironmentVariable("GITHUB_OUTPUT", tempFile);
            const string releaseNotes = "Simple single line";

            // act
            sut.WriteReleaseNotes(releaseNotes);

            // assert
            var content = File.ReadAllText(tempFile);
            content.Should().Be($"release-notes={releaseNotes}\n");
        }
        finally
        {
            File.Delete(tempFile);
        }
    }

    public void Dispose()
    {
        Environment.SetEnvironmentVariable("GITHUB_OUTPUT", null);
    }
}