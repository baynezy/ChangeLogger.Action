using ChangeLogger.Action.Features.UpdateChangeLog;
using ChangeLogger.Action.Shared;

namespace ChangeLogger.Action.UnitTests.Features.UpdateChangeLog;

public class ChangeLogTransformerExtractUnreleasedContentTests
{
    private readonly ChangeLogTransformer _sut = new();

    [Fact]
    public void ExtractUnreleasedContent_WithValidChangelog_ReturnsUnreleasedContent()
    {
        // arrange
        const string changelog = """
            # Changelog

            ## [Unreleased]

            ### Changed

            - Made log-path and repo-path parameters optional with default values (#36)
              - log-path defaults to './CHANGELOG.md'
              - repo-path defaults to '/'

            ### Added

            - Environment instructions for Copilot (#39)

            ## [1.0.0.3] - 2025-06-20

            ### Changed

            - Update README instructions
            """;

        // act
        var result = _sut.ExtractUnreleasedContent(changelog);

        // assert
        result.Should().Be("""
            ### Changed

            - Made log-path and repo-path parameters optional with default values (#36)
              - log-path defaults to './CHANGELOG.md'
              - repo-path defaults to '/'

            ### Added

            - Environment instructions for Copilot (#39)
            """);
    }

    [Fact]
    public void ExtractUnreleasedContent_WithEmptyUnreleasedSection_ReturnsEmptyContent()
    {
        // arrange
        const string changelog = """
            # Changelog

            ## [Unreleased]

            ## [1.0.0.3] - 2025-06-20

            ### Changed

            - Update README instructions
            """;

        // act
        var result = _sut.ExtractUnreleasedContent(changelog);

        // assert
        result.Should().Be("");
    }

    [Fact]
    public void ExtractUnreleasedContent_WithOnlyUnreleasedSection_ReturnsAllContent()
    {
        // arrange
        const string changelog = """
            # Changelog

            ## [Unreleased]

            ### Added

            - New feature
            - Another feature
            """;

        // act
        var result = _sut.ExtractUnreleasedContent(changelog);

        // assert
        result.Should().Be("""
            ### Added

            - New feature
            - Another feature
            """);
    }

    [Fact]
    public void ExtractUnreleasedContent_WithoutUnreleasedSection_ThrowsException()
    {
        // arrange
        const string changelog = """
            # Changelog

            ## [1.0.0.3] - 2025-06-20

            ### Changed

            - Update README instructions
            """;

        // act
        var action = () => _sut.ExtractUnreleasedContent(changelog);

        // assert
        action.Should()
            .Throw<InvalidChangeLogException>()
            .WithMessage("[Unreleased] section not found.");
    }
}