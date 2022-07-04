using BotwLocalizationEditor.Models;
using ReactiveUI;
using System.Linq;

namespace BotwLocalizationEditor.ViewModels
{
    public class SingleLanguageViewModel : LanguageViewModelBase, IFolderChoosable
    {
        private string chosenLanguage;
        private string locText;
        public string ChosenLanguage { get => chosenLanguage; set => this.RaiseAndSetIfChanged(ref chosenLanguage, value); }
        public string LocText { get => locText; set => locText = value; }

        public SingleLanguageViewModel() : base()
        {
            chosenLanguage = "";
            locText = "";
        }

        public void OnFolderChosen(LanguageModel languageModel)
        {
            model = languageModel;
            Languages = model.GetLangs();
            ChosenLanguage = langs[0];
        }

        internal void LanguageChanged(string newLang)
        {
            MsbtFolders = model.GetOneLangMsbtFolders(newLang);
            ChosenMsbtFolder = msbtFolders.First();
            MsbtNames = model.GetOneLangMsbtNames(newLang, chosenMsbtFolder);
            ChosenMsbtName = msbtNames.First();
            MsbtKeys = model.GetOneLangMsbtKeys(newLang, chosenMsbtFolder, chosenMsbtName);
            ChosenMsbtKey = msbtKeys.FirstOrDefault("");
        }

        internal void FolderChanged(string newFolder)
        {
            MsbtNames = model.GetOneLangMsbtNames(chosenLanguage, newFolder);
            ChosenMsbtName = msbtNames.First();
            MsbtKeys = model.GetOneLangMsbtKeys(chosenLanguage, newFolder, chosenMsbtName);
            ChosenMsbtKey = msbtKeys.FirstOrDefault("");
        }

        internal void MsbtChanged(string newMsbt)
        {
            MsbtKeys = model.GetOneLangMsbtKeys(chosenLanguage, chosenMsbtFolder, newMsbt);
            ChosenMsbtKey = msbtKeys.FirstOrDefault("");
        }

        internal string KeyChanged(string lang, string folder, string name, string key)
        {
            return model.GetOneLangMsbtValue(lang, folder, name, key);
        }

        internal void SaveLoc(string newLoc)
        {
            model.SetOneLangMsbtValue(chosenLanguage, chosenMsbtFolder, chosenMsbtName, chosenMsbtKey, newLoc);
        }
    }
}
