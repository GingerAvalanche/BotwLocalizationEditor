using Avalonia.Controls;
using BotwLocalizationEditor.Models;
using BotwLocalizationEditor.Views;
using ReactiveUI;

namespace BotwLocalizationEditor.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        private string chosenFolder;
        public UserControl[] languageControls;
        private UserControl selectedLanguageControl;
        private int languageSelector;
        public string ChosenFolder { get => chosenFolder; }
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
            chosenFolder = folder;
            if (chosenFolder != null)
            {
                LanguageModel model = new(folder);
                foreach (UserControl languageControl in languageControls)
                {
                    ((IFolderChoosable)languageControl.DataContext!).OnFolderChosen(model);
                }
            }
        }

        public void OnLanguageSelected(int param)
        {
            languageSelector = param;
            SelectedLanguageControl = languageControls[param];
        }

        public MainWindowViewModel()
        {
            chosenFolder = "";
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
