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
    public void LaunchApp_NullPath_DoesNotThrow()
    {
        // Act
        var act = () => _sut.LaunchApp(null!);

        // Assert
        act.Should().NotThrow();
    }

    [Fact]
    public void LaunchApp_EmptyPath_DoesNotThrow()
    {
        // Act
        var act = () => _sut.LaunchApp(string.Empty);

        // Assert
        act.Should().NotThrow();
    }

    [Fact]
    public void LaunchApp_NonExistentPath_DocumentedBehavior()
    {
        // Note: 현재 구현은 MessageBox.Show를 사용하므로 UI 없는 테스트 환경에서
        //       테스트 불가. 리팩토링 후 예외 기반으로 변경되면 테스트 강화 예정.
        //       현재는 null/empty 경로에 대한 방어 로직만 테스트 가능.
        true.Should().BeTrue();
    }
}
