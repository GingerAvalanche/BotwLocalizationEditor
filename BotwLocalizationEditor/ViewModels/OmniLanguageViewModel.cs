using Avalonia.Controls;
using BotwLocalizationEditor.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BotwLocalizationEditor.ViewModels
{
    public class OmniLanguageViewModel : LanguageViewModelBase
    {
        public TextBox[] langBoxes;

        public OmniLanguageViewModel() : base()
        {
            langBoxes = new TextBox[16];
        }

        public override void OnFolderChosen(LanguageModel languageModel)
        {
            base.OnFolderChosen(languageModel);
        }

        protected override void OnKeyChanged(string key)
        {
            Dictionary<string, string> newLocs = model.GetAllLangsMsbtValues(chosenMsbtFolder, chosenMsbtName, key);
            foreach ((int langIdx, int gridIdx) in GridIndices(Languages.Length))
            {
                langBoxes[gridIdx].Text = newLocs[Languages[langIdx]];
            }
        }

        internal void SaveLoc(int langNum)
        {
            model.SetOneLangMsbtValue(Languages[langNum], chosenMsbtFolder, chosenMsbtName, chosenMsbtKey, langBoxes[langNum].Text!);
        }

        // I really wish Avalonia wasn't dumbtarded when it came to binding things to TextBoxes
        // It would be great if I didn't need to do all this manually!
        public static (int, int)[] GridIndices(int numLangs)
        {
            int[] indices = new int[numLangs];
            int squareSide = (int)Math.Ceiling(Math.Sqrt(numLangs));
            int idx = 0;
            for (int row = 0; row < squareSide; ++row)
            {
                for (int col = 0; col < squareSide; ++col)
                {
                    indices[idx] = row * 4 + col;
                    if (++idx == numLangs)
                    {
                        goto End;
                    }
                }
            }
            End:
                return indices.Select((gridIdx, langIdx) => (langIdx, gridIdx)).ToArray();
        }
    }
}
