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
        public string[] Languages
        {
            get => langs;
            set
            {
                this.RaiseAndSetIfChanged(ref langs, value);
                OnLanguagesSet(value);
            }
        }
        public SortedSet<string> MsbtFolders
        {
            get => msbtFolders;
            set
            {
                this.RaiseAndSetIfChanged(ref msbtFolders, value);
                if (value.Count > 0)
                {
                    ChosenMsbtFolder = value.First();
                }
            }
        }
        public string ChosenMsbtFolder
        {
            get => chosenMsbtFolder;
            set
            {
                this.RaiseAndSetIfChanged(ref chosenMsbtFolder, value);
                if (!string.IsNullOrEmpty(value))
                {
                    MsbtNames = model.GetAllLangsMsbtNames(value);
                }
            }
        }
        public SortedSet<string> MsbtNames
        {
            get => msbtNames;
            set
            {
                this.RaiseAndSetIfChanged(ref msbtNames, value);
                if (value.Count > 0)
                {
                    ChosenMsbtName = value.First();
                }
            }
        }
        public string ChosenMsbtName
        {
            get => chosenMsbtName;
            set
            {
                this.RaiseAndSetIfChanged(ref chosenMsbtName, value);
                if (!(string.IsNullOrEmpty(value) ||
                    string.IsNullOrEmpty(chosenMsbtFolder)))
                MsbtKeys = model.GetAllLangsMsbtKeys(chosenMsbtFolder, value);
            }
        }
        public SortedSet<string> MsbtKeys
        {
            get => msbtKeys;
            set
            {
                this.RaiseAndSetIfChanged(ref msbtKeys, value);
                if (value.Count > 0)
                {
                    ChosenMsbtKey = value.First();
                }
            }
        }
        public string ChosenMsbtKey
        {
            get => chosenMsbtKey;
            set
            {
                this.RaiseAndSetIfChanged(ref chosenMsbtKey, value);
                OnKeyChanged(value);
            }
        }

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

        public virtual void OnFolderChosen(LanguageModel languageModel)
        {
            model = languageModel;
            Languages = model.GetLangs();
        }

        public void SaveFiles(string folder)
        {
            model.Save(folder);
        }

        protected virtual void OnLanguagesSet(string[] langs)
        {
            MsbtFolders = model.GetMsbtFolders();
        }
        protected virtual void OnKeyChanged(string newKey) { }
    }
}
