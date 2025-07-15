# SpProgress

A lightweight and customizable console progress bar library for asynchronous operations in .NET.

---

## Features

- Simple and flexible console progress bar with percentage display
- Supports any kind of progress tracking (file download, file copy, or custom use cases)
- Asynchronous API for smooth UI updates
- UTF-8 support for rich progress characters
- Easy to extend and customize

---

---

## Installation

Install via [NuGet](https://www.nuget.org/packages/SpProgress):

```bash
dotnet add package SpProgress
```

---

## Usage

Use the `SpProgress` library for various progress tracking scenarios such as downloading files, copying files, or any custom progress.

### Download a file with progress bar

```csharp
using SpProgress;

var downloader = new FileDownloadProgress();
await downloader.DownloadAsync("https://example.com/file.zip", "file.zip");
```

### Copy a file with progress bar

```csharp
using SpProgress;

var copier = new CopyFileProgress();
await copier.CopyAsync("source/path/file.zip", "destination/path/file.zip");
```

### Custom progress bar for any task

```csharp
using SpProgress;

var totalSteps = 100;
var progressBar = new ProgressBar(totalSteps, barLength: 40, progressChar: '#', prefix: "Processing: ");

for (int i = 0; i <= totalSteps; i++)
{
    progressBar.Update(i);
    await Task.Delay(50);
}

progressBar.Finish();
```
