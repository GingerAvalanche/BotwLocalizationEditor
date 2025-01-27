using Avalonia.Controls;
using BotwLocalizationEditor.Models;
using BotwLocalizationEditor.Views;
using ReactiveUI;
using System.Collections.Generic;

namespace BotwLocalizationEditor.ViewModels
{
    public class DualLanguageViewModel : LanguageViewModelBase
    {
        private readonly string[] chosenLangs;
        private readonly string[] locTexts;
        private readonly BrowserControl[] langBrowsers;
        public string Lang0
        {
            get => chosenLangs[0];
            set
            {
                this.RaiseAndSetIfChanged(ref chosenLangs[0], value);
                if (!(string.IsNullOrEmpty(value) ||
                    string.IsNullOrEmpty(chosenMsbtFolder) ||
                    string.IsNullOrEmpty(chosenMsbtName) ||
                    string.IsNullOrEmpty(chosenMsbtKey)))
                {
                    LocText0 = model.GetOneLangMsbtValue(value, chosenMsbtFolder, chosenMsbtName, chosenMsbtKey);
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
            get => chosenLangs[1];
            set
            {
                this.RaiseAndSetIfChanged(ref chosenLangs[1], value);
                if (!(string.IsNullOrEmpty(value) ||
                    string.IsNullOrEmpty(chosenMsbtFolder) ||
                    string.IsNullOrEmpty(chosenMsbtName) ||
                    string.IsNullOrEmpty(chosenMsbtKey)))
                {
                    LocText1 = model.GetOneLangMsbtValue(value, chosenMsbtFolder, chosenMsbtName, chosenMsbtKey);
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
            chosenLangs =
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
        protected void Lang0_SelectionChanged(object? sender, SelectionChangedEventArgs e)
        {
            if (sender is ListBox { SelectedItem: not null } box)
            {
                Lang0 = (string)box.SelectedItem;
            }
        }
        protected void Lang1_SelectionChanged(object? sender, SelectionChangedEventArgs e)
        {
            if (sender is ListBox { SelectedItem: not null } box)
            {
                Lang1 = (string)box.SelectedItem;
            }
        }

        public override void OnFolderChosen(LanguageModel languageModel)
        {
            SortedSet<string> sortedLangs = languageModel.GetSortedLangs();
            langBrowsers[0].Items = sortedLangs;
            langBrowsers[0].ItemList.SelectedIndex = 0;
            langBrowsers[1].Items = sortedLangs;
            langBrowsers[1].ItemList.SelectedIndex = sortedLangs.Count > 1 ? 1 : 0;
            this.RaiseAndSetIfChanged(ref chosenLangs[0], (string)langBrowsers[0].ItemList.SelectedItem!, nameof(Lang0));
            this.RaiseAndSetIfChanged(ref chosenLangs[1], (string)langBrowsers[1].ItemList.SelectedItem!, nameof(Lang1));
            base.OnFolderChosen(languageModel);
        }

        protected override void OnKeyChanged(string key)
        {
            LocText0 = model.GetOneLangMsbtValue(Lang0, chosenMsbtFolder, chosenMsbtName, key);
            LocText1 = model.GetOneLangMsbtValue(Lang1, chosenMsbtFolder, chosenMsbtName, key);
        }

        internal void SaveLoc(int langNum)
        {
            model.SetOneLangMsbtValue(chosenLangs[langNum], chosenMsbtFolder, chosenMsbtName, chosenMsbtKey, locTexts[langNum]);
        }
    }
}
