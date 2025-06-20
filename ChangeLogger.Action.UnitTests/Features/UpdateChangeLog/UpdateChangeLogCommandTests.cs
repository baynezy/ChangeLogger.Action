using AutoFixture.Xunit2;
using ChangeLogger.Action.Features.UpdateChangeLog;
using ChangeLogger.Action.Shared;
using Microsoft.Extensions.Logging;
using NSubstitute;

namespace ChangeLogger.Action.UnitTests.Features.UpdateChangeLog;

public class UpdateChangeLogCommandTests
{
    private readonly UpdateChangeLogCommand _sut;

    private readonly IFileReader _mockFileReader = Substitute.For<IFileReader>();
    private readonly IChangeLogTransformer _mockTransformer = Substitute.For<IChangeLogTransformer>();
    private readonly IInputGenerator _mockInputGenerator = Substitute.For<IInputGenerator>();
    private readonly ILogger<UpdateChangeLogCommand> _logger = new StubLogger<UpdateChangeLogCommand>();

    public UpdateChangeLogCommandTests()
    {
        _sut = new UpdateChangeLogCommand(
            _mockFileReader,
            _mockTransformer,
            _mockInputGenerator,
            _logger);
    }

    [Theory]
    [AutoData]
    public void Update_ChangeLogDoesNotExist_ThenReturnsErrorCode(UpdateChangeLogOptions options)
    {
        // arrange
        _mockFileReader.Exists(options.LogPath)
            .Returns(false);

        // act
        var result = _sut.Update(options);

        // assert
        result.Should()
            .Be(1);
        _mockFileReader.Received(1)
            .Exists(options.LogPath);
    }

    [Theory]
    [AutoData]
    public void Update_ChangeLogDoesExist_ThenReturnsSuccessCode(
        UpdateChangeLogOptions options,
        string changeLogContent,
        string updatedContent,
        Inputs inputs)
    {
        // arrange
        _mockFileReader.Exists(options.LogPath)
            .Returns(true);
        _mockFileReader.ReadAllText(options.LogPath)
            .Returns(changeLogContent);
        _mockInputGenerator.CreateInputs(options)
            .Returns(inputs);
        _mockTransformer.Update(changeLogContent, inputs)
            .Returns(updatedContent);

        // act
        var result = _sut.Update(options);

        // assert
        result.Should()
            .Be(0);
        _mockFileReader.Received(1)
            .WriteAllText(options.LogPath, updatedContent);
    }

    [Theory]
    [InlineData("1.0.0")]
    [InlineData("2.5.3")]
    public void Update_WhenUsingDefaultValues_ThenUsesDefaultLogPathAndRepoPath(string tag)
    {
        // arrange
        var options = new UpdateChangeLogOptions { Tag = tag };
        var changeLogContent = "# Changelog\n\n## [Unreleased]\n- Some changes";
        var updatedContent = "# Changelog\n\n## [1.0.0] - 2023-01-01\n- Some changes";
        var inputs = new Inputs(tag, DateOnly.FromDateTime(DateTime.UtcNow), 
            new Repository("TestRepo", new Owner("TestOwner"), "hash123"), 
            "./CHANGELOG.md");
        
        _mockFileReader.Exists("./CHANGELOG.md")
            .Returns(true);
        _mockFileReader.ReadAllText("./CHANGELOG.md")
            .Returns(changeLogContent);
        _mockInputGenerator.CreateInputs(options)
            .Returns(inputs);
        _mockTransformer.Update(changeLogContent, inputs)
            .Returns(updatedContent);

        // act
        var result = _sut.Update(options);

        // assert
        result.Should()
            .Be(0);
        options.LogPath.Should()
            .Be("./CHANGELOG.md");
        options.RepositoryPath.Should()
            .Be("/");
        _mockFileReader.Received(1)
            .Exists("./CHANGELOG.md");
        _mockFileReader.Received(1)
            .ReadAllText("./CHANGELOG.md");
        _mockFileReader.Received(1)
            .WriteAllText("./CHANGELOG.md", updatedContent);
    }
}