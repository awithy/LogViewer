using System.Windows;
using System.Windows.Input;

namespace LogViewer
{
    public partial class SearchView : Window
    {
        public SearchView()
        {
            InitializeComponent();
        }

        protected override void OnKeyUp(KeyEventArgs e)
        {
            if (e.Key == Key.Return || e.Key == Key.Escape)
                this.Close();
            base.OnKeyUp(e);
        }
    }
}
