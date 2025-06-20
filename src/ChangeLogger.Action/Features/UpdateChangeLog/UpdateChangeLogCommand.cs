using ChangeLogger.Action.Shared;
using Microsoft.Extensions.Logging;

namespace ChangeLogger.Action.Features.UpdateChangeLog;

public class UpdateChangeLogCommand(
    IFileReader fileReader,
    IChangeLogTransformer transformer,
    IInputGenerator inputGenerator,
    ILogger<UpdateChangeLogCommand> logger)
{
    public int Update(UpdateChangeLogOptions opts)
    {
        var changelogFile = opts.LogPath;
        var inputs = inputGenerator.CreateInputs(opts);
        if (!fileReader.Exists(changelogFile))
        {
            logger.LogError("{LogFile} not found", changelogFile);
            return 1;
        }

        var changelog = fileReader.ReadAllText(changelogFile);
        var updated = transformer.Update(changelog, inputs);
        fileReader.WriteAllText(changelogFile, updated);
        logger.LogInformation("{LogFile} updated for tag {Tag}", changelogFile, inputs.Tag);
        return 0;
    }
}