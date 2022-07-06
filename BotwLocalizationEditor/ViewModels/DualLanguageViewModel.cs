using ReactiveUI;
using System.Collections.Generic;

namespace BotwLocalizationEditor.ViewModels
{
    public class DualLanguageViewModel : LanguageViewModelBase
    {
        private readonly string[] chosenLangs = new string[2];
        private readonly string[] locTexts = new string[2];
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

        public DualLanguageViewModel() : base() { }

        protected override void OnLanguagesSet(string[] langs)
        {
            string[] temp = new string[2];
            temp[0] = langs[0];
            temp[1] = langs[langs.Length < 2 ? 0 : 1];
            this.RaiseAndSetIfChanged(ref chosenLangs[0], temp[0], nameof(Lang0));
            this.RaiseAndSetIfChanged(ref chosenLangs[1], temp[1], nameof(Lang1));
            base.OnLanguagesSet(langs);
        }

        protected override void OnKeyChanged(string key)
        {
            Dictionary<string, string> newLocs = model.GetTwoLangsMsbtValues(chosenLangs, chosenMsbtFolder, chosenMsbtName, key);
            if (newLocs.Count > 0)
            {
                LocText0 = newLocs[Lang0];
                LocText1 = newLocs[Lang1];
            }
            else
            {
                LocText0 = "";
                LocText1 = "";
            }
        }

        internal void SaveLoc(int langNum)
        {
            model.SetOneLangMsbtValue(chosenLangs[langNum], chosenMsbtFolder, chosenMsbtName, chosenMsbtKey, locTexts[langNum]);
        }
    }
}
