using Avalonia.Controls;
using Avalonia.Interactivity;
using BotwLocalizationEditor.ViewModels;

namespace BotwLocalizationEditor.Views
{
    public partial class SingleLanguageControl : UserControl
    {
        private bool HandleThis = true;
        public SingleLanguageControl()
        {
            InitializeComponent();

            LanguageBox.SelectionChanged += LanguageBox_SelectionChanged;
            FolderBox.SelectionChanged += FolderBox_SelectionChanged;
            MsbtBox.SelectionChanged += MsbtBox_SelectionChanged;
            KeyBox.SelectionChanged += KeyBox_SelectionChanged;
            ApplyButton.Click += ApplyButton_Click;
        }

        private void LanguageBox_SelectionChanged(object? sender, SelectionChangedEventArgs e)
        {
            if (!HandleThis) return;
            HandleThis = false;
            SingleLanguageViewModel vm = (SingleLanguageViewModel)DataContext!;
            string newLang = ((sender as ComboBox)!.SelectedItem as string)!;
            vm.LanguageChanged(newLang);
            LocBox.Text = vm.KeyChanged(newLang, vm.ChosenMsbtFolder, vm.ChosenMsbtName, vm.ChosenMsbtKey);
            HandleThis = true;
        }

        private void FolderBox_SelectionChanged(object? sender, SelectionChangedEventArgs e)
        {
            if (!HandleThis) return;
            HandleThis = false;
            SingleLanguageViewModel vm = (SingleLanguageViewModel)DataContext!;
            string newFolder = ((sender as ComboBox)!.SelectedItem as string)!;
            vm.FolderChanged(newFolder);
            LocBox.Text = vm.KeyChanged(vm.ChosenLanguage, newFolder, vm.ChosenMsbtName, vm.ChosenMsbtKey);
            HandleThis = true;
        }

        private void MsbtBox_SelectionChanged(object? sender, SelectionChangedEventArgs e)
        {
            if (!HandleThis) return;
            HandleThis = false;
            SingleLanguageViewModel vm = (SingleLanguageViewModel)DataContext!;
            string newMsbt = ((sender as ComboBox)!.SelectedItem as string)!;
            vm.MsbtChanged(newMsbt);
            LocBox.Text = vm.KeyChanged(vm.ChosenLanguage, vm.ChosenMsbtFolder, newMsbt, vm.ChosenMsbtKey);
            HandleThis = true;
        }

        private void KeyBox_SelectionChanged(object? sender, SelectionChangedEventArgs e)
        {
            if (!HandleThis) return;
            HandleThis = false;
            SingleLanguageViewModel vm = (SingleLanguageViewModel)DataContext!;
            string newKey = ((sender as ComboBox)!.SelectedItem as string)!;
            LocBox.Text = vm.KeyChanged(vm.ChosenLanguage, vm.ChosenMsbtFolder, vm.ChosenMsbtName, newKey);
            HandleThis = true;
        }

        private void ApplyButton_Click(object? sender, RoutedEventArgs e)
        {
            (DataContext as SingleLanguageViewModel)!.SaveLoc(LocBox.Text);
        }
    }
}
