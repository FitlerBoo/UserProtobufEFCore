using Avalonia;
using Avalonia.Controls;
using Avalonia.Threading;
using System;
using System.Threading.Tasks;

public static class WindowOpener
{
    public static async Task OpenWindow(Window window)
    {
        await Dispatcher.UIThread.InvokeAsync(() =>
        {
            window.Show();
        }, DispatcherPriority.Normal);
    }
}
