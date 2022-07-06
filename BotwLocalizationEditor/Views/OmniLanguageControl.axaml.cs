using Avalonia;
using Avalonia.Controls;
using Avalonia.Data;
using BotwLocalizationEditor.Interfaces;

namespace BotwLocalizationEditor.Views
{
    public partial class OmniLanguageControl : UserControl, IUpdatable
    {
        public OmniLanguageControl()
        {
            InitializeComponent();
        }

        public void Update(string[] langs)
        {
            RootGrid.Children.Clear();

            RowDefinition rowDef = new() { Height = GridLength.Star };
            IndexerBinding zero = new(RootGrid, new AttachedProperty<int>("Column", typeof(Grid), new(0)), BindingMode.Default);
            IndexerBinding one = new(RootGrid, new AttachedProperty<int>("Column", typeof(Grid), new(1)), BindingMode.Default);
            IndexerBinding two = new(RootGrid, new AttachedProperty<int>("Column", typeof(Grid), new(2)), BindingMode.Default);
            Binding saveLoc = new("SaveLoc");

            for (int i = 0; i < langs.Length; i++)
            {
                IndexerBinding row = new(RootGrid, new AttachedProperty<int>("Row", typeof(Grid), new(i)), BindingMode.Default);
                RootGrid.RowDefinitions.Add(rowDef);
                RootGrid.Children.Add(new TextBlock()
                {
                    [!Grid.RowProperty] = row,
                    [!Grid.ColumnProperty] = zero,
                    [!TextBlock.TextProperty] = new Binding($"Languages[{i}]"),
                });
                RootGrid.Children.Add(new TextBox()
                {
                    [!Grid.RowProperty] = row,
                    [!Grid.ColumnProperty] = one,
                    [!TextBox.TextProperty] = new Binding($"LocText{i}"),
                });
                RootGrid.Children.Add(new Button()
                {
                    [!Grid.RowProperty] = row,
                    [!Grid.ColumnProperty] = two,
                    [!Button.CommandProperty] = saveLoc,
                    Content = "Apply",
                });
                ((Button)RootGrid.Children[2])[!Button.CommandParameterProperty] = new IndexerBinding(RootGrid.Children[2], new StyledProperty<int>("CommandParameter", typeof(Button), new(i)), BindingMode.Default);
            }
        }
    }
}
