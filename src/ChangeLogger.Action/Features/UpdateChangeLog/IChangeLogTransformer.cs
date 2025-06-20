using ChangeLogger.Action.Shared;

namespace ChangeLogger.Action.Features.UpdateChangeLog;

public interface IChangeLogTransformer
{
    string Update(string logContent, Inputs inputs);
}