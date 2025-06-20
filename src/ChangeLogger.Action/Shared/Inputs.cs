namespace ChangeLogger.Action.Shared;

public record Inputs (string Tag, DateOnly Date, Repository Repository, string ChangeLogPath);