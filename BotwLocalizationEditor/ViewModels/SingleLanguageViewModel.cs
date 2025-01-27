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
                    string.IsNullOrEmpty(chosenMsbtFolder) ||
                    string.IsNullOrEmpty(chosenMsbtName) ||
                    string.IsNullOrEmpty(chosenMsbtKey)))
                {
                    LocText = model.GetOneLangMsbtValue(value, chosenMsbtFolder, chosenMsbtName, chosenMsbtKey);
                }
            }
        }
        public BrowserControl LanguageBrowser { get; }

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
            LanguageBrowser.Items = languageModel.GetSortedLangs();
            this.RaiseAndSetIfChanged(ref chosenLanguage, (string)LanguageBrowser.ItemList.SelectedItem!, nameof(ChosenLanguage));
            base.OnFolderChosen(languageModel);
        }

        protected override void OnKeyChanged(string key)
        {
            LocText = model.GetOneLangMsbtValue(chosenLanguage, chosenMsbtFolder, chosenMsbtName, key);
        }

        internal void SaveLoc()
        {
            model.SetOneLangMsbtValue(chosenLanguage, chosenMsbtFolder, chosenMsbtName, chosenMsbtKey, locText);
        }
    }
}
