using System;
using System.Threading.Tasks;
using Avalonia.Controls;
using MsBox.Avalonia;
using MsBox.Avalonia.Dto;
using MsBox.Avalonia.Enums;
using BotwLocalizationEditor.ViewModels;

namespace BotwLocalizationEditor.Views
{
    public class LanguageControlBase : UserControl
    {
        protected async void AddMsbtButton_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            try
            {
                var dialog = MessageBoxManager.GetMessageBoxStandard(
                    new MessageBoxStandardParams()
                    {
                        ContentTitle = "New MSBT file",
                        ContentMessage = "Input the name of the MSBT file to add:",
                        InputParams = new() {
                            Label = "NameOfFile.msbt",
                        },
                        ButtonDefinitions = ButtonEnum.OkCancel,
                    });
                var result = await dialog.ShowWindowDialogAsync((Window)VisualRoot!);
                if (result == ButtonResult.Cancel)
                {
                    return;
                }
                (DataContext as LanguageViewModelBase)!.AddMsbt(dialog.InputValue);
            }
            catch (Exception ex)
            {
                await ErrorDialog(ex);
            }
        }

        protected async void AddMsbtKeyButton_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            try
            {
                var dialog = MessageBoxManager.GetMessageBoxStandard(
                    new MessageBoxStandardParams()
                    {
                        ContentTitle = "New MSBT key",
                        ContentMessage = "Input the name of the MSBT key to add:",
                        InputParams = new()
                        {
                            Label = "Name_Of_Key",
                        },
                        ButtonDefinitions = ButtonEnum.OkCancel,
                    });
                var result = await dialog.ShowWindowDialogAsync((Window)VisualRoot!);
                if (result == ButtonResult.Cancel)
                {
                    return;
                }

                (DataContext as LanguageViewModelBase)!.AddMsbtKey(dialog.InputValue);
            }
            catch (Exception ex)
            {
                await ErrorDialog(ex);
            }
        }

        private async Task ErrorDialog(Exception ex)
        {
            if (TopLevel.GetTopLevel(this) is Window topLevel)
            {
                await MessageBoxManager.GetMessageBoxStandard(
                    new()
                    {
                        ButtonDefinitions = ButtonEnum.Ok,
                        ContentTitle = "Error",
                        ContentMessage = ex.Message,
                        MaxHeight = 800,
                        Width = 500,
                        WindowStartupLocation = WindowStartupLocation.CenterOwner,
                    }).ShowWindowDialogAsync(topLevel);
            }
        }
    }
}
