using ReactiveUI;
using System.Collections.Generic;
using System.Linq;

namespace BotwLocalizationEditor.ViewModels
{
    public class BrowserViewModel : ViewModelBase
    {
        private SortedSet<string> baseItems = new();
        public SortedSet<string> BaseItems
        {
            get => baseItems;
            set
            {
                baseItems = value;
                Filter = "";
            }
        }

        private SortedSet<string> items = new();
        public SortedSet<string> Items
        {
            get => items;
            set => this.RaiseAndSetIfChanged(ref items, value);
        }

        private string selected = "";
        public string Selected
        {
            get => selected;
            set => this.RaiseAndSetIfChanged(ref selected, value);
        }

        private string filter = "";
        public string Filter
        {
            get => filter;
            set
            {
                this.RaiseAndSetIfChanged(ref filter, value);

                Items = string.IsNullOrEmpty(filter) ? baseItems :
                    new(baseItems.Where(s => s.ToLower().Contains(filter.ToLower())));
                Selected = items.Count > 0 ? items.First() : "";
            }
        }
    }
}
