using Avalonia.Controls;
using BotwLocalizationEditor.Models;
using BotwLocalizationEditor.Views;
using ReactiveUI;
using System.Collections.Generic;

namespace BotwLocalizationEditor.ViewModels
{
    public class DualLanguageViewModel : LanguageViewModelBase
    {
        private readonly string[] chosenLanguages;
        private readonly string[] locTexts;
        private readonly BrowserControl[] langBrowsers;
        public string Lang0
        {
            get => chosenLanguages[0];
            set
            {
                this.RaiseAndSetIfChanged(ref chosenLanguages[0], value);
                if (!(string.IsNullOrEmpty(value) ||
                    string.IsNullOrEmpty(ChosenMsbtFolder) ||
                    string.IsNullOrEmpty(ChosenMsbtName) ||
                    string.IsNullOrEmpty(ChosenMsbtKey)))
                {
                    LocText0 = Model.GetOneLangMsbtValue(value, ChosenMsbtFolder, ChosenMsbtName, ChosenMsbtKey);
                }
            }
        }

        public UserControl LangBrowser0
        {
            get => langBrowsers[0];
            set => this.RaiseAndSetIfChanged(ref langBrowsers[0], (BrowserControl)value);
        }
        public string Lang1
        {
            get => chosenLanguages[1];
            set
            {
                this.RaiseAndSetIfChanged(ref chosenLanguages[1], value);
                if (!(string.IsNullOrEmpty(value) ||
                    string.IsNullOrEmpty(ChosenMsbtFolder) ||
                    string.IsNullOrEmpty(ChosenMsbtName) ||
                    string.IsNullOrEmpty(ChosenMsbtKey)))
                {
                    LocText1 = Model.GetOneLangMsbtValue(value, ChosenMsbtFolder, ChosenMsbtName, ChosenMsbtKey);
                }
            }
        }

        public UserControl LangBrowser1
        {
            get => langBrowsers[1];
            set => this.RaiseAndSetIfChanged(ref langBrowsers[1], (BrowserControl)value);
        }
        public string LocText0
        {
            get => locTexts[0];
            set => this.RaiseAndSetIfChanged(ref locTexts[0], value);
        }
        public string LocText1
        {
            get => locTexts[1];
            set => this.RaiseAndSetIfChanged(ref locTexts[1], value);
        }

        public DualLanguageViewModel()
        {
            chosenLanguages =
            [
                string.Empty,
                string.Empty,
            ];
            locTexts =
            [
                string.Empty,
                string.Empty,
            ];
            langBrowsers =
            [
                new(Lang0_SelectionChanged),
                new(Lang1_SelectionChanged),
            ];
            langBrowsers[0].AddButton.IsVisible = false;
            langBrowsers[1].AddButton.IsVisible = false;
        }

        private void Lang0_SelectionChanged(object? sender, SelectionChangedEventArgs e)
        {
            if (sender is ListBox { SelectedItem: not null } box)
            {
                Lang0 = (string)box.SelectedItem;
            }
        }

        private void Lang1_SelectionChanged(object? sender, SelectionChangedEventArgs e)
        {
            if (sender is ListBox { SelectedItem: not null } box)
            {
                Lang1 = (string)box.SelectedItem;
            }
        }

        public override void OnFolderChosen(LanguageModel languageModel)
        {
            SortedSet<string> sortedLanguages = languageModel.GetSortedLangs();
            langBrowsers[0].Items = sortedLanguages;
            langBrowsers[0].ItemList.SelectedIndex = 0;
            langBrowsers[1].Items = sortedLanguages;
            langBrowsers[1].ItemList.SelectedIndex = sortedLanguages.Count > 1 ? 1 : 0;
            this.RaiseAndSetIfChanged(ref chosenLanguages[0], (string)langBrowsers[0].ItemList.SelectedItem!, nameof(Lang0));
            this.RaiseAndSetIfChanged(ref chosenLanguages[1], (string)langBrowsers[1].ItemList.SelectedItem!, nameof(Lang1));
            base.OnFolderChosen(languageModel);
        }

        protected override void OnKeyChanged(string key)
        {
            LocText0 = Model.GetOneLangMsbtValue(Lang0, ChosenMsbtFolder, ChosenMsbtName, key);
            LocText1 = Model.GetOneLangMsbtValue(Lang1, ChosenMsbtFolder, ChosenMsbtName, key);
        }

        internal void SaveLoc(int langNum)
        {
            Model.SetOneLangMsbtValue(chosenLanguages[langNum], ChosenMsbtFolder, ChosenMsbtName, ChosenMsbtKey, locTexts[langNum]);
        }
    }
}
