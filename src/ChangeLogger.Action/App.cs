using ChangeLogger.Action.Features.UpdateChangeLog;
using CommandLine;
using Microsoft.Extensions.Configuration;

namespace ChangeLogger.Action;

public class App
{
    private readonly UpdateChangeLogCommand _updateChangeLogCommand;

    public App(IConfiguration configuration, UpdateChangeLogCommand updateChangeLogCommand)
    {
        _updateChangeLogCommand = updateChangeLogCommand;
    }

    public int Run(IEnumerable<string> args)
    {
        return Parser.Default.ParseArguments<UpdateChangeLogOptions>(args)
            .MapResult(
                (UpdateChangeLogOptions opts) => _updateChangeLogCommand.Update(opts),
                _ => 1
            );
    }
}