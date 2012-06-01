using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Windows;
using Microsoft.Win32;

namespace LogViewer
{
    public class LogFileViewModel : INotifyPropertyChanged
    {
        private readonly List<Line> _allLines = new List<Line>();
        private readonly ObservableCollection<Line> _currentLines = new ObservableCollection<Line>();
        public IEnumerable<Line> CurrentLines { get { return _currentLines; } }
        private readonly List<string> _sources = new List<string>();
        private readonly List<string> _logLevels = new List<string>();
        public readonly ObservableCollection<ILogFilter> LogLevelFilters = new ObservableCollection<ILogFilter>();
        public readonly ObservableCollection<ILogFilter> SourceFilters = new ObservableCollection<ILogFilter>();
        public ILogFilter SearchLogFilter { get; private set; }
        public ObservableCollection<FilterSelectrion> LogLevelFilterSelections { get; private set; }
        public ObservableCollection<FilterSelectrion> SourceFilterSelections { get; private set; }

        public LogFileViewModel()
        {
            var dlg = new OpenFileDialog();
            if (dlg.ShowDialog() != true)
            {
                Application.Current.Shutdown();
                return;
            }
            var fileName = dlg.FileName;

            _LoadFileAndSetup(fileName);
        }

        private void _LoadFileAndSetup(string fileName)
        {
            using (new WaitCursor())
            {
                var fileLines = _ReadFileLines(fileName);
                _ParseFileLines(fileLines);
                _CreateFilterSelections();
                _RefreshCurrentLines();
            }
        }

        private void _CreateFilterSelections()
        {
            LogLevelFilterSelections = new ObservableCollection<FilterSelectrion>();
            foreach (var logLevel in _logLevels)
                LogLevelFilterSelections.Add(new FilterSelectrion {FilterName = logLevel, Selected = true});

            SourceFilterSelections = new ObservableCollection<FilterSelectrion>();
            _sources.Sort();
            foreach (var source in _sources)
                SourceFilterSelections.Add(new FilterSelectrion {FilterName = source, Selected = true});
        }

        private void _ParseFileLines(List<string> fileLines)
        {
            foreach (var line in fileLines)
            {
                var newLine = new Line(line);
                if (newLine.DateTime == "Err")
                    MessageBox.Show(line);
                _allLines.Add(newLine);
                _currentLines.Add(newLine);
                if (!_sources.Contains(newLine.Source))
                    _sources.Add(newLine.Source);
                if (!_logLevels.Contains(newLine.LogLevel))
                    _logLevels.Add(newLine.LogLevel);
            }
        }

        private static List<string> _ReadFileLines(string fileName)
        {
            var actualLines = File.ReadAllLines(fileName);
            var fileLines = new List<string>();
            var currentLine = "";
            foreach (var line in actualLines)
            {
                if (line.Length > 0 && line[0] == '[')
                {
                    if (currentLine != "")
                        fileLines.Add(currentLine);
                    currentLine = line;
                }
                else
                {
                    currentLine += line;
                }
            }
            return fileLines;
        }

        public void ShowLogLevelFilterDialog()
        {
            var filters = _ShowFilterDialog(LogLevelFilterSelections);
            LogLevelFilterSelections = new ObservableCollection<FilterSelectrion>(filters);
            _RefreshCurrentLines();
        }

        private IEnumerable<FilterSelectrion> _ShowFilterDialog(ObservableCollection<FilterSelectrion> filterSelections)
        {
            var filterView = new FilterView();
            var filterViewModel = new FilterViewModel(filterSelections);
            filterView.DataContext = filterViewModel;
            filterView.ShowDialog();
            return filterViewModel.Filters;
        }

        private void _RefreshCurrentLines()
        {
            using (new WaitCursor())
            {
                _CreateFilters();
                _currentLines.Clear();
                foreach (var line in _FilterLines())
                    _currentLines.Add(line);
            }
        }

        private IEnumerable<Line> _FilterLines()
        {
            return _allLines.Where(x => 
                                  SourceFilters.Any(y => y.ShouldShow(x)) 
                                  && LogLevelFilters.Any(y => y.ShouldShow(x))
                                  && (SearchLogFilter == null || SearchLogFilter.ShouldShow(x)));
        }

        private void _CreateFilters()
        {
            LogLevelFilters.Clear();

            foreach (var filter in LogLevelFilterSelections.Where(x => x.Selected == true))
            {
                var filterName = filter.FilterName;
                LogLevelFilters.Add(new LineFilter(x => x.LogLevel == filterName));
            }

            SourceFilters.Clear();

            foreach (var filter in SourceFilterSelections.Where(x => x.Selected == true))
            {
                var filterName = filter.FilterName;
                SourceFilters.Add(new LineFilter(x => x.Source == filterName));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void NotifyChanged(string propertyName)
        {
            var handler = PropertyChanged;
            if(handler != null)
                handler(this, new PropertyChangedEventArgs(propertyName));
        }

        public void ShowSourceFilterDialog()
        {
            var filters = _ShowFilterDialog(SourceFilterSelections);
            SourceFilterSelections = new ObservableCollection<FilterSelectrion>(filters);
            _RefreshCurrentLines();
        }

        private string _searchValue = "";
        public void ShowSearchFilterDialog()
        {
            var searchViewModel = new SearchViewModel();
            searchViewModel.SearchValue = _searchValue;
            var searchView = new SearchView();
            searchView.DataContext = searchViewModel;
            searchView.ShowDialog();
            if (searchViewModel.SearchValue == _searchValue)
                return;
            _searchValue = searchViewModel.SearchValue;
            SearchLogFilter = !String.IsNullOrEmpty(_searchValue)
                                  ? new LineFilter(x => x.Message.ToLower().Contains(_searchValue.ToLower())) 
                                  : null;
            _RefreshCurrentLines();
        }
    }

    public interface ILogFilter
    {
        bool ShouldShow(Line line);
    }

    public class LineFilter : ILogFilter
    {
        private readonly Func<Line, bool> _predicate;

        public LineFilter(Func<Line,bool> predicate)
        {
            _predicate = predicate;
        }

        public bool ShouldShow(Line line)
        {
            return _predicate(line);
        }
    }
    
    public class WaitCursor : IDisposable
    {
        public WaitCursor()
        {
            System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.WaitCursor;
        }

        public void Dispose()
        {
            System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Default;
        }
    }
}











