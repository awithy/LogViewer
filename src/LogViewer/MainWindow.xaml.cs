using System.Windows;

namespace LogViewer
{
    public partial class MainWindow : Window
    {
        private readonly LogFileViewModel _logFileViewModel;

        public MainWindow()
        {
            InitializeComponent();
            _logFileViewModel = new LogFileViewModel();
            this.DataContext = _logFileViewModel;
        }

        private void _ShowFilterLevelDialog(object sender, RoutedEventArgs e)
        {
            _logFileViewModel.ShowLogLevelFilterDialog();
        }

        private void _ShowFilterSourceDialog(object sender, RoutedEventArgs e)
        {
            _logFileViewModel.ShowSourceFilterDialog();
        }

        private void _ShowFilterSearchDialog(object sender, RoutedEventArgs e)
        {
            _logFileViewModel.ShowSearchFilterDialog();
        }
    }
}
