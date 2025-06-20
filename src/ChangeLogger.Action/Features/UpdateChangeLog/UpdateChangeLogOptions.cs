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
        Required = true,
        HelpText = "The path to the CHANGELOG.md file, e.g., /path/to/repo/CHANGELOG.md.")]
    public required string LogPath { get; [UsedImplicitly] init; }
    
    [Option('p', "path",
        Required = true,
        HelpText = "The path to the root of the repository, e.g., /path/to/repo.")]
    public required string RepositoryPath { get; [UsedImplicitly] init; }
}