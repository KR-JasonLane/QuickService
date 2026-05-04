using CommunityToolkit.Mvvm.Messaging;
using Moq;
using QuickService.Abstract.Interfaces;
using QuickService.Models.Configure;
using QuickService.ViewModels;
using QuickService.ViewModels.Messenger;

namespace QuickService.Tests.ViewModels;

public class InteractionViewModelTests : IDisposable
{
    private readonly Mock<IUserSelectPathService> _userSelectPathMock;
    private readonly Mock<IConfigurationService> _configServiceMock;
    private readonly InteractionViewModel _sut;

    public InteractionViewModelTests()
    {
        _userSelectPathMock = new Mock<IUserSelectPathService>();
        _configServiceMock = new Mock<IConfigurationService>();

        _configServiceMock
            .Setup(s => s.GetConfiguration<RegisteredApplicationModel>())
            .Returns(new RegisteredApplicationModel());

        _sut = new InteractionViewModel(
            _userSelectPathMock.Object,
            _configServiceMock.Object);
    }

    public void Dispose()
    {
        WeakReferenceMessenger.Default.Reset();
    }

    #region RegistrationQuickServiceApplication Tests

    [Fact]
    public void RegistrationQuickServiceApplication_ValidPath_SavesConfiguration()
    {
        // Arrange
        _userSelectPathMock
            .Setup(s => s.GetUserSelectedFilePath())
            .Returns(@"C:\test\app.exe");

        // Act
        _sut.RegistrationQuickServiceApplicationCommand.Execute("Left");

        // Assert
        _configServiceMock.Verify(
            s => s.SaveConfiguration(It.IsAny<RegisteredApplicationModel>()),
            Times.Once);
    }

    [Fact]
    public void RegistrationQuickServiceApplication_ValidPath_SetsAppPathOnCorrectPosition()
    {
        // Arrange
        var config = new RegisteredApplicationModel();
        _configServiceMock
            .Setup(s => s.GetConfiguration<RegisteredApplicationModel>())
            .Returns(config);
        _userSelectPathMock
            .Setup(s => s.GetUserSelectedFilePath())
            .Returns(@"C:\test\app.exe");

        // Act
        _sut.RegistrationQuickServiceApplicationCommand.Execute("Top");

        // Assert
        config.TopAppInformation.AppPath.Should().Be(@"C:\test\app.exe");
    }

    [Fact]
    public void RegistrationQuickServiceApplication_EmptyPath_DoesNotSave()
    {
        // Arrange
        _userSelectPathMock
            .Setup(s => s.GetUserSelectedFilePath())
            .Returns(string.Empty);

        // Act
        _sut.RegistrationQuickServiceApplicationCommand.Execute("Left");

        // Assert
        _configServiceMock.Verify(
            s => s.SaveConfiguration(It.IsAny<RegisteredApplicationModel>()),
            Times.Never);
    }

    [Fact]
    public void RegistrationQuickServiceApplication_NullPath_DoesNotSave()
    {
        // Arrange
        _userSelectPathMock
            .Setup(s => s.GetUserSelectedFilePath())
            .Returns((string)null!);

        // Act
        _sut.RegistrationQuickServiceApplicationCommand.Execute("Right");

        // Assert
        _configServiceMock.Verify(
            s => s.SaveConfiguration(It.IsAny<RegisteredApplicationModel>()),
            Times.Never);
    }

    [Fact]
    public void RegistrationQuickServiceApplication_ValidPath_SendsAppInformationChangedMessage()
    {
        // Arrange
        _userSelectPathMock
            .Setup(s => s.GetUserSelectedFilePath())
            .Returns(@"C:\test\app.exe");

        AppPosition? receivedPosition = null;
        WeakReferenceMessenger.Default.Register<AppInformationChangedMessage>(this, (r, m) =>
            receivedPosition = m.Value);

        // Act
        _sut.RegistrationQuickServiceApplicationCommand.Execute("Bottom");

        // Assert
        receivedPosition.Should().Be(AppPosition.Bottom);
    }

    [Theory]
    [InlineData("Left", AppPosition.Left)]
    [InlineData("Top", AppPosition.Top)]
    [InlineData("Right", AppPosition.Right)]
    [InlineData("Bottom", AppPosition.Bottom)]
    [InlineData("left", AppPosition.Left)]
    [InlineData("RIGHT", AppPosition.Right)]
    public void RegistrationQuickServiceApplication_AllPositions_ParsesCorrectly(
        string param, AppPosition expectedPosition)
    {
        // Arrange
        var config = new RegisteredApplicationModel();
        _configServiceMock
            .Setup(s => s.GetConfiguration<RegisteredApplicationModel>())
            .Returns(config);
        _userSelectPathMock
            .Setup(s => s.GetUserSelectedFilePath())
            .Returns(@"C:\test\app.exe");

        // Act
        _sut.RegistrationQuickServiceApplicationCommand.Execute(param);

        // Assert
        config.GetByPosition(expectedPosition).AppPath.Should().Be(@"C:\test\app.exe");
    }

    [Fact]
    public void RegistrationQuickServiceApplication_InvalidPosition_ThrowsArgumentException()
    {
        // Arrange
        _userSelectPathMock
            .Setup(s => s.GetUserSelectedFilePath())
            .Returns(@"C:\test\app.exe");

        // Act
        var act = () => _sut.RegistrationQuickServiceApplicationCommand.Execute("InvalidPosition");

        // Assert
        act.Should().Throw<ArgumentException>();
    }

    #endregion
}
