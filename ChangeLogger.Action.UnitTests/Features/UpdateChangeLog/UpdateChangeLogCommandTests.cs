using AutoFixture.Xunit2;
using ChangeLogger.Action.Features.UpdateChangeLog;
using ChangeLogger.Action.Shared;
using CommandLine;
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

    [Fact]
    public void UpdateChangeLogOptions_WhenDefaultValuesUsed_ThenShouldHaveCorrectDefaults()
    {
        // arrange & act
        var options = new UpdateChangeLogOptions
        {
            Tag = "1.0.0"
        };

        // assert
        options.LogPath.Should()
            .Be("./CHANGELOG.md");
        options.RepositoryPath.Should()
            .Be("/");
    }

    [Fact]
    public void UpdateChangeLogOptions_WhenParsedWithOnlyTag_ThenShouldUseDefaults()
    {
        // arrange
        var args = new[] { "update", "--tag", "1.0.0" };

        // act
        var parseResult = Parser.Default.ParseArguments<UpdateChangeLogOptions>(args);
        UpdateChangeLogOptions? options = null;
        parseResult.WithParsed(opts => options = opts);

        // assert
        options.Should().NotBeNull();
        options!.Tag.Should().Be("1.0.0");
        options.LogPath.Should().Be("./CHANGELOG.md");
        options.RepositoryPath.Should().Be("/");
    }

    [Fact]
    public void UpdateChangeLogOptions_WhenExplicitValuesProvided_ThenShouldUseProvidedValues()
    {
        // arrange
        var args = new[] { "update", "--tag", "2.0.0", "--log-path", "/custom/CHANGELOG.md", "--repo-path", "/custom/repo" };

        // act
        var parseResult = Parser.Default.ParseArguments<UpdateChangeLogOptions>(args);
        UpdateChangeLogOptions? options = null;
        parseResult.WithParsed(opts => options = opts);

        // assert
        options.Should().NotBeNull();
        options!.Tag.Should().Be("2.0.0");
        options.LogPath.Should().Be("/custom/CHANGELOG.md");
        options.RepositoryPath.Should().Be("/custom/repo");
    }
}