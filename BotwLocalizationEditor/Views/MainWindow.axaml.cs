using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using BotwLocalizationEditor.ViewModels;
using MessageBox.Avalonia.Enums;

namespace BotwLocalizationEditor.Views
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            Open.Click += Open_Click;
            Save.Click += Save_Click;
            SaveAs.Click += SaveAs_Click;
            Exit.Click += Exit_Click;
        }

        private void Exit_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            if (Avalonia.Application.Current!.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktopLifetime)
            {
                desktopLifetime.Shutdown();
            }
        }

        private async void SaveAs_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            string folder;
            MainWindowViewModel vm = (DataContext as MainWindowViewModel)!;
            OpenFolderDialog dialog = new()
            {
                Title = "Select the content/Pack folder of your mod",
            };
            string? maybeFolder = await dialog.ShowAsync(this);
            if (maybeFolder == null)
            {
                return;
            }
            folder = maybeFolder!;
            if (vm.WillSaveOverwriteFile(folder))
            {
                var messageBoxStandardWindow = MessageBox.Avalonia.MessageBoxManager.GetMessageBoxStandardWindow(
                    new MessageBox.Avalonia.DTO.MessageBoxStandardParams()
                    {
                        ButtonDefinitions = ButtonEnum.OkCancel,
                        ContentTitle = "Save As...",
                        ContentHeader = "Files already exist in the destination!",
                        ContentMessage = "Saving will overwrite existing localization files in the selected folder. Continue?",
                    });
                ButtonResult result = await messageBoxStandardWindow.ShowDialog(this);
                if (result == ButtonResult.Cancel)
                {
                    return;
                }
            }
            vm.SaveFiles(folder);
        }

        private void Save_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            (DataContext as MainWindowViewModel)!.SaveFiles();
        }

        private async void Open_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            OpenFolderDialog dialog = new()
            {
                Title = "Select the content/Pack folder of your mod",
            };
            string? folder = await dialog.ShowAsync(this);
            if (!string.IsNullOrEmpty(folder))
            {
                (DataContext as MainWindowViewModel)!.OpenFolder(folder);
            }
        }
    }
}
