# Unix File Search System

This project implements a flexible file search system similar to Unix's `find` command, allowing users to search for files based on various criteria such as name, owner, size, and directory status.

## Project Structure

```
OodInterview.FileSearch/
├── FileSearch.cs              # Main search implementation
├── FileSearchCriteria.cs      # Search criteria wrapper
├── FileSystem/
│   ├── FileAttribute.cs       # Enum for file attributes
│   └── FileEntry.cs           # File/directory representation
├── Operators/
│   ├── IComparisonOperator.cs # Base interface for operators
│   ├── EqualsOperator.cs      # Exact equality comparison
│   ├── GreaterThanOperator.cs # Greater than comparison
│   ├── LessThanOperator.cs    # Less than comparison
│   └── RegexMatchOperator.cs  # Regex pattern matching
└── Predicates/
    ├── IPredicate.cs          # Base predicate interface
    ├── ICompositePredicate.cs # Marker for composite predicates
    ├── SimplePredicate.cs     # Single attribute comparison
    ├── AndPredicate.cs        # Logical AND (Composite Pattern)
    ├── OrPredicate.cs         # Logical OR (Composite Pattern)
    └── NotPredicate.cs        # Logical NOT (Composite Pattern)
```

## Design Patterns Used

### 1. Composite Pattern

The **Composite Pattern** is used to build complex search criteria from simpler predicates. This allows combining multiple conditions using AND, OR, and NOT operators.

```
        IPredicate
            │
    ┌───────┴───────┐
    │               │
SimplePredicate  ICompositePredicate
                    │
        ┌───────────┼───────────┐
        │           │           │
   AndPredicate OrPredicate NotPredicate
```

**Key benefits:**
- Predicates can be nested to any depth
- Complex queries are built from simple components
- Uniform treatment of simple and composite predicates

**Example - Building a complex query:**
```csharp
// Find files that are NOT directories AND owned by someone whose name starts with "ge"
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
```

### 2. Strategy Pattern

The **Strategy Pattern** is used for comparison operators. Different operators can be plugged in to change how attribute values are compared.

```
    IComparisonOperator<T>
            │
    ┌───────┼───────────────┬────────────────┐
    │       │               │                │
Equals   GreaterThan   LessThan    RegexMatch
Operator   Operator      Operator    Operator
```

**Available Operators:**
- `EqualsOperator<T>` - Exact equality comparison
- `GreaterThanOperator<T>` - Greater than for comparable types
- `LessThanOperator<T>` - Less than for comparable types
- `RegexMatchOperator` - Regular expression matching for strings

### 3. Iterator Pattern (Implicit)

The file search uses a **Stack-based iteration** to traverse the file system without recursion, avoiding stack overflow on deep directory structures.

```csharp
var recursionStack = new Stack<FileEntry>();
recursionStack.Push(root);

while (recursionStack.Count > 0)
{
    var next = recursionStack.Pop();
    
    if (criteria.IsMatch(next))
        result.Add(next);
    
    foreach (var entry in next.Entries)
        recursionStack.Push(entry);
}
```

## Usage Example

```csharp
using OodInterview.FileSearch;
using OodInterview.FileSearch.FileSystem;
using OodInterview.FileSearch.Operators;
using OodInterview.FileSearch.Predicates;

// Create a file hierarchy
var root = new FileEntry(isDirectory: true, size: 0, owner: "adam", filename: "root");
var docs = new FileEntry(isDirectory: true, size: 0, owner: "adam", filename: "docs");
var readme = new FileEntry(isDirectory: false, size: 2000, owner: "adam", filename: "README.md");
var config = new FileEntry(isDirectory: false, size: 500, owner: "system", filename: "config.json");

root.AddEntry(docs);
root.AddEntry(config);
docs.AddEntry(readme);

// Search for markdown files
var criteria = new FileSearchCriteria(
    new SimplePredicate<string>(
        FileAttribute.Filename,
        new RegexMatchOperator(),
        ".*\\.md"));

var searcher = new FileSearcher();
var results = searcher.Search(root, criteria);

// results will contain only the README.md file
```

## Searchable Attributes

| Attribute | Type | Description |
|-----------|------|-------------|
| `IsDirectory` | `bool` | Whether the entry is a directory |
| `Size` | `int` | File size in bytes |
| `Owner` | `string` | Owner of the file |
| `Filename` | `string` | Name of the file |

## Building Complex Queries

### AND Query (All conditions must match)
```csharp
// Files larger than 1000 bytes AND owned by "admin"
new AndPredicate(
[
    new SimplePredicate<int>(FileAttribute.Size, new GreaterThanOperator<int>(), 1000),
    new SimplePredicate<string>(FileAttribute.Owner, new EqualsOperator<string>(), "admin")
]);
```

### OR Query (Any condition can match)
```csharp
// Files owned by "adam" OR "george"
new OrPredicate(
[
    new SimplePredicate<string>(FileAttribute.Owner, new EqualsOperator<string>(), "adam"),
    new SimplePredicate<string>(FileAttribute.Owner, new EqualsOperator<string>(), "george")
]);
```

### NOT Query (Negation)
```csharp
// Files that are NOT directories
new NotPredicate(
    new SimplePredicate<bool>(FileAttribute.IsDirectory, new EqualsOperator<bool>(), true));
```

### Nested Queries
```csharp
// Files that are NOT directories AND (owner is "adam" OR size > 5000)
new AndPredicate(
[
    new NotPredicate(
        new SimplePredicate<bool>(FileAttribute.IsDirectory, new EqualsOperator<bool>(), true)),
    new OrPredicate(
    [
        new SimplePredicate<string>(FileAttribute.Owner, new EqualsOperator<string>(), "adam"),
        new SimplePredicate<int>(FileAttribute.Size, new GreaterThanOperator<int>(), 5000)
    ])
]);
```

## Java to C# Conversion Notes

| Java | C# |
|------|-----|
| `File` class | `FileEntry` class (renamed to avoid conflict with `System.IO.File`) |
| `HashSet<File>` | `HashSet<FileEntry>` with `IReadOnlySet<FileEntry>` interface |
| `Collections.unmodifiableSet()` | Direct exposure via `IReadOnlySet<T>` |
| `ArrayDeque<File>` | `Stack<FileEntry>` |
| `stream().allMatch()` | LINQ `.All()` |
| `stream().anyMatch()` | LINQ `.Any()` |
| `Pattern.compile().matcher().matches()` | `new Regex().IsMatch()` |
| `Objects.equals()` | `Equals()` |

## Running Tests

```bash
# From repository root
dotnet test tests/OodInterview.FileSearch.Tests
```
