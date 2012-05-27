using System.Collections.ObjectModel;
using System.ComponentModel;

namespace LogViewer
{
    public class FilterViewModel
    {
        public FilterViewModel(ObservableCollection<FilterSelectrion> filters)
        {
            foreach (var filter in filters)
                _filters.Add(filter);
        }

        private ObservableCollection<FilterSelectrion> _filters = new ObservableCollection<FilterSelectrion>();  
        public ObservableCollection<FilterSelectrion> Filters
        {
            get { return _filters; }
        }
    }

    public class FilterSelectrion : INotifyPropertyChanged
    {
        public string FilterName { get; set; }
        private bool _selected;
        public bool Selected
        {
            get { return _selected; }
            set 
            { 
                _selected = value;
                NotifyProperty("Selected");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyProperty(string property)
        {
            var handler = PropertyChanged;
            if(handler != null)
                handler(this, new PropertyChangedEventArgs(property));
        }
    }
}
