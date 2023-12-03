using Avalonia.Controls;
using Avalonia.Platform.Storage;
using BotwLocalizationEditor.ViewModels;
using System;
using System.IO;

namespace BotwLocalizationEditor.Views
{
    public partial class SettingsWindow : Window
    {
        public SettingsWindow()
        {
            WindowStartupLocation = WindowStartupLocation.CenterOwner;
            InitializeComponent();

            DataContext = new SettingsViewModel();

            DumpBox.TextChanged += DumpBox_TextChanged;
            BrowseButton.Click += BrowseButton_Click;
            SaveButton.Click += SaveButton_Click;
            CancelButton.Click += CancelButton_Click;

            Width = 600;
            Height = 250;
        }

        private void DumpBox_TextChanged(object? sender, TextChangedEventArgs e)
        {
            string path = Path.Combine(DumpBox.Text!, "content", "Pack");
            if (Directory.Exists(path))
            {
                foreach (var file in Directory.EnumerateFiles(path))
                {
                    if (File.Exists(file) && Path.GetFileNameWithoutExtension(file).StartsWith("Bootup_"))
                    {
                        DumpInvalid.IsVisible = false;
                        DumpValid.IsVisible = true;
                        return;
                    }
                }
            }
            DumpValid.IsVisible = false;
            DumpInvalid.IsVisible = true;
        }

        private async void BrowseButton_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            IStorageFolder maybeFolder = (await StorageProvider.OpenFolderPickerAsync(
                    new()
                    {
                        Title = "Select the root folder of your game dump",
                        AllowMultiple = false,
                    }
                ))[0];
            string folder = System.Uri.UnescapeDataString(maybeFolder.Path.AbsolutePath);
            ((SettingsViewModel)DataContext!).DumpPath = folder;
        }

        private void SaveButton_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            ((SettingsViewModel)DataContext!).SaveSettings();
            Close();
        }

        private void CancelButton_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            Close();
        }
    }
}
