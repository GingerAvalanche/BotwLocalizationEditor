using Avalonia;
using Avalonia.ReactiveUI;
using log4net;
using ReactiveUI;
using System;
using System.Diagnostics;
using System.Reactive.Concurrency;
using System.Threading.Tasks;

namespace BotwLocalizationEditor
{
    internal class Program
    {
        // Initialization code. Don't use any Avalonia, third-party APIs or any
        // SynchronizationContext-reliant code before AppMain is called: things aren't initialized
        // yet and stuff might break.
        [STAThread]
        public static void Main(string[] args)
        {
            AppDomain currentDomain = AppDomain.CurrentDomain;
            // Handler for unhandled exceptions.
            currentDomain.UnhandledException += GlobalUnhandledExceptionHandler;
            // Handler for exceptions in threads behind forms.
            TaskScheduler.UnobservedTaskException += GlobalThreadExceptionHandler;
            RxApp.DefaultExceptionHandler = new BleObservableExceptionHandler();
            BuildAvaloniaApp().StartWithClassicDesktopLifetime(args);
        }

        // Avalonia configuration, don't remove; also used by visual designer.
        private static AppBuilder BuildAvaloniaApp()
            => AppBuilder.Configure<App>()
                .UsePlatformDetect()
                .WithInterFont()
                .LogToTrace()
                .UseReactiveUI();

        private static void GlobalUnhandledExceptionHandler(object sender, UnhandledExceptionEventArgs e)
        {
            Exception ex = (Exception)e.ExceptionObject;
            ILog log = LogManager.GetLogger(typeof(Program));
            log.Error($"{ex.Message}\n{ex.StackTrace}");
        }

        private static void GlobalThreadExceptionHandler(object? sender, UnobservedTaskExceptionEventArgs e)
        {
            Exception ex = e.Exception;
            ILog log = LogManager.GetLogger(typeof(Program)); //Log4NET
            log.Error($"{ex.Message}\n{ex.StackTrace}");
        }
    }
    public class BleObservableExceptionHandler : IObserver<Exception>
    {
        public void OnNext(Exception value)
        {
            if (Debugger.IsAttached) Debugger.Break();


            ILog log = LogManager.GetLogger(typeof(Program)); //Log4NET
            log.Error($"{value.Message}\n{value.StackTrace}");

            RxApp.MainThreadScheduler.Schedule(() => throw value);
        }

        public void OnError(Exception error)
        {
            if (Debugger.IsAttached) Debugger.Break();

            ILog log = LogManager.GetLogger(typeof(Program)); //Log4NET
            log.Error($"{error.Message}\n{error.StackTrace}");

            RxApp.MainThreadScheduler.Schedule(() => throw error);
        }

        public void OnCompleted()
        {
            if (Debugger.IsAttached) Debugger.Break();
            //RxApp.MainThreadScheduler.Schedule(() => throw new NotImplementedException());
        }
    }
}
