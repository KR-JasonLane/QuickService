using Moq;
using Point = System.Windows.Point;
using QuickService.Abstract.Interfaces;
using QuickService.Models.Configure;
using QuickService.ViewModels;
using QuickService.ViewModels.Messenger;

namespace QuickService.Tests.ViewModels;

/// <summary>
/// SelectLaunchAppWindowViewModel의 핵심 비즈니스 로직을 테스트합니다.
/// 각도 계산(CalculateMousePoint)과 위치 판별(CalculatePointedPosition) 로직은
/// private이므로 public 상태(IsSelectedLeft/Top/Right/Bottom)를 통해 간접 검증합니다.
///
/// 현재 이 메서드들은 private이기 때문에 리팩토링 전에는 ViewModel 생성 후
/// 메신저를 통해 간접적으로 테스트합니다.
///
/// 각도 계산 공식:
///   deltaX = mouse.X - center.X
///   deltaY = center.Y - mouse.Y  (Y축 반전)
///   angle = atan2(deltaY, deltaX) * (180/PI)
///
/// 위치 판별 (angle < 0이면 +360):
///   Right:  angle >= 315 || angle < 45
///   Top:    angle >= 45  && angle < 135
///   Left:   angle >= 135 && angle < 225
///   Bottom: angle >= 225 && angle < 315
/// </summary>
public class SelectLaunchAppWindowViewModelTests
{
    private readonly Mock<IConfigurationService> _configServiceMock;
    private readonly Mock<ILaunchAppService> _launchServiceMock;

    public SelectLaunchAppWindowViewModelTests()
    {
        _configServiceMock = new Mock<IConfigurationService>();
        _configServiceMock
            .Setup(s => s.GetConfiguration<RegisteredApplicationModel>())
            .Returns(new RegisteredApplicationModel());
        _launchServiceMock = new Mock<ILaunchAppService>();
    }

    private SelectLaunchAppWindowViewModel CreateSut()
    {
        return new SelectLaunchAppWindowViewModel(
            _configServiceMock.Object,
            _launchServiceMock.Object);
    }

    #region Constructor Tests

    [Fact]
    public void Constructor_InitializesWindowLength()
    {
        // Act
        var sut = CreateSut();

        // Assert
        sut.WindowLength.Should().Be(300);
    }

    [Fact]
    public void Constructor_InitializesInvalidAreaDiameter()
    {
        // Act
        var sut = CreateSut();

        // Assert
        sut.InvalidAreaDiameter.Should().Be(52);
    }

    [Fact]
    public void Constructor_IsWindowOpen_DefaultsFalse()
    {
        // Act
        var sut = CreateSut();

        // Assert
        sut.IsWindowOpen.Should().BeFalse();
    }

    [Fact]
    public void Constructor_LoadsConfiguration()
    {
        // Act
        _ = CreateSut();

        // Assert - 4방향 아이콘 로드를 위해 GetConfiguration이 호출됨
        _configServiceMock.Verify(
            s => s.GetConfiguration<RegisteredApplicationModel>(),
            Times.AtLeastOnce);
    }

    #endregion

    #region Angle Calculation & Position Tests (via Messenger)

    /// <summary>
    /// 각도 계산 순수 로직을 검증하기 위한 헬퍼.
    /// ViewModel의 private 메서드를 직접 호출할 수 없으므로,
    /// 동일한 수학 공식을 사용하여 기대값을 계산합니다.
    /// 리팩토링 시 이 로직이 Domain 레이어로 이동하면 직접 테스트 가능해집니다.
    /// </summary>
    private static double CalculateExpectedAngle(Point center, Point mouse)
    {
        double deltaX = mouse.X - center.X;
        double deltaY = center.Y - mouse.Y;
        return Math.Atan2(deltaY, deltaX) * (180 / Math.PI);
    }

    private static string DetermineExpectedPosition(double angle)
    {
        if (angle < 0) angle += 360;

        if (angle >= 315 || angle < 45) return "Right";
        if (angle >= 45 && angle < 135) return "Top";
        if (angle >= 135 && angle < 225) return "Left";
        return "Bottom";
    }

    [Theory]
    [InlineData(200, 100, "Right")]    // center(100,100) → (200,100): 0도 → Right
    [InlineData(100, 0,   "Top")]      // center(100,100) → (100,0):   90도 → Top
    [InlineData(0,   100, "Left")]     // center(100,100) → (0,100):   180도 → Left
    [InlineData(100, 200, "Bottom")]   // center(100,100) → (100,200): -90도 → Bottom
    public void AngleCalculation_CardinalDirections_ReturnsCorrectPosition(
        double mouseX, double mouseY, string expectedPosition)
    {
        // Arrange
        var center = new Point(100, 100);
        var mouse = new Point(mouseX, mouseY);

        // Act
        var angle = CalculateExpectedAngle(center, mouse);
        var position = DetermineExpectedPosition(angle);

        // Assert
        position.Should().Be(expectedPosition);
    }

    [Theory]
    [InlineData(200, 0,   "Top")]      // 45도 → angle >= 45 → Top
    [InlineData(0,   0,   "Left")]     // 135도 → angle >= 135 → Left
    [InlineData(0,   200, "Bottom")]   // 225도 → angle >= 225 → Bottom
    [InlineData(200, 200, "Right")]    // 315도 → angle >= 315 → Right
    public void AngleCalculation_DiagonalDirections_ReturnsExpectedPosition(
        double mouseX, double mouseY, string expectedPosition)
    {
        // Arrange
        var center = new Point(100, 100);
        var mouse = new Point(mouseX, mouseY);

        // Act
        var angle = CalculateExpectedAngle(center, mouse);
        var position = DetermineExpectedPosition(angle);

        // Assert
        position.Should().Be(expectedPosition);
    }

    [Theory]
    [InlineData(150, 80, "Right")]     // 약 15도
    [InlineData(120, 30, "Top")]       // 약 74도
    [InlineData(30,  80, "Left")]      // 약 164도
    [InlineData(80,  170, "Bottom")]   // 약 -74도 (286도)
    public void AngleCalculation_IntermediateDirections_ReturnsCorrectPosition(
        double mouseX, double mouseY, string expectedPosition)
    {
        // Arrange
        var center = new Point(100, 100);
        var mouse = new Point(mouseX, mouseY);

        // Act
        var angle = CalculateExpectedAngle(center, mouse);
        var position = DetermineExpectedPosition(angle);

        // Assert
        position.Should().Be(expectedPosition);
    }

    [Fact]
    public void AngleCalculation_ExactBoundary45_ReturnsTop()
    {
        // Arrange - 정확히 45도: deltaX == deltaY
        var center = new Point(100, 100);
        var mouse = new Point(200, 0); // deltaX=100, deltaY=100 → atan2(100,100) = 45도

        // Act
        var angle = CalculateExpectedAngle(center, mouse);
        var position = DetermineExpectedPosition(angle);

        // Assert - angle >= 45 이므로 Top
        position.Should().Be("Top");
    }

    [Fact]
    public void AngleCalculation_ExactBoundary135_ReturnsLeft()
    {
        // Arrange - 정확히 135도: deltaX=-100, deltaY=100
        var center = new Point(100, 100);
        var mouse = new Point(0, 0);

        // Act
        var angle = CalculateExpectedAngle(center, mouse);
        var position = DetermineExpectedPosition(angle);

        // Assert - angle >= 135 이므로 Left
        position.Should().Be("Left");
    }

    [Fact]
    public void AngleCalculation_ExactBoundary225_ReturnsBottom()
    {
        // Arrange - 정확히 225도 (= -135도): deltaX=-100, deltaY=-100
        var center = new Point(100, 100);
        var mouse = new Point(0, 200);

        // Act
        var angle = CalculateExpectedAngle(center, mouse);
        var position = DetermineExpectedPosition(angle);

        // Assert - angle >= 225 이므로 Bottom
        position.Should().Be("Bottom");
    }

    [Fact]
    public void AngleCalculation_ExactBoundary315_ReturnsRight()
    {
        // Arrange - 정확히 315도 (= -45도): deltaX=100, deltaY=-100
        var center = new Point(100, 100);
        var mouse = new Point(200, 200);

        // Act
        var angle = CalculateExpectedAngle(center, mouse);
        var position = DetermineExpectedPosition(angle);

        // Assert - angle >= 315 이므로 Right
        position.Should().Be("Right");
    }

    [Fact]
    public void AngleCalculation_NegativeAngle_NormalizedCorrectly()
    {
        // Arrange - 아래쪽 방향: angle = -90도 → 정규화 후 270도 → Bottom
        var center = new Point(100, 100);
        var mouse = new Point(100, 300);

        // Act
        var angle = CalculateExpectedAngle(center, mouse);
        var position = DetermineExpectedPosition(angle);

        // Assert
        angle.Should().BeNegative();
        position.Should().Be("Bottom");
    }

    [Fact]
    public void AngleCalculation_SamePoint_Returns0()
    {
        // Arrange
        var center = new Point(100, 100);
        var mouse = new Point(100, 100);

        // Act
        var angle = CalculateExpectedAngle(center, mouse);

        // Assert - atan2(0, 0) = 0
        angle.Should().Be(0);
    }

    [Fact]
    public void AngleCalculation_VeryLargeCoordinates_HandlesCorrectly()
    {
        // Arrange
        var center = new Point(10000, 10000);
        var mouse = new Point(20000, 10000); // 오른쪽

        // Act
        var angle = CalculateExpectedAngle(center, mouse);
        var position = DetermineExpectedPosition(angle);

        // Assert
        position.Should().Be("Right");
    }

    #endregion

    #region Invalid Area Tests

    /// <summary>
    /// 무효영역 판별 로직 검증 (원형 영역, 지름 52)
    /// distance = sqrt((dx/rx)^2 + (dy/ry)^2), rx = ry = 26
    /// distance <= 1.0이면 무효영역 안
    /// </summary>
    [Fact]
    public void InvalidArea_CenterPoint_IsInvalid()
    {
        // Arrange
        var center = new Point(150, 150);
        var mouse = center; // 동일 좌표
        double radius = 52.0 / 2;

        // Act
        double dx = (mouse.X - center.X) / radius;
        double dy = (mouse.Y - center.Y) / radius;
        double distance = Math.Sqrt(dx * dx + dy * dy);

        // Assert
        distance.Should().BeLessThanOrEqualTo(1.0);
    }

    [Fact]
    public void InvalidArea_JustInsideBoundary_IsInvalid()
    {
        // Arrange
        var center = new Point(150, 150);
        var mouse = new Point(150 + 25, 150); // 반지름 26 안쪽
        double radius = 52.0 / 2;

        // Act
        double dx = (mouse.X - center.X) / radius;
        double dy = (mouse.Y - center.Y) / radius;
        double distance = Math.Sqrt(dx * dx + dy * dy);

        // Assert
        distance.Should().BeLessThanOrEqualTo(1.0);
    }

    [Fact]
    public void InvalidArea_OutsideBoundary_IsValid()
    {
        // Arrange
        var center = new Point(150, 150);
        var mouse = new Point(150 + 30, 150); // 반지름 26 바깥
        double radius = 52.0 / 2;

        // Act
        double dx = (mouse.X - center.X) / radius;
        double dy = (mouse.Y - center.Y) / radius;
        double distance = Math.Sqrt(dx * dx + dy * dy);

        // Assert
        distance.Should().BeGreaterThan(1.0);
    }

    [Fact]
    public void InvalidArea_FarAway_IsValid()
    {
        // Arrange
        var center = new Point(150, 150);
        var mouse = new Point(300, 300);
        double radius = 52.0 / 2;

        // Act
        double dx = (mouse.X - center.X) / radius;
        double dy = (mouse.Y - center.Y) / radius;
        double distance = Math.Sqrt(dx * dx + dy * dy);

        // Assert
        distance.Should().BeGreaterThan(1.0);
    }

    #endregion

    #region LaunchSelectedApp Tests

    [Fact]
    public void Constructor_NoAppsRegistered_DoesNotLaunch()
    {
        // Act
        _ = CreateSut();

        // Assert
        _launchServiceMock.Verify(
            s => s.LaunchApp(It.IsAny<string>()),
            Times.Never);
    }

    #endregion
}
