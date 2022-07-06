using ReactiveUI;

namespace BotwLocalizationEditor.ViewModels
{
    public class SingleLanguageViewModel : LanguageViewModelBase
    {
        private string chosenLanguage = "";
        private string locText = "";
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
        public string LocText
        {
            get => locText;
            set => this.RaiseAndSetIfChanged(ref locText, value);
        }

        public SingleLanguageViewModel() : base() { }

        protected override void OnLanguagesSet(string[] langs)
        {
            this.RaiseAndSetIfChanged(ref chosenLanguage, langs[0], nameof(ChosenLanguage));
            base.OnLanguagesSet(langs);
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
