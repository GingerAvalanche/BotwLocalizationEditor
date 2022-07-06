using Avalonia.Controls;
using BotwLocalizationEditor.Interfaces;

namespace BotwLocalizationEditor.Views
{
    public partial class DualLanguageControl : UserControl, IUpdatable
    {
        //private bool HandleThis = true;
        public DualLanguageControl()
        {
            InitializeComponent();
        }

        public void Update(string[] langs) { }
    }
}
