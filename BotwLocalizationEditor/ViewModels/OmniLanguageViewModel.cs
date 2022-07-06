using BotwLocalizationEditor.Models;
using ReactiveUI;
using System;
using System.Collections.Generic;

namespace BotwLocalizationEditor.ViewModels
{
    public class OmniLanguageViewModel : LanguageViewModelBase
    {
        private string[] locTexts = new string[14];

        // BotW has 14 languages. This is messy, but for some reason indexer bindings only work from XAML
        public string LocText0 { get => locTexts[0]; set => this.RaiseAndSetIfChanged(ref locTexts[0], value); }
        public string LocText1 { get => locTexts[1]; set => this.RaiseAndSetIfChanged(ref locTexts[1], value); }
        public string LocText2 { get => locTexts[2]; set => this.RaiseAndSetIfChanged(ref locTexts[2], value); }
        public string LocText3 { get => locTexts[3]; set => this.RaiseAndSetIfChanged(ref locTexts[3], value); }
        public string LocText4 { get => locTexts[4]; set => this.RaiseAndSetIfChanged(ref locTexts[4], value); }
        public string LocText5 { get => locTexts[5]; set => this.RaiseAndSetIfChanged(ref locTexts[5], value); }
        public string LocText6 { get => locTexts[6]; set => this.RaiseAndSetIfChanged(ref locTexts[6], value); }
        public string LocText7 { get => locTexts[7]; set => this.RaiseAndSetIfChanged(ref locTexts[7], value); }
        public string LocText8 { get => locTexts[8]; set => this.RaiseAndSetIfChanged(ref locTexts[8], value); }
        public string LocText9 { get => locTexts[9]; set => this.RaiseAndSetIfChanged(ref locTexts[9], value); }
        public string LocText10 { get => locTexts[10]; set => this.RaiseAndSetIfChanged(ref locTexts[10], value); }
        public string LocText11 { get => locTexts[11]; set => this.RaiseAndSetIfChanged(ref locTexts[11], value); }
        public string LocText12 { get => locTexts[12]; set => this.RaiseAndSetIfChanged(ref locTexts[12], value); }
        public string LocText13 { get => locTexts[13]; set => this.RaiseAndSetIfChanged(ref locTexts[13], value); }

        public OmniLanguageViewModel() : base() { }

        public override void OnFolderChosen(LanguageModel languageModel)
        {
            //locTexts = new string[languageModel.GetLangs().Length];
            base.OnFolderChosen(languageModel);
        }

        protected override void OnKeyChanged(string key)
        {
            Dictionary<string, string> newLocs = model.GetAllLangsMsbtValues(chosenMsbtFolder, chosenMsbtName, key);
            for (int i = 0; i < langs.Length; i++)
            {
                this.RaiseAndSetIfChanged(ref locTexts[i], newLocs[langs[i]], $"LocText{i}");
            }
        }

        internal void SaveLoc(string langNumstr)
        {
            int langNum = int.Parse(langNumstr);
            model.SetOneLangMsbtValue(langs[langNum], chosenMsbtFolder, chosenMsbtName, chosenMsbtKey, locTexts[langNum]);
        }
    }
}
