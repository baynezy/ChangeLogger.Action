using ChangeLogger.Action.Features.UpdateChangeLog;
using ChangeLogger.Action.Shared;

namespace ChangeLogger.Action.UnitTests.Features.UpdateChangeLog;

public class ChangeLogTransformerTests
{
    private readonly ChangeLogTransformer _sut = new();

    [Theory]
    [MemberData(nameof(LoadExampleChangeLogs))]
    public Task UpdateChangelog_MovesUnreleasedToNewTag(string fileName, string original)
    {
        // arrange
        const string tag = "1.0.0";
        var inputs = new Inputs(tag, DateOnly.FromDateTime(DateTime.UtcNow),
            new Repository("RepoName", new Owner("OwnerName"), "GenesisHash"), "CHANGELOG.md");

        // act
        var result = _sut.Update(original, inputs);

        // assert
        return Verify(result)
            .ScrubInlineDates("yyyy-MM-dd")
            .UseParameters(fileName);
    }

    [Fact]
    public void UpdateChangelog_ThrowsIfNoUnreleasedSection()
    {
        // arrange
        const string original = "# Changelog\n\n## [1.0.0] - 2025-06-17\n";
        const string tag = "1.0.1";
        var inputs = new Inputs(tag, DateOnly.FromDateTime(DateTime.UtcNow),
            new Repository("RepoName", new Owner("OwnerName"), "GenesisHash"), "CHANGELOG.md");

        // act
        var action = () => _sut.Update(original, inputs);

        // assert
        action.Should()
            .Throw<InvalidChangeLogException>()
            .WithMessage("[Unreleased] section not found.");
    }

    public static TheoryData<string, string> LoadExampleChangeLogs()
    {
        // get the contents of all the files in the ChangeLogger.Action.UnitTests/Features/UpdateChangeLog/Examples directory
        var data = new TheoryData<string, string>();
        var directory =
            Path.GetFullPath(Path.GetDirectoryName("../../../Features/UpdateChangeLog/Example-Changelogs/")!);
        foreach (var file in Directory.GetFiles(directory, "*.md"))
        {
            var fileName = Path.GetFileName(file);
            var content = File.ReadAllText(file);
            data.Add(fileName, content);
        }

        return data;
    }
}