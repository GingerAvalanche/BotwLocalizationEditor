using BotwLocalizationEditor.Interfaces;

namespace BotwLocalizationEditor.Views
{
    public partial class SingleLanguageControl : LanguageControlBase, IUpdatable
    {
        public SingleLanguageControl()
        {
            InitializeComponent();

            AddMsbtButton.Click += AddMsbtButton_Click;
            AddMsbtKeyButton.Click += AddMsbtKeyButton_Click;
        }

        public void Update(string[] langs) { }
    }
}
