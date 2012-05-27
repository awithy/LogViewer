using System.Windows;
using System.Windows.Input;

namespace LogViewer
{
    public partial class FilterView : Window
    {
        public FilterView()
        {
            InitializeComponent();
        }

        private void _SelectAll(object sender, RoutedEventArgs e)
        {
            foreach (var filter in (this.DataContext as FilterViewModel).Filters)
                filter.Selected = true;
        }

        private void _SelectNone(object sender, RoutedEventArgs e)
        {
            foreach (var filter in (this.DataContext as FilterViewModel).Filters)
                filter.Selected = false;
        }

        protected override void OnKeyUp(KeyEventArgs e)
        {
            if (e.Key == Key.Return || e.Key == Key.Escape)
                this.Close();
            base.OnKeyUp(e);
        }
    }
}
