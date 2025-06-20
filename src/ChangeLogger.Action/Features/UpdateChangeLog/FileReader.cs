namespace ChangeLogger.Action.Features.UpdateChangeLog;

public class FileReader : IFileReader
{
    public bool Exists(string path) => File.Exists(path);

    public string ReadAllText(string path) => File.ReadAllText(path);

    public void WriteAllText(string path, string content) => File.WriteAllText(path, content);
}