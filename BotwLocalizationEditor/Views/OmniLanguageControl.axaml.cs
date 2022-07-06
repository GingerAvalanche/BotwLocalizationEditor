using Avalonia;
using Avalonia.Controls;
using Avalonia.Data;
using BotwLocalizationEditor.Interfaces;

namespace BotwLocalizationEditor.Views
{
    public partial class OmniLanguageControl : LanguageControlBase, IUpdatable
    {
        public OmniLanguageControl()
        {
            InitializeComponent();

            AddMsbtButton.Click += AddMsbtButton_Click;
            AddMsbtKeyButton.Click += AddMsbtKeyButton_Click;
        }

        public void Update(string[] langs)
        {
            RootGrid.Children.Clear();
            RootGrid.RowDefinitions.Clear();

            IndexerBinding zero = new(RootGrid, new AttachedProperty<int>("Column", typeof(Grid), new(0)), BindingMode.Default);
            IndexerBinding one = new(RootGrid, new AttachedProperty<int>("Column", typeof(Grid), new(1)), BindingMode.Default);
            IndexerBinding two = new(RootGrid, new AttachedProperty<int>("Column", typeof(Grid), new(2)), BindingMode.Default);

            for (int i = 0; i < langs.Length; i++)
            {
                RootGrid.RowDefinitions.Add(new() { Height = GridLength.Star });
                IndexerBinding row = new(RootGrid, new AttachedProperty<int>("Row", typeof(Grid), new(i)), BindingMode.Default);
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
                    [!Button.CommandProperty] = new Binding("SaveLoc"),
                    Content = "Apply",
                });
                ((Button)RootGrid.Children[2])[!Button.CommandParameterProperty] = new IndexerBinding(RootGrid.Children[2], new StyledProperty<int>("CommandParameter", typeof(Button), new(i)), BindingMode.Default);
            }
        }
    }
}
