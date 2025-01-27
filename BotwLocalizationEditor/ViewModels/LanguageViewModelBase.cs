using Avalonia.Controls;
using BotwLocalizationEditor.Models;
using BotwLocalizationEditor.Views;
using ReactiveUI;
using System.Collections.Generic;

namespace BotwLocalizationEditor.ViewModels
{
    public class LanguageViewModelBase : ViewModelBase
    {
        protected LanguageModel Model;
        private readonly BrowserControl[] browserControls;
        private BrowserControl selectedBrowserControl;
        private readonly int selectedBrowserControlIndex;
        private string chosenMsbtFolder;
        private string chosenMsbtName;
        private string chosenMsbtKey;
        public string[] Languages => Model.GetLangs();

        public SortedSet<string> MsbtFolders
        {
            get => FolderBrowser.Items;
            set
            {
                FolderBrowser.Items = value;
                if (value.Count <= 0) return;
                ChosenMsbtFolder = FolderBrowser.Selected;
                MsbtNames = Model.GetAllLangsMsbtNames(chosenMsbtFolder);
            }
        }
        public SortedSet<string> MsbtNames
        {
            get => MsbtBrowser.Items;
            set
            {
                MsbtBrowser.Items = value;
                if (value.Count <= 0) return;
                ChosenMsbtName = MsbtBrowser.Selected;
                MsbtKeys = Model.GetAllLangsMsbtKeys(chosenMsbtFolder, chosenMsbtName);
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
        public UserControl SelectedBrowserControl
        {
            get => selectedBrowserControl;
            set => this.RaiseAndSetIfChanged(ref selectedBrowserControl, (BrowserControl)value);
        }

        private BrowserControl FolderBrowser => browserControls[0];
        public BrowserControl MsbtBrowser => browserControls[1];
        public BrowserControl KeyBrowser => browserControls[2];
        public bool IsShowFolder => selectedBrowserControlIndex == 1;
        public bool IsShowMsbtName => selectedBrowserControlIndex == 2;
        public bool IsShowMsbtKey => selectedBrowserControlIndex == 3;

        protected LanguageViewModelBase()
        {
            Model = new("");
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
            Model = languageModel;
            OnLanguagesSet(languageModel.GetLangs());
        }

        public void OnTabSelected(int tabNum)
        {
            SelectedBrowserControl = browserControls[tabNum];
        }

        public void SaveFiles(string folder)
        {
            Model.Save(folder);
        }

        public void AddMsbt(string msbtName)
        {
            Model.AddMsbtAllLangs(chosenMsbtFolder, msbtName);
            MsbtNames = Model.GetAllLangsMsbtNames(chosenMsbtFolder);
        }

        public void AddMsbtKey(string keyName)
        {
            Model.AddMsbtKeyAllLangs(chosenMsbtFolder, chosenMsbtName, keyName);
            MsbtKeys = Model.GetAllLangsMsbtKeys(chosenMsbtFolder, chosenMsbtName);
        }

        protected virtual void OnLanguagesSet(string[] languages)
        {
            MsbtFolders = Model.GetSortedMsbtFolders();
        }
        protected virtual void OnKeyChanged(string newKey) { }

        private void MsbtFolder_SelectionChanged(object? sender, SelectionChangedEventArgs e)
        {
            if (sender is not ListBox { SelectedItem: not null } box) return;
            ChosenMsbtFolder = (string)box.SelectedItem;
            MsbtNames = Model.GetAllLangsMsbtNames(chosenMsbtFolder);
        }

        private void MsbtName_SelectionChanged(object? sender, SelectionChangedEventArgs e)
        {
            if (sender is not ListBox { SelectedItem: not null } box) return;
            ChosenMsbtName = (string)box.SelectedItem;
            MsbtKeys = Model.GetAllLangsMsbtKeys(chosenMsbtFolder, chosenMsbtName);
        }

        private void MsbtKey_SelectionChanged(object? sender, SelectionChangedEventArgs e)
        {
            if (sender == null || sender is not ListBox box || box.SelectedItem == null) return;
            ChosenMsbtKey = (string)box.SelectedItem;
            OnKeyChanged(chosenMsbtKey);
        }

        public Dictionary<string, Dictionary<string, Dictionary<string, Dictionary<string, bool>>>> ScanForMissing()
        {
            return Model.FindMissing();
        }

        public Dictionary<string, Dictionary<string, Dictionary<string, bool>>> ScanForNew()
        {
            return Model.FindNew();
        }
    }
}
