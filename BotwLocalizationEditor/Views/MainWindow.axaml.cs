using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Platform;
using Avalonia.Platform.Storage;
using BotwLocalizationEditor.ViewModels;
using MsBox.Avalonia;
using MsBox.Avalonia.Dto;
using MsBox.Avalonia.Enums;
using System.Collections.Immutable;
using System.IO;
using System.Linq;

namespace BotwLocalizationEditor.Views
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            SettingsViewModel.InitSettingsFile();

            InitializeComponent();

            Open.Click += Open_Click;
            Save.Click += Save_Click;
            SaveAs.Click += SaveAs_Click;
            Settings.Click += Settings_Click;
            Exit.Click += Exit_Click;
            ScanNew.Click += ScanNew_Click;
            ScanMissingEmpty.Click += ScanMissingEmpty_Click;
            About.Click += About_Click;
        }

        private async void Settings_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            await new SettingsWindow().ShowDialog(this);
        }

        private async void Open_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            var selection = (await StorageProvider.OpenFolderPickerAsync(
                    new()
                    {
                        Title = "Select the root folder of your mod",
                        AllowMultiple = false,
                    }
                ));
            if (selection.Count != 1)
            {
                return;
            }
            string folder = System.Uri.UnescapeDataString(selection[0].Path.AbsolutePath);
            if (!string.IsNullOrEmpty(folder))
            {
                (DataContext as MainWindowViewModel)!.OpenFolder(folder);
            }
        }

        private void Save_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            (DataContext as MainWindowViewModel)!.SaveFiles();
        }

        private async void SaveAs_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            var selection = (await StorageProvider.OpenFolderPickerAsync(
                    new()
                    {
                        Title = "Select the root folder of your mod",
                        AllowMultiple = false,
                    }
                ));
            if (selection.Count != 1)
            {
                return;
            }
            string folder = Path.Combine(System.Uri.UnescapeDataString(selection[0].Path.AbsolutePath), "content", "Pack");
            MainWindowViewModel vm = (DataContext as MainWindowViewModel)!;
            if (vm.WillSaveOverwriteFile(folder))
            {
                var messageBoxStandardWindow = MessageBoxManager.GetMessageBoxStandard(
                    new MessageBoxStandardParams()
                    {
                        ButtonDefinitions = ButtonEnum.OkCancel,
                        ContentTitle = "Save As...",
                        ContentHeader = "Files already exist in the destination!",
                        ContentMessage = "Saving will overwrite existing localization files in the selected folder. Continue?",
                    });
                ButtonResult result = await messageBoxStandardWindow.ShowWindowDialogAsync(this);
                if (result == ButtonResult.Cancel)
                {
                    return;
                }
            }
            vm.SaveFiles(folder);
        }

        private void Exit_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            if (Avalonia.Application.Current!.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktopLifetime)
            {
                desktopLifetime.Shutdown();
            }
        }

        private async void ScanMissingEmpty_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            MainWindowViewModel vm = (DataContext as MainWindowViewModel)!;
            var missing = vm.ScanForMissing();
            string message;
            if (missing.Count == 0)
            {
                message = "None missing!";
            }
            else
            {
                message = string.Join("\n", missing.Select(l => $"{l.Key}:\n\t{string.Join("\n\t", l.Value.Select(f => $"{f.Key}:\n\t\t{string.Join("\n\t\t", f.Value.Select(fi => $"{fi.Key}:\n\t\t\t{string.Join("\n\t\t\t", fi.Value.Keys.ToImmutableSortedSet())}").ToImmutableSortedSet())}").ToImmutableSortedSet())}").ToImmutableSortedSet());
            }
            await MessageBoxManager.GetMessageBoxStandard(
                new MessageBoxStandardParams()
                {
                    ButtonDefinitions = ButtonEnum.Ok,
                    ContentTitle = "Scan for Missing/Empty Keys",
                    ContentHeader = "These keys are either missing from the given language, or they contain no text for that language:",
                    ContentMessage = message,
                    MaxHeight = 800,
                    Width = 400,
                    WindowIcon = new("Assets/icon.png"),
                    WindowStartupLocation = WindowStartupLocation.CenterOwner,
                }).ShowWindowDialogAsync(this);
        }

        private async void ScanNew_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            MainWindowViewModel vm = (DataContext as MainWindowViewModel)!;
            var missing = vm.ScanForNew();
            string message;
            if (missing.Count == 0)
            {
                message = "No new keys!";
            }
            else
            {
                message = string.Join("\n", missing.Select(f => $"{f.Key}:\n\t{string.Join("\n\t", f.Value.Select(fi => $"{fi.Key}:\n\t\t{string.Join("\n\t\t", fi.Value.Keys.ToImmutableSortedSet())}").ToImmutableSortedSet())}").ToImmutableSortedSet());
            }
            await MessageBoxManager.GetMessageBoxStandard(
                new MessageBoxStandardParams()
                {
                    ButtonDefinitions = ButtonEnum.Ok,
                    ContentTitle = "Scan for New Keys",
                    ContentHeader = "These keys are new in your mod:",
                    ContentMessage = message,
                    MaxHeight = 800,
                    Width = 400,
                    WindowIcon = new("Assets/icon.png"),
                    WindowStartupLocation = WindowStartupLocation.CenterOwner,
                }).ShowWindowDialogAsync(this);
        }

        private async void About_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            string message;
            using (var streamReader = new StreamReader(AssetLoader.Open(new("avares://BotwLocalizationEditor/Assets/about.md"))))
            {
                message = streamReader.ReadToEnd();
            }
            await MessageBoxManager.GetMessageBoxStandard(
                new MessageBoxStandardParams()
                {
                    ButtonDefinitions = ButtonEnum.Ok,
                    ContentTitle = "About",
                    ContentMessage = message,
                    Markdown = true,
                    MaxHeight = 800,
                    Width = 500,
                    WindowIcon = new("Assets/icon.png"),
                    WindowStartupLocation = WindowStartupLocation.CenterOwner,
                }).ShowWindowDialogAsync(this);
        }
    }
}
