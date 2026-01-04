using System.IO;
using QuickService.Core.Services;

namespace QuickService.Tests.Core;

public class JsonFileServiceTests : IDisposable
{
    private readonly JsonFileService _sut;
    private readonly string _testDirectory;

    public JsonFileServiceTests()
    {
        _sut = new JsonFileService();
        _testDirectory = Path.Combine(Path.GetTempPath(), "QuickServiceTests", Guid.NewGuid().ToString());
    }

    public void Dispose()
    {
        if (Directory.Exists(_testDirectory))
            Directory.Delete(_testDirectory, recursive: true);
    }

    private string GetTestFilePath(string fileName = "test.json")
    {
        return Path.Combine(_testDirectory, fileName);
    }

    #region Save Tests

    [Fact]
    public void Save_ValidObject_CreatesFile()
    {
        // Arrange
        var path = GetTestFilePath();
        var obj = new TestModel { Name = "Test", Value = 42 };

        // Act
        _sut.Save(obj, path);

        // Assert
        File.Exists(path).Should().BeTrue();
    }

    [Fact]
    public void Save_ValidObject_SerializesCorrectly()
    {
        // Arrange
        var path = GetTestFilePath();
        var obj = new TestModel { Name = "Hello", Value = 99 };

        // Act
        _sut.Save(obj, path);

        // Assert
        var json = File.ReadAllText(path);
        json.Should().Contain("\"Name\":\"Hello\"");
        json.Should().Contain("\"Value\":99");
    }

    [Fact]
    public void Save_DirectoryNotExists_CreatesDirectory()
    {
        // Arrange
        var path = Path.Combine(_testDirectory, "sub", "deep", "test.json");
        var obj = new TestModel { Name = "Test", Value = 1 };

        // Act
        _sut.Save(obj, path);

        // Assert
        File.Exists(path).Should().BeTrue();
    }

    [Fact]
    public void Save_NullPath_ThrowsArgumentException()
    {
        // Arrange
        var obj = new TestModel { Name = "Test", Value = 1 };

        // Act
        var act = () => _sut.Save(obj, null!);

        // Assert
        act.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void Save_EmptyPath_ThrowsArgumentException()
    {
        // Arrange
        var obj = new TestModel { Name = "Test", Value = 1 };

        // Act
        var act = () => _sut.Save(obj, string.Empty);

        // Assert
        act.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void Save_OverwritesExistingFile()
    {
        // Arrange
        var path = GetTestFilePath();
        var original = new TestModel { Name = "Original", Value = 1 };
        var updated = new TestModel { Name = "Updated", Value = 2 };

        // Act
        _sut.Save(original, path);
        _sut.Save(updated, path);

        // Assert
        var json = File.ReadAllText(path);
        json.Should().Contain("\"Name\":\"Updated\"");
        json.Should().Contain("\"Value\":2");
    }

    #endregion

    #region Load Tests

    [Fact]
    public void Load_ExistingFile_DeserializesCorrectly()
    {
        // Arrange
        var path = GetTestFilePath();
        var original = new TestModel { Name = "Saved", Value = 77 };
        _sut.Save(original, path);

        // Act
        var loaded = _sut.Load<TestModel>(path);

        // Assert
        loaded.Name.Should().Be("Saved");
        loaded.Value.Should().Be(77);
    }

    [Fact]
    public void Load_NonExistentFile_CreatesDefaultAndSaves()
    {
        // Arrange
        var path = GetTestFilePath();

        // Act
        var result = _sut.Load<TestModel>(path);

        // Assert
        result.Should().NotBeNull();
        result.Name.Should().BeNull();
        result.Value.Should().Be(0);
        File.Exists(path).Should().BeTrue();
    }

    [Fact]
    public void Load_CorruptedFile_ThrowsJsonException()
    {
        // Arrange
        var path = GetTestFilePath();
        Directory.CreateDirectory(Path.GetDirectoryName(path)!);
        File.WriteAllText(path, "NOT VALID JSON {{{");

        // Act
        var act = () => _sut.Load<TestModel>(path);

        // Assert
        act.Should().Throw<Newtonsoft.Json.JsonReaderException>();
    }

    [Fact]
    public void SaveAndLoad_RoundTrip_PreservesData()
    {
        // Arrange
        var path = GetTestFilePath();
        var original = new TestModel { Name = "RoundTrip", Value = 123 };

        // Act
        _sut.Save(original, path);
        var loaded = _sut.Load<TestModel>(path);

        // Assert
        loaded.Name.Should().Be(original.Name);
        loaded.Value.Should().Be(original.Value);
    }

    [Fact]
    public void SaveAndLoad_ComplexObject_PreservesNestedData()
    {
        // Arrange
        var path = GetTestFilePath();
        var original = new TestModelWithNested
        {
            Title = "Parent",
            Child = new TestModel { Name = "Child", Value = 42 }
        };

        // Act
        _sut.Save(original, path);
        var loaded = _sut.Load<TestModelWithNested>(path);

        // Assert
        loaded.Title.Should().Be("Parent");
        loaded.Child.Should().NotBeNull();
        loaded.Child!.Name.Should().Be("Child");
        loaded.Child.Value.Should().Be(42);
    }

    [Fact]
    public void SaveAndLoad_EmptyObject_PreservesDefaults()
    {
        // Arrange
        var path = GetTestFilePath();
        var original = new TestModel();

        // Act
        _sut.Save(original, path);
        var loaded = _sut.Load<TestModel>(path);

        // Assert
        loaded.Name.Should().BeNull();
        loaded.Value.Should().Be(0);
    }

    #endregion

    #region Test Models

    private class TestModel
    {
        public string? Name { get; set; }
        public int Value { get; set; }
    }

    private class TestModelWithNested
    {
        public string? Title { get; set; }
        public TestModel? Child { get; set; }
    }

    #endregion
}
