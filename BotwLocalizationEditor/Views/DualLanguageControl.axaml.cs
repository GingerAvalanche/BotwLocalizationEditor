using BotwLocalizationEditor.Interfaces;

namespace BotwLocalizationEditor.Views
{
    public partial class DualLanguageControl : LanguageControlBase, IUpdatable
    {
        public DualLanguageControl()
        {
            InitializeComponent();

            AddMsbtButton.Click += AddMsbtButton_Click;
            AddMsbtKeyButton.Click += AddMsbtKeyButton_Click;
        }

        public void Update(string[] langs) { }
    }
}
