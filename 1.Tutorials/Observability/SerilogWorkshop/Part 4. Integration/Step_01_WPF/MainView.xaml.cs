using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Step_01_WPF
{
    /// <summary>
    /// Interaction logic for MainView.xaml
    /// </summary>
    public partial class MainView : Window
    {
        //
        // WriteTo.RichTextBox 동기화를 위해 필요하다.
        // 
        private static readonly object _syncRoot = new object();

        public MainView()
        {
            InitializeComponent();

            const string outputTemplate = "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj} {Properties:j}{NewLine}{Exception}";

            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Verbose()
                .WriteTo.RichTextBox(_richTextBox,
                    outputTemplate: outputTemplate,
                    syncRoot: _syncRoot)
                .Enrich.WithThreadId()
                .CreateLogger();
        }

        private void Clear_OnClick(object sender, RoutedEventArgs e)
        {
            lock (_syncRoot)
            {
                _richTextBox.Document.Blocks.Clear();
            }
        }

        private void LogVerbose_OnClick(object sender, RoutedEventArgs e)
        {
            Log.Verbose("Hello! Now => {Now}", DateTime.Now);
        }

        private void LogDebug_OnClick(object sender, RoutedEventArgs e)
        {
            Log.Debug("Hello! Now => {Now}", DateTime.Now);
        }

        private void LogInformation_OnClick(object sender, RoutedEventArgs e)
        {
            Log.Information("Hello! Now => {Now}", DateTime.Now);
        }

        private void LogWarning_OnClick(object sender, RoutedEventArgs e)
        {
            Log.Warning("Hello! Now => {Now}", DateTime.Now);
        }

        private void LogError_OnClick(object sender, RoutedEventArgs e)
        {
            Log.Error("Hello! Now => {Now}", DateTime.Now);
        }

        private void LogFatal_OnClick(object sender, RoutedEventArgs e)
        {
            Log.Fatal("Hello! Now => {Now}", DateTime.Now);
        }

        private void LogParallelFor_OnClick(object sender, RoutedEventArgs e)
        {
            Parallel.For(1, 101, stepNumber =>
            {
                var stepName = FormattableString.Invariant($"Step {stepNumber:000}");

                Log.Verbose("Hello from Parallel.For({StepName}) Verbose", stepName);
                Log.Debug("Hello from Parallel.For({StepName}) Debug", stepName);
                Log.Information("Hello from Parallel.For({StepName}) Information", stepName);
                Log.Warning("Hello from Parallel.For({StepName}) Warning", stepName);
                Log.Error("Hello from Parallel.For({StepName}) Error", stepName);
                Log.Fatal("Hello from Parallel.For({StepName}) Fatal", stepName);
            });
        }

        private async void LogTaskRun_OnClick(object sender, RoutedEventArgs e)
        {
            var tasks = new System.Collections.Generic.List<Task>();

            for (var i = 1; i <= 100; i++)
            {
                var stepNumber = i;
                var task = Task.Run(() =>
                {
                    var stepName = FormattableString.Invariant($"Step {stepNumber:000}");

                    Log.Verbose("Hello from Task.Run({StepName}) Verbose", stepName);
                    Log.Debug("Hello from Task.Run({StepName}) Debug", stepName);
                    Log.Information("Hello from Task.Run({StepName}) Information", stepName);
                    Log.Warning("Hello from Task.Run({StepName}) Warning", stepName);
                    Log.Error("Hello from Task.Run({StepName}) Error", stepName);
                    Log.Fatal("Hello from Task.Run({StepName}) Fatal", stepName);
                });

                tasks.Add(task);
            }

            await Task.WhenAll(tasks);
        }
    }
}
