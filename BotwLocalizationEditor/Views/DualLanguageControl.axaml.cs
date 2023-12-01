using BotwLocalizationEditor.Interfaces;

namespace BotwLocalizationEditor.Views
{
    public partial class DualLanguageControl : LanguageControlBase, IUpdatable
    {
        public DualLanguageControl()
        {
            InitializeComponent();
        }

        public void Update(string[] langs) { }
    }
}
