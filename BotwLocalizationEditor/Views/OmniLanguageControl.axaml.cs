using Avalonia.Controls;
using BotwLocalizationEditor.Interfaces;
using BotwLocalizationEditor.ViewModels;
using System;

namespace BotwLocalizationEditor.Views
{
    public partial class OmniLanguageControl : LanguageControlBase, IUpdatable
    {
        private readonly Grid[] locGrids;
        private readonly TextBlock[] langBlocks;
        public OmniLanguageControl()
        {
            InitializeComponent();
            locGrids = new Grid[16];
            langBlocks = new TextBlock[16];

            OmniLanguageViewModel vm = (OmniLanguageViewModel)DataContext!;
            int gridNum = -1;

            foreach (Control child in RootGrid.Children)
            {
                if (child is Grid grid)
                {
                    grid.IsVisible = false;
                    locGrids[++gridNum] = grid;
                    foreach (Control child2 in grid.Children)
                    {
                        if (child2 is TextBox box)
                        {
                            vm.langBoxes[gridNum] = box;
                        }
                        if (child2 is TextBlock block)
                        {
                            langBlocks[gridNum] = block;
                        }
                    }
                }
            }
        }

        public void Update(string[] langs)
        {
            Array.ForEach(locGrids, grid => grid.IsVisible = false);

            int squareSide = (int)Math.Ceiling(Math.Sqrt(langs.Length));
            string[] defs = new string[squareSide];
            Array.Fill(defs, "1*");
            string def = string.Join(",", defs);
            RootGrid.ColumnDefinitions = new(def);
            RootGrid.RowDefinitions = new(def);

            foreach ((int langIdx, int gridIdx) in OmniLanguageViewModel.GridIndices(langs.Length))
            {
                locGrids[gridIdx].IsVisible = true;
                langBlocks[gridIdx].Text = langs[langIdx];
            }
        }
    }
}
