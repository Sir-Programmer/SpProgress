namespace Example;

public abstract class Program
{
    private static async Task Main()
    {
        // Example usage of FileDownloadProgress
        var fileDownloadProgress = new SpProgress.FileDownloadProgress();
        await fileDownloadProgress.DownloadAsync("https://karamlou.com/assets/images/GN.gif", "GN.gif");

        // Example usage of CopyFileProgress
        var copyFileProgress = new SpProgress.CopyFileProgress();
        await copyFileProgress.CopyAsync("source.txt", "destination.txt");

        // Example usage of ProgressBar
        var progressBar = new SpProgress.ProgressBar(100, prefix: "Loading: ");
        for (var i = 0; i <= 100; i++)
        {
            progressBar.Update(i);
            await Task.Delay(50);
        }
        progressBar.Finish();
        
        await Task.Delay(-1);
    }
}