using System.IO;
using QuickService.Core.Services;

namespace QuickService.Tests.Core;

public class LaunchAppServiceTests
{
    private readonly LaunchAppService _sut;

    public LaunchAppServiceTests()
    {
        _sut = new LaunchAppService();
    }

    [Fact]
    public void LaunchApp_NullPath_ThrowsArgumentException()
    {
        // Act
        var act = () => _sut.LaunchApp(null!);

        // Assert
        act.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void LaunchApp_EmptyPath_ThrowsArgumentException()
    {
        // Act
        var act = () => _sut.LaunchApp(string.Empty);

        // Assert
        act.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void LaunchApp_NonExistentPath_ThrowsFileNotFoundException()
    {
        // Act
        var act = () => _sut.LaunchApp(@"C:\nonexistent\fake_app.exe");

        // Assert
        act.Should().Throw<FileNotFoundException>();
    }
}
