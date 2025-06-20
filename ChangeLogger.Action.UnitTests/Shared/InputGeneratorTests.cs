using ChangeLogger.Action.Features.UpdateChangeLog;
using ChangeLogger.Action.Shared;
using ChangeLogger.Action.Shared.Configuration;
using ChangeLogger.Action.Shared.FileSystem;
using NSubstitute;

namespace ChangeLogger.Action.UnitTests.Shared;

public class InputGeneratorTests
{
    private readonly IGitHubSettings _mockGitHubSettings = Substitute.For<IGitHubSettings>();
    private readonly IGitClient _mockGitClient = Substitute.For<IGitClient>();
    private readonly InputGenerator _sut;
    private readonly Faker _faker = new();

    public InputGeneratorTests()
    {
        _sut = new InputGenerator(_mockGitHubSettings, _mockGitClient);

        _mockGitHubSettings.RepositoryName.Returns("ChangeLogger/ChangeLogger.Action");
    }

    [Fact]
    public void CreateInputs_WhenTagIsSetCorrectly_ThenTagShouldSetInInputsCorrectly()
    {
        // arrange
        var expectedTag = GenerateRandomTag();
        var options = new UpdateChangeLogOptions
        {
            Tag = expectedTag,
            RepositoryPath = "/",
            LogPath = "/CHANGELOG.md"
        };


        // act
        var inputs = _sut.CreateInputs(options);

        // assert
        inputs.Tag.Should()
            .Be(expectedTag);
    }

    [Fact]
    public void CreateInputs_WhenInputsCreated_ThenDateShouldBeSetToToday()
    {
        // arrange
        var options = new UpdateChangeLogOptions
        {
            Tag = GenerateRandomTag(),
            RepositoryPath = "/",
            LogPath = "/CHANGELOG.md"
        };

        // act
        var inputs = _sut.CreateInputs(options);

        // assert
        inputs.Date.Should()
            .Be(DateOnly.FromDateTime(DateTime.UtcNow));
    }

    [Fact]
    public void CreateInputs_WhenChangeLogPathSetCorrectly_ThenChangeLogPathShouldBeSetInInputs()
    {
        // arrange
        const string expectedChangeLogPath = "/CHANGELOG.md";
        var options = new UpdateChangeLogOptions
        {
            Tag = GenerateRandomTag(),
            RepositoryPath = "/",
            LogPath = expectedChangeLogPath
        };

        // act
        var inputs = _sut.CreateInputs(options);

        // assert
        inputs.ChangeLogPath.Should()
            .Be(expectedChangeLogPath);
    }

    [Theory]
    [MemberData(nameof(RepositoryNameData))]
    public void CreateInputs_WhenRepositoryUrlSetCorrectly_ThenOwnerShouldBeSetCorrectly(
        string repositoryUrl,
        string expectedOwnerName, 
        string _)
    {
        // arrange
        _mockGitHubSettings.RepositoryName.Returns(repositoryUrl);
        var options = new UpdateChangeLogOptions
        {
            Tag = GenerateRandomTag(),
            RepositoryPath = "/",
            LogPath = "/CHANGELOG.md"
        };

        // act
        var inputs = _sut.CreateInputs(options);

        // assert
        inputs.Repository.Owner.Name.Should()
            .Be(expectedOwnerName);
    }

    [Theory]
    [MemberData(nameof(RepositoryNameData))]
    public void CreateInputs_WhenRepositoryUrlSetCorrectly_ThenRepositoryNameShouldBeSetCorrectly(
        string repositoryUrl,
        string _, 
        string expectedRepositoryName)
    {
        // arrange
        _mockGitHubSettings.RepositoryName.Returns(repositoryUrl);
        var options = new UpdateChangeLogOptions
        {
            Tag = GenerateRandomTag(),
            RepositoryPath = "/",
            LogPath = "/CHANGELOG.md"
        };

        // act
        var inputs = _sut.CreateInputs(options);

        // assert
        inputs.Repository.Name.Should()
            .Be(expectedRepositoryName);
    }
    
    [Theory]
    [InlineData("MissingForwardSlash")]
    [InlineData("Too/Many/Slashes/In/Repository/Name")]
    [InlineData("!$%£^$%£^$%$!¬$%YGFVS^%£YERGSDV WASDZXGHC&Y")]
    [InlineData("")]
    public void CreateInputs_WhenRepositoryUrlSetIncorrectly_ThenThrowsArgumentException(
        string repositoryUrl)
    {
        // arrange
        _mockGitHubSettings.RepositoryName.Returns(repositoryUrl);
        var options = new UpdateChangeLogOptions
        {
            Tag = GenerateRandomTag(),
            RepositoryPath = "/",
            LogPath = "/CHANGELOG.md"
        };

        // act
        var act = () => _sut.CreateInputs(options);

        // assert
        act.Should()
            .Throw<ArgumentException>()
            .Where(e => e.Message.StartsWith("Repository name must be in the format 'owner/repository'."));
    }

    public static TheoryData<string, string, string> RepositoryNameData =>
        new()
        {
            {"ChangeLogger/ChangeLogger.Action", "ChangeLogger", "ChangeLogger.Action"},
            {"AnotherOwner/AnotherRepository", "AnotherOwner", "AnotherRepository"},
            {"TestOwner/TestRepo", "TestOwner", "TestRepo"}
        };


    private string GenerateRandomTag()
    {
        var major = _faker.Random.Int(0, 10);
        var minor = _faker.Random.Int(0, 10);
        var patch = _faker.Random.Int(0, 10);
        var expectedTag = $"{major}.{minor}.{patch}";
        return expectedTag;
    }
}