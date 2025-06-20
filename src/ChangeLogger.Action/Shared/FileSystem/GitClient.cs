using LibGit2Sharp;

namespace ChangeLogger.Action.Shared.FileSystem;

public class GitClient : IGitClient
{
    public string GetGenesisHash(string path)
    {
        using var repo = new LibGit2Sharp.Repository(path);

        var firstCommit = repo.Commits.QueryBy(new CommitFilter()
            {
                SortBy = CommitSortStrategies.Time | CommitSortStrategies.Reverse
            })
            .FirstOrDefault();

        var sha1 = firstCommit?.Sha;
        
        if (string.IsNullOrEmpty(sha1))
        {
            throw new InvalidOperationException("No commits found in the repository.");
        }
        
        return sha1;
    }
}