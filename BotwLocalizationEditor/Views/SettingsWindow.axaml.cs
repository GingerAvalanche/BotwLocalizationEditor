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
            DataContext = new SettingsViewModel();
            WindowStartupLocation = WindowStartupLocation.CenterOwner;

            InitializeComponent();

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
            string folder = Uri.UnescapeDataString(selection[0].Path.AbsolutePath);
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
