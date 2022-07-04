using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace BotwLocalizationEditor.Views
{
    public partial class OmniLanguageControl : UserControl
    {
        public OmniLanguageControl()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
