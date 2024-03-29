using Avalonia.Controls;
using BotwLocalizationEditor.Interfaces;
using BotwLocalizationEditor.Models;
using BotwLocalizationEditor.Views;
using ReactiveUI;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace BotwLocalizationEditor.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        private string title = "BotwLocalizationEditor";
        private string loadFolder;
        private readonly UserControl[] languageControls;
        private UserControl selectedLanguageControl;
        private int languageSelector;
        public string Title { get => title; set => this.RaiseAndSetIfChanged(ref title, value); }
        public string LoadFolder { get => loadFolder; }
        public UserControl SelectedLanguageControl { get => selectedLanguageControl; set { this.RaiseAndSetIfChanged(ref selectedLanguageControl, value); } }
        public bool IsSingleLanguage { get => languageSelector == 0; }
        public bool IsDualLanguage { get => languageSelector == 1; }
        public bool IsOmniLanguage { get => languageSelector == 2; }

        public void OnLanguageModeSelected(int param)
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

        public void OpenFolder(string folder)
        {
            string mod = Path.GetFileName(folder);
            Title = $"BotwLocalizationEditor - {mod}";
            loadFolder = $"{folder}/content/Pack";
            LanguageModel model = new(loadFolder);
            foreach (UserControl languageControl in languageControls)
            {
                ((IUpdatable)languageControl).Update(model.GetLangs());
                ((LanguageViewModelBase)languageControl.DataContext!).OnFolderChosen(model);
            }
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
