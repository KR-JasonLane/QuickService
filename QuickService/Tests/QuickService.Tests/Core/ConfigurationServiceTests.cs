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
            s => s.SaveJsonProperties(config, It.Is<string>(p => p.EndsWith("Configuration.json"))),
            Times.Once);
    }

    [Fact]
    public void SaveConfiguration_NullObject_DoesNotCallJsonService()
    {
        // Act
        _sut.SaveConfiguration<TestConfig>(null!);

        // Assert
        _jsonFileServiceMock.Verify(
            s => s.SaveJsonProperties(It.IsAny<TestConfig>(), It.IsAny<string>()),
            Times.Never);
    }

    #endregion

    #region GetConfiguration Tests

    [Fact]
    public void GetConfiguration_DelegatesToJsonFileService()
    {
        // Arrange
        var expected = new TestConfig { Setting = "loaded" };
        _jsonFileServiceMock
            .Setup(s => s.GetJsonProperties<TestConfig>(It.IsAny<string>()))
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
            .Setup(s => s.GetJsonProperties<TestConfig>(It.IsAny<string>()))
            .Returns(new TestConfig());

        // Act
        _sut.GetConfiguration<TestConfig>();

        // Assert
        _jsonFileServiceMock.Verify(
            s => s.GetJsonProperties<TestConfig>(It.Is<string>(p =>
                p.Contains("Config") && p.EndsWith("Configuration.json"))),
            Times.Once);
    }

    #endregion

    private class TestConfig
    {
        public string? Setting { get; set; }
    }
}
