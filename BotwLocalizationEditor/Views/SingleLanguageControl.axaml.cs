using Avalonia.Controls;
using BotwLocalizationEditor.Interfaces;

namespace BotwLocalizationEditor.Views
{
    public partial class SingleLanguageControl : UserControl, IUpdatable
    {
        //private bool HandleThis = true;
        public SingleLanguageControl()
        {
            InitializeComponent();
        }

        public void Update(string[] langs) { }
    }
}
