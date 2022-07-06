using Avalonia.Controls;
using BotwLocalizationEditor.ViewModels;

namespace BotwLocalizationEditor.Views
{
    public class LanguageControlBase : UserControl
    {
        protected async void AddMsbtButton_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            var dialog = MessageBox.Avalonia.MessageBoxManager.GetMessageBoxInputWindow(
                new MessageBox.Avalonia.DTO.MessageBoxInputParams()
                {
                    ContentTitle = "New MSBT file",
                    ContentMessage = "Input the name of the MSBT file to add:",
                    WatermarkText = "NameOfFile.msbt",
                    ButtonDefinitions = new[]
                    {
                        new MessageBox.Avalonia.Models.ButtonDefinition() { IsDefault = true },
                        new MessageBox.Avalonia.Models.ButtonDefinition() { Name = "Cancel", IsCancel = true },
                    },
                });
            MessageBox.Avalonia.DTO.MessageWindowResultDTO result = await dialog.ShowDialog((Window)VisualRoot!);
            if (result.Button == "Cancel")
            {
                return;
            }
            (DataContext as LanguageViewModelBase)!.AddMsbt(result.Message);
        }
        protected async void AddMsbtKeyButton_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            var dialog = MessageBox.Avalonia.MessageBoxManager.GetMessageBoxInputWindow(
                new MessageBox.Avalonia.DTO.MessageBoxInputParams()
                {
                    ContentTitle = "New MSBT key",
                    ContentMessage = "Input the name of the MSBT key to add:",
                    WatermarkText = "Name_Of_Key",
                    ButtonDefinitions = new[]
                    {
                        new MessageBox.Avalonia.Models.ButtonDefinition() { IsDefault = true },
                        new MessageBox.Avalonia.Models.ButtonDefinition() { Name = "Cancel", IsCancel = true },
                    },
                });
            MessageBox.Avalonia.DTO.MessageWindowResultDTO result = await dialog.ShowDialog((Window)VisualRoot!);
            if (result.Button == "Cancel")
            {
                return;
            }
            (DataContext as LanguageViewModelBase)!.AddMsbtKey(result.Message);
        }
    }
}
