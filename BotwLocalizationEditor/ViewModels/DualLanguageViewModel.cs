using BotwLocalizationEditor.Models;
using System.Linq;

namespace BotwLocalizationEditor.ViewModels
{
    public class DualLanguageViewModel : LanguageViewModelBase, IFolderChoosable
    {
        private string[] chosenLangs;

        public DualLanguageViewModel() : base()
        {
            chosenLangs = new string[2];
        }

        public void OnFolderChosen(LanguageModel languageModel)
        {
            model = languageModel;
            Languages = model.GetLangs();
            if (langs.Length < 2)
            {
                chosenLangs[0] = langs[0];
                chosenLangs[1] = langs[0];
            }
            else
            {
                chosenLangs = langs[..2];
            }
            MsbtFolders = model.GetTwoLangsMsbtFolders(chosenLangs);
        }

        internal void FolderChanged(string newFolder)
        {
            MsbtNames = model.GetTwoLangsMsbtNames(chosenLangs, newFolder);
            ChosenMsbtName = msbtNames.First();
            MsbtKeys = model.GetTwoLangsMsbtKeys(chosenLangs, newFolder, chosenMsbtKey);
            ChosenMsbtKey = msbtKeys.FirstOrDefault("");
        }

        internal void MsbtChanged(string newMsbt)
        {
            MsbtKeys = model.GetTwoLangsMsbtKeys(chosenLangs, chosenMsbtFolder, newMsbt);
            ChosenMsbtKey = msbtKeys.FirstOrDefault("");
        }

        internal void KeyChanged(string newKey) { }
    }
}
