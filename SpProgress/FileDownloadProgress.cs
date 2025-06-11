using System.Text;

namespace SpProgress;

/// <summary>
/// Provides functionality to download a file from a URL while displaying a live progress bar in the console.
/// </summary>
public class FileDownloadProgress
{
    private readonly HttpClient _httpClient;
    private readonly int _barLength;
    private readonly char _progressChar;
    private readonly string _prefix;

    /// <summary>
    /// Initializes a new instance of the <see cref="FileDownloadProgress"/> class.
    /// </summary>
    /// <param name="httpClient">An optional custom <see cref="HttpClient"/> instance. If null, a default client is used.</param>
    /// <param name="barLength">Length of the progress bar (number of characters).</param>
    /// <param name="progressChar">Character used to display the filled portion of the progress bar.</param>
    /// <param name="prefix">Optional prefix string displayed before the progress bar.</param>
    public FileDownloadProgress(HttpClient? httpClient = null, int barLength = 50, char progressChar = '█', string prefix = "Downloading: ")
    {
        _httpClient = httpClient ?? new HttpClient();
        _barLength = barLength;
        _progressChar = progressChar;
        _prefix = prefix;

        Console.OutputEncoding = Encoding.UTF8;
    }

    /// <summary>
    /// Downloads a file from the specified URL to the specified output path,
    /// displaying a live progress bar in the console during the download.
    /// </summary>
    /// <param name="url">The URL of the file to download.</param>
    /// <param name="outputPath">The local file path to save the downloaded file.</param>
    /// <returns>A task that represents the asynchronous download operation.</returns>
    /// <exception cref="InvalidOperationException">Thrown when the server does not provide a valid Content-Length header.</exception>
    public async Task DownloadAsync(string url, string outputPath)
    {
        using var response = await _httpClient.GetAsync(url, HttpCompletionOption.ResponseHeadersRead);
        response.EnsureSuccessStatusCode();

        var contentLength = response.Content.Headers.ContentLength;

        if (contentLength is null or <= 0)
            throw new InvalidOperationException("Content-Length header is missing or invalid.");

        var totalBytes = contentLength.Value;
        var progressBar = new ProgressBar(totalBytes, _barLength, _progressChar, _prefix);

        await using var inputStream = await response.Content.ReadAsStreamAsync();
        await using var outputStream = new FileStream(outputPath, FileMode.Create, FileAccess.Write, FileShare.None, bufferSize: 8192, useAsync: true);

        var buffer = new byte[8192];
        long totalRead = 0;
        int read;

        while ((read = await inputStream.ReadAsync(buffer.AsMemory(0, buffer.Length))) > 0)
        {
            await outputStream.WriteAsync(buffer.AsMemory(0, read));
            totalRead += read;
            progressBar.Update(totalRead);
        }

        progressBar.Finish();
    }
}
