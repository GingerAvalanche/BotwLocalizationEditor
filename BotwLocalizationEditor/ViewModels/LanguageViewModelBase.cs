using Avalonia.Controls;
using BotwLocalizationEditor.Models;
using BotwLocalizationEditor.Views;
using ReactiveUI;
using System.Collections.Generic;

namespace BotwLocalizationEditor.ViewModels
{
    public class LanguageViewModelBase : ViewModelBase
    {
        protected LanguageModel model;
        protected readonly BrowserControl[] browserControls;
        protected BrowserControl selectedBrowserControl;
        protected int selectedBrowserControlIndex;
        protected string chosenMsbtFolder;
        protected string chosenMsbtName;
        protected string chosenMsbtKey;
        public string[] Languages
        {
            get => model.GetLangs();
        }
        public SortedSet<string> MsbtFolders
        {
            get => FolderBrowser.Items;
            set
            {
                FolderBrowser.Items = value;
                if (value.Count > 0)
                {
                    ChosenMsbtFolder = FolderBrowser.Selected;
                    MsbtNames = model.GetAllLangsMsbtNames(chosenMsbtFolder);
                }
            }
        }
        public SortedSet<string> MsbtNames
        {
            get => MsbtBrowser.Items;
            set
            {
                MsbtBrowser.Items = value;
                if (value.Count > 0)
                {
                    ChosenMsbtName = MsbtBrowser.Selected;
                    MsbtKeys = model.GetAllLangsMsbtKeys(chosenMsbtFolder, chosenMsbtName);
                }
            }
        }
        public SortedSet<string> MsbtKeys
        {
            get => KeyBrowser.Items;
            set
            {
                KeyBrowser.Items = value;
                ChosenMsbtKey = KeyBrowser.Selected;
                OnKeyChanged(chosenMsbtKey);
            }
        }
        public string ChosenMsbtFolder { get => chosenMsbtFolder; set => this.RaiseAndSetIfChanged(ref chosenMsbtFolder, value); }
        public string ChosenMsbtName { get => chosenMsbtName; set => this.RaiseAndSetIfChanged(ref chosenMsbtName, value); }
        public string ChosenMsbtKey { get => chosenMsbtKey; set => this.RaiseAndSetIfChanged(ref chosenMsbtKey, value); }
        public UserControl SelectedBrowserControl { get => selectedBrowserControl; set { this.RaiseAndSetIfChanged(ref selectedBrowserControl, (BrowserControl)value); } }
        public BrowserControl FolderBrowser => browserControls[0];
        public BrowserControl MsbtBrowser => browserControls[1];
        public BrowserControl KeyBrowser => browserControls[2];
        public bool IsShowFolder { get => selectedBrowserControlIndex == 1; }
        public bool IsShowMsbtName { get => selectedBrowserControlIndex == 2; }
        public bool IsShowMsbtKey { get => selectedBrowserControlIndex == 3; }

        public LanguageViewModelBase()
        {
            model = new("");
            chosenMsbtFolder = string.Empty;
            chosenMsbtName = string.Empty;
            chosenMsbtKey = string.Empty;

            selectedBrowserControlIndex = 0;
            browserControls = new BrowserControl[3];
            browserControls[0] = new(MsbtFolder_SelectionChanged);
            FolderBrowser.AddButton.IsVisible = false;
            browserControls[1] = new(MsbtName_SelectionChanged);
            MsbtBrowser.AddButton.Content = "Add MSBT";
            browserControls[2] = new(MsbtKey_SelectionChanged);
            KeyBrowser.AddButton.Content = "Add Key";
            selectedBrowserControl = browserControls[selectedBrowserControlIndex];
        }

        public virtual void OnFolderChosen(LanguageModel languageModel)
        {
            model = languageModel;
            OnLanguagesSet(languageModel.GetLangs());
        }

        public void OnTabSelected(int tabNum)
        {
            SelectedBrowserControl = browserControls[tabNum];
        }

        public void SaveFiles(string folder)
        {
            model.Save(folder);
        }

        public void AddMsbt(string msbtName)
        {
            model.AddMsbtAllLangs(chosenMsbtFolder, msbtName);
            MsbtNames = model.GetAllLangsMsbtNames(chosenMsbtFolder);
        }

        public void AddMsbtKey(string keyName)
        {
            model.AddMsbtKeyAllLangs(chosenMsbtFolder, chosenMsbtName, keyName);
            MsbtKeys = model.GetAllLangsMsbtKeys(chosenMsbtFolder, chosenMsbtName);
        }

        protected virtual void OnLanguagesSet(string[] langs)
        {
            MsbtFolders = model.GetSortedMsbtFolders();
        }
        protected virtual void OnKeyChanged(string newKey) { }

        private void MsbtFolder_SelectionChanged(object? sender, SelectionChangedEventArgs e)
        {
            if (sender != null && sender is ListBox box && box.SelectedItem != null)
            {
                ChosenMsbtFolder = (string)box.SelectedItem;
                MsbtNames = model.GetAllLangsMsbtNames(chosenMsbtFolder);
            }
        }

        private void MsbtName_SelectionChanged(object? sender, SelectionChangedEventArgs e)
        {
            if (sender != null && sender is ListBox box && box.SelectedItem != null)
            {
                ChosenMsbtName = (string)box.SelectedItem;
                MsbtKeys = model.GetAllLangsMsbtKeys(chosenMsbtFolder, chosenMsbtName);
            }
        }

        private void MsbtKey_SelectionChanged(object? sender, SelectionChangedEventArgs e)
        {
            if (sender != null && sender is ListBox box && box.SelectedItem != null)
            {
                ChosenMsbtKey = (string)box.SelectedItem;
                OnKeyChanged(chosenMsbtKey);
            }
        }

        public Dictionary<string, Dictionary<string, Dictionary<string, Dictionary<string, bool>>>> ScanForMissing()
        {
            return model.FindMissing();
        }

        public Dictionary<string, Dictionary<string, Dictionary<string, bool>>> ScanForNew()
        {
            return model.FindNew();
        }
    }
}
