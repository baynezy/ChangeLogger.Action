using ChangeLogger.Action.Features.UpdateChangeLog;

namespace ChangeLogger.Action.Shared;

public interface IInputGenerator
{
    Inputs CreateInputs(UpdateChangeLogOptions opts);
}