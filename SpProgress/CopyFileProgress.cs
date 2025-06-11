using System.Text;

namespace SpProgress
{
    /// <summary>
    /// Provides functionality to copy a file asynchronously while displaying a live progress bar in the console.
    /// </summary>
    public class CopyFileProgress
    {
        private readonly int _barLength;
        private readonly char _progressChar;
        private readonly string _prefix;

        /// <summary>
        /// Initializes a new instance of the <see cref="CopyFileProgress"/> class.
        /// </summary>
        /// <param name="barLength">Length of the progress bar (number of characters).</param>
        /// <param name="progressChar">Character used to display the filled portion of the progress bar.</param>
        /// <param name="prefix">Optional prefix string displayed before the progress bar.</param>
        public CopyFileProgress(int barLength = 50, char progressChar = '█', string prefix = "Copying: ")
        {
            _barLength = barLength;
            _progressChar = progressChar;
            _prefix = prefix;

            Console.OutputEncoding = Encoding.UTF8;
        }

        /// <summary>
        /// Copies a file asynchronously from <paramref name="sourcePath"/> to <paramref name="destinationPath"/>,
        /// displaying a live progress bar in the console during the copy operation.
        /// </summary>
        /// <param name="sourcePath">The full path of the source file.</param>
        /// <param name="destinationPath">The full path where the file will be copied.</param>
        /// <param name="bufferSize">The size of the buffer used for reading and writing (default is 8192 bytes).</param>
        /// <returns>A task that represents the asynchronous copy operation.</returns>
        /// <exception cref="FileNotFoundException">Thrown if the source file does not exist.</exception>
        public async Task CopyAsync(string sourcePath, string destinationPath, int bufferSize = 8192)
        {
            if (!File.Exists(sourcePath))
                throw new FileNotFoundException("Source file not found.", sourcePath);

            var destDirectory = Path.GetDirectoryName(destinationPath);
            if (!string.IsNullOrWhiteSpace(destDirectory) && !Directory.Exists(destDirectory))
                Directory.CreateDirectory(destDirectory);

            var totalBytes = new FileInfo(sourcePath).Length;
            var progressBar = new ProgressBar(totalBytes, _barLength, _progressChar, _prefix);

            await using var sourceStream = new FileStream(sourcePath, FileMode.Open, FileAccess.Read, FileShare.Read, bufferSize, useAsync: true);
            await using var destinationStream = new FileStream(destinationPath, FileMode.Create, FileAccess.Write, FileShare.None, bufferSize, useAsync: true);

            var buffer = new byte[bufferSize];
            long totalRead = 0;
            int bytesRead;

            while ((bytesRead = await sourceStream.ReadAsync(buffer.AsMemory(0, buffer.Length))) > 0)
            {
                await destinationStream.WriteAsync(buffer.AsMemory(0, bytesRead));
                totalRead += bytesRead;
                progressBar.Update(totalRead);
            }

            progressBar.Finish();
        }
    }
}
