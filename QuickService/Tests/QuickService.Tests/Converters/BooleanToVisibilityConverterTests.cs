using System.Globalization;
using System.Windows;
using QuickService.Util.Converters;

namespace QuickService.Tests.Converters;

public class BooleanToVisibilityConverterTests
{
    private readonly BooleanToVisibilityConverter _sut = new();

    #region Convert Tests

    [Fact]
    public void Convert_True_ReturnsVisible()
    {
        var result = _sut.Convert(true, typeof(Visibility), null!, CultureInfo.InvariantCulture);
        result.Should().Be(Visibility.Visible);
    }

    [Fact]
    public void Convert_False_ReturnsCollapsed()
    {
        var result = _sut.Convert(false, typeof(Visibility), null!, CultureInfo.InvariantCulture);
        result.Should().Be(Visibility.Collapsed);
    }

    [Fact]
    public void Convert_NonBoolValue_ReturnsVisible()
    {
        var result = _sut.Convert("not a bool", typeof(Visibility), null!, CultureInfo.InvariantCulture);
        result.Should().Be(Visibility.Visible);
    }

    [Fact]
    public void Convert_NullValue_ReturnsVisible()
    {
        var result = _sut.Convert(null!, typeof(Visibility), null!, CultureInfo.InvariantCulture);
        result.Should().Be(Visibility.Visible);
    }

    [Fact]
    public void Convert_IntegerValue_ReturnsVisible()
    {
        var result = _sut.Convert(42, typeof(Visibility), null!, CultureInfo.InvariantCulture);
        result.Should().Be(Visibility.Visible);
    }

    #endregion

    #region ConvertBack Tests

    [Fact]
    public void ConvertBack_Visible_ReturnsTrue()
    {
        var result = _sut.ConvertBack(Visibility.Visible, typeof(bool), null!, CultureInfo.InvariantCulture);
        result.Should().Be(true);
    }

    [Fact]
    public void ConvertBack_Collapsed_ReturnsFalse()
    {
        var result = _sut.ConvertBack(Visibility.Collapsed, typeof(bool), null!, CultureInfo.InvariantCulture);
        result.Should().Be(false);
    }

    [Fact]
    public void ConvertBack_Hidden_ReturnsFalse()
    {
        var result = _sut.ConvertBack(Visibility.Hidden, typeof(bool), null!, CultureInfo.InvariantCulture);
        result.Should().Be(false);
    }

    [Fact]
    public void ConvertBack_NonVisibilityValue_ReturnsFalse()
    {
        var result = _sut.ConvertBack("not visibility", typeof(bool), null!, CultureInfo.InvariantCulture);
        result.Should().Be(false);
    }

    #endregion
}

public class BooleanToVisibilityReverseConverterTests
{
    private readonly BooleanToVisibilityReverseConverter _sut = new();

    #region Convert Tests

    [Fact]
    public void Convert_True_ReturnsCollapsed()
    {
        var result = _sut.Convert(true, typeof(Visibility), null!, CultureInfo.InvariantCulture);
        result.Should().Be(Visibility.Collapsed);
    }

    [Fact]
    public void Convert_False_ReturnsVisible()
    {
        var result = _sut.Convert(false, typeof(Visibility), null!, CultureInfo.InvariantCulture);
        result.Should().Be(Visibility.Visible);
    }

    [Fact]
    public void Convert_NonBoolValue_ReturnsCollapsed()
    {
        var result = _sut.Convert("not a bool", typeof(Visibility), null!, CultureInfo.InvariantCulture);
        result.Should().Be(Visibility.Collapsed);
    }

    [Fact]
    public void Convert_NullValue_ReturnsCollapsed()
    {
        var result = _sut.Convert(null!, typeof(Visibility), null!, CultureInfo.InvariantCulture);
        result.Should().Be(Visibility.Collapsed);
    }

    #endregion

    #region ConvertBack Tests

    [Fact]
    public void ConvertBack_Visible_ReturnsTrue()
    {
        var result = _sut.ConvertBack(Visibility.Visible, typeof(bool), null!, CultureInfo.InvariantCulture);
        result.Should().Be(true);
    }

    [Fact]
    public void ConvertBack_Collapsed_ReturnsFalse()
    {
        var result = _sut.ConvertBack(Visibility.Collapsed, typeof(bool), null!, CultureInfo.InvariantCulture);
        result.Should().Be(false);
    }

    #endregion

    #region Reverse Behavior Verification

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public void Convert_OppositeOfNormalConverter(bool input)
    {
        // Arrange
        var normalConverter = new BooleanToVisibilityConverter();

        // Act
        var normalResult = normalConverter.Convert(input, typeof(Visibility), null!, CultureInfo.InvariantCulture);
        var reverseResult = _sut.Convert(input, typeof(Visibility), null!, CultureInfo.InvariantCulture);

        // Assert - 두 컨버터의 결과는 항상 반대여야 함
        normalResult.Should().NotBe(reverseResult);
    }

    #endregion
}
