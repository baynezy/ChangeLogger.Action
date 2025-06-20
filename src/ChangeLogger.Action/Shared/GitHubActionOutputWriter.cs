using System.Text;

namespace ChangeLogger.Action.Shared;

public interface IGitHubActionOutputWriter
{
    void WriteReleaseNotes(string releaseNotes);
}

public class GitHubActionOutputWriter : IGitHubActionOutputWriter
{
    private const string GitHubOutput = "GITHUB_OUTPUT";

    public void WriteReleaseNotes(string releaseNotes)
    {
        var gitHubOutputFile = Environment.GetEnvironmentVariable(GitHubOutput);
        if (string.IsNullOrEmpty(gitHubOutputFile)) return;

        using var textWriter = new StreamWriter(gitHubOutputFile, append: true, Encoding.UTF8);
        WriteVariableToGitHubAction(textWriter, "release-notes", releaseNotes);
    }

    private static void WriteVariableToGitHubAction(TextWriter textWriter, string name, string value)
    {
        // Handle multi-line values by using GitHub's heredoc syntax for complex values
        if (value.Contains('\n'))
        {
            var delimiter = "EOF";
            textWriter.WriteLine($"{name}<<{delimiter}");
            textWriter.WriteLine(value);
            textWriter.WriteLine(delimiter);
        }
        else
        {
            var output = $"{name}={value}";
            textWriter.WriteLine(output);
        }
    }
}