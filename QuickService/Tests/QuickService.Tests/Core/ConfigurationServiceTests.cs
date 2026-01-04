using Moq;
using QuickService.Abstract.Interfaces;
using QuickService.Core.Services;

namespace QuickService.Tests.Core;

public class ConfigurationServiceTests
{
    private readonly Mock<IJsonFileService> _jsonFileServiceMock;
    private readonly ConfigurationService _sut;

    public ConfigurationServiceTests()
    {
        _jsonFileServiceMock = new Mock<IJsonFileService>();
        _sut = new ConfigurationService(_jsonFileServiceMock.Object);
    }

    #region Constructor Tests

    [Fact]
    public void Constructor_NullService_ThrowsArgumentNullException()
    {
        var act = () => new ConfigurationService(null!);
        act.Should().Throw<ArgumentNullException>();
    }

    #endregion

    #region SaveConfiguration Tests

    [Fact]
    public void SaveConfiguration_ValidObject_DelegatesToJsonFileService()
    {
        // Arrange
        var config = new TestConfig { Setting = "value" };

        // Act
        _sut.SaveConfiguration(config);

        // Assert
        _jsonFileServiceMock.Verify(
            s => s.Save(config, It.Is<string>(p => p.EndsWith("Configuration.json"))),
            Times.Once);
    }

    [Fact]
    public void SaveConfiguration_NullObject_ThrowsArgumentNullException()
    {
        // Act
        var act = () => _sut.SaveConfiguration<TestConfig>(null!);

        // Assert
        act.Should().Throw<ArgumentNullException>();
    }

    #endregion

    #region GetConfiguration Tests

    [Fact]
    public void GetConfiguration_DelegatesToJsonFileService()
    {
        // Arrange
        var expected = new TestConfig { Setting = "loaded" };
        _jsonFileServiceMock
            .Setup(s => s.Load<TestConfig>(It.IsAny<string>()))
            .Returns(expected);

        // Act
        var result = _sut.GetConfiguration<TestConfig>();

        // Assert
        result.Should().BeSameAs(expected);
    }

    [Fact]
    public void GetConfiguration_UsesConfigFilePath()
    {
        // Arrange
        _jsonFileServiceMock
            .Setup(s => s.Load<TestConfig>(It.IsAny<string>()))
            .Returns(new TestConfig());

        // Act
        _sut.GetConfiguration<TestConfig>();

        // Assert
        _jsonFileServiceMock.Verify(
            s => s.Load<TestConfig>(It.Is<string>(p =>
                p.Contains("Config") && p.EndsWith("Configuration.json"))),
            Times.Once);
    }

    #endregion

    private class TestConfig
    {
        public string? Setting { get; set; }
    }
}
