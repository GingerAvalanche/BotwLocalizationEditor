using Avalonia.Controls;
using BotwLocalizationEditor.ViewModels;
using System;
using System.Collections.Generic;

namespace BotwLocalizationEditor.Views
{
    public partial class BrowserControl : UserControl
    {
        public BrowserControl()
        {
            InitializeComponent();
        }
        public BrowserControl(EventHandler<SelectionChangedEventArgs> selectionFunc)
        {
            InitializeComponent();

            ItemList.SelectionChanged += selectionFunc;

            TextBox searchField = this.FindControl<TextBox>("Filter")!;
            searchField.AttachedToVisualTree += (_, _) => searchField.Focus();
        }

        public SortedSet<string> Items
        {
            get => (DataContext as BrowserViewModel)!.BaseItems;
            set => (DataContext as BrowserViewModel)!.BaseItems = value;
        }
        public string Selected => (DataContext as BrowserViewModel)!.Selected;
    }
}
