using System;
using System.Text;

namespace SpProgress;

public class ProgressBar
{
    private readonly long _total;
    private readonly int _barLength;
    private readonly char _progressChar;
    private readonly string _prefix;
    private int _lastRenderedPercent = -1;

    public ProgressBar(long total, int barLength = 50, char progressChar = '█', string prefix = "")
    {
        if (total <= 0)
            throw new ArgumentOutOfRangeException(nameof(total), "Total must be greater than zero.");

        _total = total;
        _barLength = barLength;
        _progressChar = progressChar;
        _prefix = prefix;
        Console.OutputEncoding = Encoding.UTF8;
    }

    /// <summary>
    /// Updates the progress bar in the console.
    /// </summary>
    /// <param name="current">The current progress value.</param>
    public void Update(long current)
    {
        if (current < 0) current = 0;
        if (current > _total) current = _total;

        var percentage = (int)((double)current / _total * 100);
        if (percentage == _lastRenderedPercent) return; 
        _lastRenderedPercent = percentage;

        var filled = (int)(_barLength * percentage / 100.0);
        var empty = _barLength - filled;

        var barBuilder = new StringBuilder(_barLength);
        barBuilder.Append(_progressChar, filled);
        barBuilder.Append('-', empty);

        Console.Write($"\r{_prefix}[{barBuilder}] {percentage,3}%");
    }

    /// <summary>
    /// Completes the progress bar and moves to the next line.
    /// </summary>
    public void Finish()
    {
        Update(_total);
        Console.WriteLine();
    }
}