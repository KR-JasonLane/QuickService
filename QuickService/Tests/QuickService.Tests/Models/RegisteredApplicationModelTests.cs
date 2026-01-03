using QuickService.Models.Configure;

namespace QuickService.Tests.Models;

public class RegisteredApplicationModelTests
{
    [Fact]
    public void Constructor_Default_AllPositionsInitialized()
    {
        // Act
        var model = new RegisteredApplicationModel();

        // Assert
        model.LeftAppInformation.Should().NotBeNull();
        model.TopAppInformation.Should().NotBeNull();
        model.RightAppInformation.Should().NotBeNull();
        model.BottomAppInformation.Should().NotBeNull();
    }

    [Fact]
    public void AllPositions_AreIndependent()
    {
        // Arrange
        var model = new RegisteredApplicationModel();

        // Assert - 각 위치의 AppInformation은 서로 다른 인스턴스
        model.LeftAppInformation.Should().NotBeSameAs(model.TopAppInformation);
        model.LeftAppInformation.Should().NotBeSameAs(model.RightAppInformation);
        model.LeftAppInformation.Should().NotBeSameAs(model.BottomAppInformation);
        model.TopAppInformation.Should().NotBeSameAs(model.RightAppInformation);
        model.TopAppInformation.Should().NotBeSameAs(model.BottomAppInformation);
        model.RightAppInformation.Should().NotBeSameAs(model.BottomAppInformation);
    }

    [Fact]
    public void GetByPosition_Left_ReturnsLeftAppInformation()
    {
        var model = new RegisteredApplicationModel();
        model.GetByPosition(AppPosition.Left).Should().BeSameAs(model.LeftAppInformation);
    }

    [Fact]
    public void GetByPosition_Top_ReturnsTopAppInformation()
    {
        var model = new RegisteredApplicationModel();
        model.GetByPosition(AppPosition.Top).Should().BeSameAs(model.TopAppInformation);
    }

    [Fact]
    public void GetByPosition_Right_ReturnsRightAppInformation()
    {
        var model = new RegisteredApplicationModel();
        model.GetByPosition(AppPosition.Right).Should().BeSameAs(model.RightAppInformation);
    }

    [Fact]
    public void GetByPosition_Bottom_ReturnsBottomAppInformation()
    {
        var model = new RegisteredApplicationModel();
        model.GetByPosition(AppPosition.Bottom).Should().BeSameAs(model.BottomAppInformation);
    }

    [Fact]
    public void GetByPosition_Invalid_ThrowsArgumentOutOfRange()
    {
        var model = new RegisteredApplicationModel();
        var act = () => model.GetByPosition((AppPosition)999);
        act.Should().Throw<ArgumentOutOfRangeException>();
    }
}

public class ConfigureModelTests
{
    [Fact]
    public void Constructor_Default_HasRegisteredApplication()
    {
        var model = new ConfigureModel();
        model.RegisteredApplication.Should().NotBeNull();
    }

    [Fact]
    public void RegisteredApplication_Default_HasAllPositions()
    {
        var model = new ConfigureModel();
        model.RegisteredApplication.LeftAppInformation.Should().NotBeNull();
        model.RegisteredApplication.TopAppInformation.Should().NotBeNull();
        model.RegisteredApplication.RightAppInformation.Should().NotBeNull();
        model.RegisteredApplication.BottomAppInformation.Should().NotBeNull();
    }
}

public class AppInformationModelTests
{
    [Fact]
    public void NewModel_AppPath_IsNull()
    {
        var model = new AppInformationModel();
        model.AppPath.Should().BeNull();
    }

    [Fact]
    public void HasValidPath_NullPath_ReturnsFalse()
    {
        var model = new AppInformationModel();
        model.HasValidPath().Should().BeFalse();
    }

    [Fact]
    public void HasValidPath_EmptyPath_ReturnsFalse()
    {
        var model = new AppInformationModel { AppPath = "" };
        model.HasValidPath().Should().BeFalse();
    }

    [Fact]
    public void HasValidPath_NonExistentPath_ReturnsFalse()
    {
        var model = new AppInformationModel { AppPath = @"C:\nonexistent\fake.exe" };
        model.HasValidPath().Should().BeFalse();
    }

    [Fact]
    public void IconImage_NewModel_ReturnsNull()
    {
        var model = new AppInformationModel();
        model.IconImage.Should().BeNull();
    }

    [Fact]
    public void DisplayName_NewModel_ReturnsNull()
    {
        var model = new AppInformationModel();
        model.DisplayName.Should().BeNull();
    }

    [Fact]
    public void LoadInfoFromPath_InvalidPath_DoesNothing()
    {
        var model = new AppInformationModel { AppPath = @"C:\nonexistent\fake.exe" };

        model.LoadInfoFromPath();

        model.IconImage.Should().BeNull();
        model.DisplayName.Should().BeNull();
    }

    [Fact]
    public void AppPath_Setter_NoSideEffects()
    {
        // setter가 더 이상 부수효과를 발생시키지 않음을 확인
        var model = new AppInformationModel();

        model.AppPath = @"C:\nonexistent\fake.exe";

        model.AppPath.Should().Be(@"C:\nonexistent\fake.exe");
        model.IconImage.Should().BeNull();
        model.DisplayName.Should().BeNull();
    }
}
