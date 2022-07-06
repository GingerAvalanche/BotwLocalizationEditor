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

            System.Diagnostics.Debug.WriteLine(ApplyButton[!Button.CommandParameterProperty]);
            System.Diagnostics.Debug.WriteLine(((Avalonia.Data.IndexerBinding)ApplyButton[!Button.CommandParameterProperty]).Property);
            System.Diagnostics.Debug.WriteLine(((Avalonia.Data.IndexerBinding)ApplyButton[!Button.CommandParameterProperty]).Property.GetType());
            System.Diagnostics.Debug.WriteLine(((Avalonia.Data.IndexerBinding)ApplyButton[!Button.CommandParameterProperty]).Property.PropertyType);
            System.Diagnostics.Debug.WriteLine(((Avalonia.Data.IndexerBinding)ApplyButton[!Button.CommandParameterProperty]).Property.Name);
        }

        public void Update(string[] langs) { }
    }
}
