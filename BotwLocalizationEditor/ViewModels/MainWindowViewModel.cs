using Avalonia.Controls;
using BotwLocalizationEditor.Interfaces;
using BotwLocalizationEditor.Models;
using BotwLocalizationEditor.Views;
using ReactiveUI;
using System.IO;
using System.Linq;

namespace BotwLocalizationEditor.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        private string loadFolder;
        private readonly UserControl[] languageControls;
        private UserControl selectedLanguageControl;
        private int languageSelector;
        public string LoadFolder { get => loadFolder; }
        public UserControl SelectedLanguageControl { get => selectedLanguageControl; set { this.RaiseAndSetIfChanged(ref selectedLanguageControl, value); } }
        public bool IsSingleLanguage { get => languageSelector == 0; }
        public bool IsDualLanguage { get => languageSelector == 1; }
        public bool IsOmniLanguage { get => languageSelector == 2; }

        public async void OnOpen()
        {
            OpenFolderDialog dialog = new()
            {
                Title = "Select the content/Pack folder of your mod",
            };
            string? folder = await dialog.ShowAsync(new Window());
            if (!string.IsNullOrEmpty(folder))
            {
                OnFolderChosen(folder);
            }
        }

        public void OnFolderChosen(string folder)
        {
            loadFolder = folder;
            LanguageModel model = new(folder);
            foreach (UserControl languageControl in languageControls)
            {
                ((IUpdatable)languageControl).Update(model.GetLangs());
                ((LanguageViewModelBase)languageControl.DataContext!).OnFolderChosen(model);
            }
        }

        public async void OnSave(bool saveAs)
        {
            string folder;
            LanguageViewModelBase vm = (SelectedLanguageControl.DataContext as LanguageViewModelBase)!;
            if (saveAs)
            {
                OpenFolderDialog dialog = new()
                {
                    Title = "Select the content/Pack folder of your mod",
                };
                string? maybeFolder = await dialog.ShowAsync(new Window());
                if (maybeFolder == null)
                {
                    return;
                }
                folder = maybeFolder!;
                string[] existingFiles = Directory.GetFiles(folder);
                foreach (string lang in vm.Languages)
                {
                    string temp = $"Bootup_{lang}.pack";
                    if (existingFiles.Contains(temp))
                    {
                        var messageBoxStandardWindow = MessageBox.Avalonia.MessageBoxManager.GetMessageBoxStandardWindow(
                            new MessageBox.Avalonia.DTO.MessageBoxStandardParams()
                            {
                                ButtonDefinitions = MessageBox.Avalonia.Enums.ButtonEnum.OkCancel,
                                ContentTitle = "Save As...",
                                ContentHeader = "Files already exist in the destination!",
                                ContentMessage = "Saving will overwrite existing files in the selected folder. Continue?",
                            });
                        MessageBox.Avalonia.Enums.ButtonResult result = await messageBoxStandardWindow.ShowDialog(new Window());
                        if (result == MessageBox.Avalonia.Enums.ButtonResult.Cancel)
                        {
                            return;
                        }
                        else
                        {
                            break;
                        }
                    }
                }
            }
            else
            {
                folder = loadFolder;
            }
            vm.SaveFiles(folder);
        }

        public void OnLanguageSelected(int param)
        {
            languageSelector = param;
            SelectedLanguageControl = languageControls[param];
        }

        public MainWindowViewModel()
        {
            loadFolder = "";
            languageSelector = 0;

            languageControls = new UserControl[]
            {
                new SingleLanguageControl(),
                new DualLanguageControl(),
                new OmniLanguageControl(),
            };

            selectedLanguageControl = languageControls[languageSelector];
        }
    }
}
