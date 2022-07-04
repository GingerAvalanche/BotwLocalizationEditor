using Avalonia.Controls;
using BotwLocalizationEditor.ViewModels;

namespace BotwLocalizationEditor.Views
{
    public partial class DualLanguageControl : UserControl
    {
        private bool HandleThis = true;
        public DualLanguageControl()
        {
            InitializeComponent();

            FolderBox.SelectionChanged += FolderBox_SelectionChanged;
            MsbtBox.SelectionChanged += MsbtBox_SelectionChanged;
            KeyBox.SelectionChanged += KeyBox_SelectionChanged;
        }

        private void FolderBox_SelectionChanged(object? sender, SelectionChangedEventArgs e)
        {
            if (!HandleThis) return;
            HandleThis = false;
            (DataContext as DualLanguageViewModel)!.FolderChanged(((sender as ComboBox)!.SelectedItem as string)!);
            HandleThis = true;
        }

        private void MsbtBox_SelectionChanged(object? sender, SelectionChangedEventArgs e)
        {
            if (!HandleThis) return;
            HandleThis = false;
            (DataContext as DualLanguageViewModel)!.MsbtChanged(((sender as ComboBox)!.SelectedItem as string)!);
            HandleThis = true;
        }

        private void KeyBox_SelectionChanged(object? sender, SelectionChangedEventArgs e)
        {
            if (!HandleThis) return;
            HandleThis = false;
            (DataContext as DualLanguageViewModel)!.KeyChanged(((sender as ComboBox)!.SelectedItem as string)!);
            HandleThis = true;
        }
    }
}
