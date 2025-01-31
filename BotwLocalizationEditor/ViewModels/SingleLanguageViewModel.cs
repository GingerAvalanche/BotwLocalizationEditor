using Avalonia.Controls;
using BotwLocalizationEditor.Models;
using BotwLocalizationEditor.Views;
using ReactiveUI;

namespace BotwLocalizationEditor.ViewModels
{
    public class SingleLanguageViewModel : LanguageViewModelBase
    {
        private string chosenLanguage;
        private string locText;

        public string ChosenLanguage
        {
            get => chosenLanguage;
            set
            {
                this.RaiseAndSetIfChanged(ref chosenLanguage, value);
                if (!(string.IsNullOrEmpty(value) ||
                    string.IsNullOrEmpty(ChosenMsbtFolder) ||
                    string.IsNullOrEmpty(ChosenMsbtName) ||
                    string.IsNullOrEmpty(ChosenMsbtKey)))
                {
                    LocText = Model.GetOneLangMsbtValue(value, ChosenMsbtFolder, ChosenMsbtName, ChosenMsbtKey);
                }
            }
        }

        private BrowserControl LanguageBrowser { get; }

        public SingleLanguageViewModel()
        {
            chosenLanguage = string.Empty;
            locText = string.Empty;
            LanguageBrowser = new(Lang_SelectionChanged)
            {
                AddButton =
                {
                    IsVisible = false
                }
            };
        }

        private void Lang_SelectionChanged(object? sender, SelectionChangedEventArgs e)
        {
            if (sender is not ListBox { SelectedItem: not null } box) return;
            ChosenLanguage = (string)box.SelectedItem;
        }

        public string LocText
        {
            get => locText;
            set => this.RaiseAndSetIfChanged(ref locText, value);
        }

        public void OnLangTabSelected()
        {
            SelectedBrowserControl = LanguageBrowser;
        }

        public override void OnFolderChosen(LanguageModel languageModel)
        {
            LanguageBrowser.Items = languageModel.GetSortedLanguages();
            this.RaiseAndSetIfChanged(ref chosenLanguage, (string)LanguageBrowser.ItemList.SelectedItem!, nameof(ChosenLanguage));
            base.OnFolderChosen(languageModel);
        }

        protected override void OnKeyChanged(string key)
        {
            LocText = Model.GetOneLangMsbtValue(chosenLanguage, ChosenMsbtFolder, ChosenMsbtName, key);
        }

        internal void SaveLoc()
        {
            Model.SetOneLangMsbtValue(chosenLanguage, ChosenMsbtFolder, ChosenMsbtName, ChosenMsbtKey, locText);
        }
    }
}
