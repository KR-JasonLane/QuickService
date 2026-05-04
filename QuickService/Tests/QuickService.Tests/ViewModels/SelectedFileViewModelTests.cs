using CommunityToolkit.Mvvm.Messaging;
using Moq;
using QuickService.Abstract.Interfaces;
using QuickService.Models.Configure;
using QuickService.ViewModels;
using QuickService.ViewModels.Messenger;

namespace QuickService.Tests.ViewModels;

public class SelectedFileViewModelTests : IDisposable
{
    private readonly Mock<IConfigurationService> _configServiceMock;

    public SelectedFileViewModelTests()
    {
        _configServiceMock = new Mock<IConfigurationService>();
        // 기본 설정: 빈 등록 모델 반환
        _configServiceMock
            .Setup(s => s.GetConfiguration<RegisteredApplicationModel>())
            .Returns(new RegisteredApplicationModel());
    }

    public void Dispose()
    {
        WeakReferenceMessenger.Default.Reset();
    }

    private SelectedFileViewModel CreateSut()
    {
        return new SelectedFileViewModel(_configServiceMock.Object);
    }

    #region Constructor Tests

    [Fact]
    public void Constructor_LoadsConfigurationForAllPositions()
    {
        // Act
        _ = CreateSut();

        // Assert - 4방향 각각에 대해 GetConfiguration 호출
        _configServiceMock.Verify(
            s => s.GetConfiguration<RegisteredApplicationModel>(),
            Times.AtLeast(4));
    }

    [Fact]
    public void Constructor_NoValidPaths_SetsEmptyAppNames()
    {
        // Act
        var sut = CreateSut();

        // Assert
        sut.LeftAppName.Should().Be("Empty");
        sut.TopAppName.Should().Be("Empty");
        sut.RightAppName.Should().Be("Empty");
        sut.BottomAppName.Should().Be("Empty");
    }

    [Fact]
    public void Constructor_NoValidPaths_SetsIconImages()
    {
        // Act
        var sut = CreateSut();

        // Assert - 빈 아이콘이 설정되어야 함
        sut.LeftAppIconImageSource.Should().NotBeNull();
        sut.TopAppIconImageSource.Should().NotBeNull();
        sut.RightAppIconImageSource.Should().NotBeNull();
        sut.BottomAppIconImageSource.Should().NotBeNull();
    }

    [Fact]
    public void Constructor_NullConfiguration_ThrowsInvalidOperationException()
    {
        // Arrange
        _configServiceMock
            .Setup(s => s.GetConfiguration<RegisteredApplicationModel>())
            .Returns((RegisteredApplicationModel)null!);

        // Act
        var act = () => CreateSut();

        // Assert
        act.Should().Throw<InvalidOperationException>();
    }

    #endregion

    #region AppInformationChangedMessage Tests

    [Fact]
    public void OnAppInformationChanged_UpdatesDisplayForPosition()
    {
        // Arrange
        var sut = CreateSut();
        var initialLeftName = sut.LeftAppName;

        // Act - 메신저를 통해 변경 알림
        WeakReferenceMessenger.Default.Send(new AppInformationChangedMessage(AppPosition.Left));

        // Assert - GetConfiguration이 다시 호출됨
        _configServiceMock.Verify(
            s => s.GetConfiguration<RegisteredApplicationModel>(),
            Times.AtLeast(5)); // 초기 4회 + 메시지 1회
    }

    [Theory]
    [InlineData(AppPosition.Left)]
    [InlineData(AppPosition.Top)]
    [InlineData(AppPosition.Right)]
    [InlineData(AppPosition.Bottom)]
    public void OnAppInformationChanged_EachPosition_SetsEmptyNameWhenNoPath(AppPosition position)
    {
        // Arrange
        var sut = CreateSut();

        // Act
        WeakReferenceMessenger.Default.Send(new AppInformationChangedMessage(position));

        // Assert
        var name = position switch
        {
            AppPosition.Left   => sut.LeftAppName,
            AppPosition.Top    => sut.TopAppName,
            AppPosition.Right  => sut.RightAppName,
            AppPosition.Bottom => sut.BottomAppName,
            _ => null
        };
        name.Should().Be("Empty");
    }

    #endregion

    #region Display Logic Tests

    [Fact]
    public void UpdateAppDisplay_InvalidPath_SetsEmptyAppName()
    {
        // Arrange
        var config = new RegisteredApplicationModel();
        config.LeftAppInformation.AppPath = @"C:\nonexistent\fake.exe";
        _configServiceMock
            .Setup(s => s.GetConfiguration<RegisteredApplicationModel>())
            .Returns(config);

        // Act
        var sut = CreateSut();

        // Assert - 파일이 존재하지 않으므로 Empty
        sut.LeftAppName.Should().Be("Empty");
    }

    [Fact]
    public void UpdateAppDisplay_EmptyPath_SetsEmptyAppName()
    {
        // Arrange
        var config = new RegisteredApplicationModel();
        config.TopAppInformation.AppPath = "";
        _configServiceMock
            .Setup(s => s.GetConfiguration<RegisteredApplicationModel>())
            .Returns(config);

        // Act
        var sut = CreateSut();

        // Assert
        sut.TopAppName.Should().Be("Empty");
    }

    [Fact]
    public void UpdateAppDisplay_NullPath_SetsEmptyAppName()
    {
        // Arrange
        var config = new RegisteredApplicationModel();
        config.RightAppInformation.AppPath = null;
        _configServiceMock
            .Setup(s => s.GetConfiguration<RegisteredApplicationModel>())
            .Returns(config);

        // Act
        var sut = CreateSut();

        // Assert
        sut.RightAppName.Should().Be("Empty");
    }

    #endregion
}
