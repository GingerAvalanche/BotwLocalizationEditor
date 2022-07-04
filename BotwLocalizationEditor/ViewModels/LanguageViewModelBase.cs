using Avalonia.Controls;
using BotwLocalizationEditor.Models;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BotwLocalizationEditor.ViewModels
{
    public class LanguageViewModelBase : ViewModelBase
    {
        protected LanguageModel model;
        protected string[] langs;
        protected SortedSet<string> msbtFolders;
        protected string chosenMsbtFolder;
        protected SortedSet<string> msbtNames;
        protected string chosenMsbtName;
        protected SortedSet<string> msbtKeys;
        protected string chosenMsbtKey;
        public string[] Languages { get => langs; set => this.RaiseAndSetIfChanged(ref langs, value); }
        public SortedSet<string> MsbtFolders { get => msbtFolders; set => this.RaiseAndSetIfChanged(ref msbtFolders, value); }
        public string ChosenMsbtFolder { get => chosenMsbtFolder; set => this.RaiseAndSetIfChanged(ref chosenMsbtFolder, value); }
        public SortedSet<string> MsbtNames { get => msbtNames; set => this.RaiseAndSetIfChanged(ref msbtNames, value); }
        public string ChosenMsbtName { get => chosenMsbtName; set => this.RaiseAndSetIfChanged(ref chosenMsbtName, value); }
        public SortedSet<string> MsbtKeys { get => msbtKeys; set => this.RaiseAndSetIfChanged(ref msbtKeys, value); }
        public string ChosenMsbtKey { get => chosenMsbtKey; set => this.RaiseAndSetIfChanged(ref chosenMsbtKey, value); }

        public LanguageViewModelBase()
        {
            model = new("");
            langs = Array.Empty<string>();
            msbtFolders = new();
            chosenMsbtFolder = "";
            msbtNames = new();
            chosenMsbtName = "";
            msbtKeys = new();
            chosenMsbtKey = "";
        }
    }
}
