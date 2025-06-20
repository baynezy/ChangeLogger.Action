namespace ChangeLogger.Action.Shared.FileSystem;

public interface IGitClient
{
    string GetGenesisHash(string path);
}