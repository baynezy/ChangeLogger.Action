using CommandLine;
using JetBrains.Annotations;

namespace ChangeLogger.Action.Features.UpdateChangeLog;

[Verb("update", HelpText = "Updates the CHANGELOG.md file for a new release.")]
public class UpdateChangeLogOptions
{
    [Option('t', "tag",
        Required = true,
        HelpText = "The tag for the new release, e.g., 1.0.0.")]
    public required string Tag { get; [UsedImplicitly] init; }
    
    [Option('l', "log-path",
        Required = false,
        Default = "./CHANGELOG.md",
        HelpText = "The path to the CHANGELOG.md file, e.g., /path/to/repo/CHANGELOG.md.")]
    public string LogPath { get; [UsedImplicitly] init; } = "./CHANGELOG.md";
    
    [Option('p', "repo-path",
        Required = false,
        Default = "/",
        HelpText = "The path to the root of the repository, e.g., /path/to/repo.")]
    public string RepositoryPath { get; [UsedImplicitly] init; } = "/";
}