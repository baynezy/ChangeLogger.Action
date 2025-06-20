namespace ChangeLogger.Action.Features.UpdateChangeLog;

public interface IFileReader
{
    bool Exists(string path);
    string ReadAllText(string path);
    void WriteAllText(string path, string content);
}