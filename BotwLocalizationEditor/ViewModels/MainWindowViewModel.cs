using Avalonia.Controls;
using BotwLocalizationEditor.Interfaces;
using BotwLocalizationEditor.Models;
using BotwLocalizationEditor.Views;
using OperationResult;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace BotwLocalizationEditor.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        private string title = "BotwLocalizationEditor";
        private string loadFolder = string.Empty;
        private readonly UserControl[] languageControls;
        private UserControl selectedLanguageControl;
        private int languageSelector;
        public string Title
        {
            get => title;
            set => this.RaiseAndSetIfChanged(ref title, value);
        }
        public string LoadFolder => loadFolder;

        public UserControl SelectedLanguageControl
        {
            get => selectedLanguageControl;
            set => this.RaiseAndSetIfChanged(ref selectedLanguageControl, value);
        }
        public bool IsSingleLanguage => languageSelector == 0;
        public bool IsDualLanguage => languageSelector == 1;
        public bool IsOmniLanguage => languageSelector == 2;

        public void OnLanguageModeSelected(int param)
        {
            languageSelector = param;
            SelectedLanguageControl = languageControls[param];
        }

        public MainWindowViewModel()
        {
            languageSelector = 0;

            languageControls = [
                new SingleLanguageControl(),
                new DualLanguageControl(),
                new OmniLanguageControl(),
            ];

            selectedLanguageControl = languageControls[languageSelector];
        }

        public Result<bool> OpenFolder(string folder)
        {
            string[] paths = [
                Path.Combine(folder, "content", "Pack"),
                Path.Combine(folder, "01007EF00011E000", "romfs", "Pack")
            ];
            string temp = paths.Where(Directory.Exists).FirstOrDefault() ?? string.Empty;
            if (temp == string.Empty)
            {
                return new InvalidOperationException($"Invalid root folder\n\nNeither of the following exist:\n{string.Join('\n', paths)}");
            }
            Title = $"BotwLocalizationEditor - {Path.GetFileName(folder)}";
            loadFolder = temp;
            LanguageModel model = new(loadFolder);
            foreach (UserControl languageControl in languageControls)
            {
                ((IUpdatable)languageControl).Update(model.GetLanguages());
                ((LanguageViewModelBase)languageControl.DataContext!).OnFolderChosen(model);
            }
            return true;
        }

        public bool WillSaveOverwriteFile(string folder)
        {
            LanguageViewModelBase vm = (SelectedLanguageControl.DataContext as LanguageViewModelBase)!;
            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
                return false;
            }
            string[] existingFiles = Directory.GetFiles(folder);
            foreach (string lang in vm.Languages)
            {
                string temp = Path.Combine(folder, $"Bootup_{lang}.pack");
                if (existingFiles.Contains(temp))
                {
                    return true;
                }
            }
            return false;
        }

        public void SaveFiles(string folder = "")
        {
            if (string.IsNullOrEmpty(folder))
            {
                folder = loadFolder;
            }
            (SelectedLanguageControl.DataContext as LanguageViewModelBase)!.SaveFiles(folder);
        }

        public Dictionary<string, Dictionary<string, Dictionary<string, Dictionary<string, bool>>>> ScanForMissing()
        {
            return ((LanguageViewModelBase)languageControls[0].DataContext!).ScanForMissing();
        }

        public Dictionary<string, Dictionary<string, Dictionary<string, bool>>> ScanForNew()
        {
            return ((LanguageViewModelBase)languageControls[0].DataContext!).ScanForNew();
        }
    }
}
