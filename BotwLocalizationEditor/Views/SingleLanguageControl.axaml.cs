using BotwLocalizationEditor.Interfaces;
using BotwLocalizationEditor.ViewModels;

namespace BotwLocalizationEditor.Views
{
    public partial class SingleLanguageControl : LanguageControlBase, IUpdatable
    {
        public SingleLanguageControl()
        {
            InitializeComponent();

            SingleLanguageViewModel vm = (DataContext as SingleLanguageViewModel)!;
            vm.MsbtBrowser.AddButton.Click += AddMsbtButton_Click;
            vm.KeyBrowser.AddButton.Click += AddMsbtKeyButton_Click;
        }

        public void Update(string[] langs) { }
    }
}
