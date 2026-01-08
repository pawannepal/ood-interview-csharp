using OodInterview.FileSearch.FileSystem;
using OodInterview.FileSearch.Operators;
using OodInterview.FileSearch.Predicates;

namespace OodInterview.FileSearch.Tests;

public class FileSearchTests
{
    [Fact]
    public void TestFileSearch_WithAndPredicate_FindsMatchingFiles()
    {
        // Arrange - Create file system structure
        var root = new FileEntry(isDirectory: true, size: 0, owner: "adam", filename: "root");
        var fileA = new FileEntry(isDirectory: false, size: 2000, owner: "adam", filename: "a");
        var fileB = new FileEntry(isDirectory: false, size: 3000, owner: "george", filename: "b");
        
        root.AddEntry(fileA);
        root.AddEntry(fileB);

        // Create AND predicate: file must NOT be a directory AND owner must match "ge.*"
        var criteria = new FileSearchCriteria(
            new AndPredicate(
            [
                new SimplePredicate<bool>(
                    FileAttribute.IsDirectory,
                    new EqualsOperator<bool>(),
                    false),
                new SimplePredicate<string>(
                    FileAttribute.Owner,
                    new RegexMatchOperator(),
                    "ge.*")
            ]));

        // Act
        var fileSearcher = new FileSearcher();
        var result = fileSearcher.Search(root, criteria);

        // Assert - Only file 'b' should match (owner 'george' starts with 'ge')
        Assert.Single(result);
        Assert.Equal("b", result[0].Filename);
    }

    [Fact]
    public void TestFileSearch_WithOrPredicate_FindsMatchingFiles()
    {
        // Arrange
        var root = new FileEntry(isDirectory: true, size: 0, owner: "root", filename: "root");
        var fileA = new FileEntry(isDirectory: false, size: 2000, owner: "adam", filename: "a");
        var fileB = new FileEntry(isDirectory: false, size: 3000, owner: "george", filename: "b");
        var fileC = new FileEntry(isDirectory: false, size: 1000, owner: "charlie", filename: "c");

        root.AddEntry(fileA);
        root.AddEntry(fileB);
        root.AddEntry(fileC);

        // Create OR predicate: owner is "adam" OR owner is "george"
        var criteria = new FileSearchCriteria(
            new OrPredicate(
            [
                new SimplePredicate<string>(
                    FileAttribute.Owner,
                    new EqualsOperator<string>(),
                    "adam"),
                new SimplePredicate<string>(
                    FileAttribute.Owner,
                    new EqualsOperator<string>(),
                    "george")
            ]));

        // Act
        var fileSearcher = new FileSearcher();
        var result = fileSearcher.Search(root, criteria);

        // Assert - Files 'a' and 'b' should match
        Assert.Equal(2, result.Count);
        Assert.Contains(result, f => f.Filename == "a");
        Assert.Contains(result, f => f.Filename == "b");
    }

    [Fact]
    public void TestFileSearch_WithNotPredicate_ExcludesDirectories()
    {
        // Arrange
        var root = new FileEntry(isDirectory: true, size: 0, owner: "root", filename: "root");
        var subDir = new FileEntry(isDirectory: true, size: 0, owner: "admin", filename: "subdir");
        var fileA = new FileEntry(isDirectory: false, size: 2000, owner: "adam", filename: "a");

        root.AddEntry(subDir);
        root.AddEntry(fileA);

        // Create NOT predicate: NOT a directory
        var criteria = new FileSearchCriteria(
            new NotPredicate(
                new SimplePredicate<bool>(
                    FileAttribute.IsDirectory,
                    new EqualsOperator<bool>(),
                    true)));

        // Act
        var fileSearcher = new FileSearcher();
        var result = fileSearcher.Search(root, criteria);

        // Assert - Only file 'a' should match (excludes root and subdir)
        Assert.Single(result);
        Assert.Equal("a", result[0].Filename);
    }

    [Fact]
    public void TestFileSearch_WithSizeGreaterThan()
    {
        // Arrange
        var root = new FileEntry(isDirectory: true, size: 0, owner: "root", filename: "root");
        var smallFile = new FileEntry(isDirectory: false, size: 100, owner: "user", filename: "small");
        var mediumFile = new FileEntry(isDirectory: false, size: 500, owner: "user", filename: "medium");
        var largeFile = new FileEntry(isDirectory: false, size: 1000, owner: "user", filename: "large");

        root.AddEntry(smallFile);
        root.AddEntry(mediumFile);
        root.AddEntry(largeFile);

        // Create predicate: size > 400
        var criteria = new FileSearchCriteria(
            new SimplePredicate<int>(
                FileAttribute.Size,
                new GreaterThanOperator<int>(),
                400));

        // Act
        var fileSearcher = new FileSearcher();
        var result = fileSearcher.Search(root, criteria);

        // Assert - medium and large files should match
        Assert.Equal(2, result.Count);
        Assert.Contains(result, f => f.Filename == "medium");
        Assert.Contains(result, f => f.Filename == "large");
    }

    [Fact]
    public void TestFileSearch_WithSizeLessThan()
    {
        // Arrange
        var root = new FileEntry(isDirectory: true, size: 0, owner: "root", filename: "root");
        var smallFile = new FileEntry(isDirectory: false, size: 100, owner: "user", filename: "small");
        var mediumFile = new FileEntry(isDirectory: false, size: 500, owner: "user", filename: "medium");
        var largeFile = new FileEntry(isDirectory: false, size: 1000, owner: "user", filename: "large");

        root.AddEntry(smallFile);
        root.AddEntry(mediumFile);
        root.AddEntry(largeFile);

        // Create predicate: size < 600
        var criteria = new FileSearchCriteria(
            new SimplePredicate<int>(
                FileAttribute.Size,
                new LessThanOperator<int>(),
                600));

        // Act
        var fileSearcher = new FileSearcher();
        var result = fileSearcher.Search(root, criteria);

        // Assert - root, small, and medium files should match
        Assert.Equal(3, result.Count);
        Assert.Contains(result, f => f.Filename == "root");
        Assert.Contains(result, f => f.Filename == "small");
        Assert.Contains(result, f => f.Filename == "medium");
    }

    [Fact]
    public void TestFileSearch_WithNestedDirectories()
    {
        // Arrange - Create nested structure
        var root = new FileEntry(isDirectory: true, size: 0, owner: "root", filename: "root");
        var level1Dir = new FileEntry(isDirectory: true, size: 0, owner: "admin", filename: "level1");
        var level2Dir = new FileEntry(isDirectory: true, size: 0, owner: "admin", filename: "level2");
        var file1 = new FileEntry(isDirectory: false, size: 100, owner: "user", filename: "file1");
        var file2 = new FileEntry(isDirectory: false, size: 200, owner: "user", filename: "file2");
        var file3 = new FileEntry(isDirectory: false, size: 300, owner: "user", filename: "file3");

        level2Dir.AddEntry(file3);
        level1Dir.AddEntry(level2Dir);
        level1Dir.AddEntry(file2);
        root.AddEntry(level1Dir);
        root.AddEntry(file1);

        // Create predicate: find all files with owner "user"
        var criteria = new FileSearchCriteria(
            new SimplePredicate<string>(
                FileAttribute.Owner,
                new EqualsOperator<string>(),
                "user"));

        // Act
        var fileSearcher = new FileSearcher();
        var result = fileSearcher.Search(root, criteria);

        // Assert - All 3 files should be found across all nested levels
        Assert.Equal(3, result.Count);
        Assert.Contains(result, f => f.Filename == "file1");
        Assert.Contains(result, f => f.Filename == "file2");
        Assert.Contains(result, f => f.Filename == "file3");
    }

    [Fact]
    public void TestFileSearch_FindByFilename()
    {
        // Arrange
        var root = new FileEntry(isDirectory: true, size: 0, owner: "root", filename: "root");
        var readme = new FileEntry(isDirectory: false, size: 500, owner: "user", filename: "README.md");
        var config = new FileEntry(isDirectory: false, size: 200, owner: "user", filename: "config.json");

        root.AddEntry(readme);
        root.AddEntry(config);

        // Create predicate: filename matches ".*\\.md" (ends with .md)
        var criteria = new FileSearchCriteria(
            new SimplePredicate<string>(
                FileAttribute.Filename,
                new RegexMatchOperator(),
                ".*\\.md"));

        // Act
        var fileSearcher = new FileSearcher();
        var result = fileSearcher.Search(root, criteria);

        // Assert - Only README.md should match
        Assert.Single(result);
        Assert.Equal("README.md", result[0].Filename);
    }
}
