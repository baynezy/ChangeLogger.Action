using ChangeLogger.Action.Features.UpdateChangeLog;
using ChangeLogger.Action.Shared.Configuration;
using ChangeLogger.Action.Shared.FileSystem;

namespace ChangeLogger.Action.Shared;

public class InputGenerator(IGitHubSettings gitHubSettings, IGitClient gitClient) : IInputGenerator
{
    public Inputs CreateInputs(UpdateChangeLogOptions opts)
    {
        var tag = opts.Tag;
        var date = DateOnly.FromDateTime(DateTime.UtcNow);
        var genesisHash = gitClient.GetGenesisHash(opts.RepositoryPath);
        var (ownerName, repositoryName) = ExtractNames(gitHubSettings.RepositoryName);
        var owner = new Owner(ownerName);
        var repository = new Repository(repositoryName, owner, genesisHash);
        
        return new Inputs(tag, date, repository, opts.LogPath);
    }

    private static (string ownerName, string repositoryName) ExtractNames(string repositoryName)
    {
        var split = repositoryName.Split('/');
        
        if (split.Length != 2)
        {
            throw new ArgumentException("Repository name must be in the format 'owner/repository'.", nameof(repositoryName));
        }
        
        return (split[0], split[1]);
    }
}