using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Documents;
using MahApps.Metro.Controls;
using WpfExtensions;

namespace BassNotesMaster.WpfViews
{
    public interface IDocumentLoader
    {
        void LoadAndEnableNavigationForXaml(Uri uri);
    }

    public interface IDocumentPresenter
    {
        void PresentContent(FlowDocument flowDocument);
    }

    public class DocumentLoader : IDocumentLoader
    {
        private readonly Uri _uriBase = new Uri(@"WpfHelpSystem;;;component", UriKind.Relative);
        private readonly HelpHistoryPropertyNotifier<FlowDocument> _helpHistory;
        private readonly IDocumentPresenter _documentPresenter;
        
        public DocumentLoader(HelpHistoryPropertyNotifier<FlowDocument> helpHistory,
            IDocumentPresenter documentPresenter)
        {
            _helpHistory = helpHistory;
            _documentPresenter = documentPresenter;
        }

        public void LoadAndEnableNavigationForXaml(Uri uri)
        {
            var document = LoadXaml(uri);
            EnableHyperlinkTracebility(document);
            _helpHistory.Add(document);
            _documentPresenter.PresentContent(document);
        }

        private FlowDocument LoadXaml(Uri uri)
        {
            var uriPath = Path.Combine(_uriBase.OriginalString, uri.OriginalString);
            return (FlowDocument) Application.LoadComponent(new Uri(uriPath, UriKind.Relative));
        }

        private void EnableHyperlinkTracebility(FlowDocument document)
        {
            var hyperlinks = document.GetVisuals().OfType<Hyperlink>();
            foreach (var link in hyperlinks)
            {
                link.RequestNavigate -= LinkRequestNavigate;
                link.RequestNavigate += LinkRequestNavigate;
            }
        }

        private void LinkRequestNavigate(object sender, System.Windows.Navigation.RequestNavigateEventArgs e)
        {
          LoadAndEnableNavigationForXaml(e.Uri);
        }
    }

    /// <summary>
    /// Interaction logic for HelpView.xaml
    /// </summary>
    public partial class HelpView : IViewControl, IDocumentPresenter
    {
        private readonly HelpHistoryPropertyNotifier<FlowDocument> _helpHistory =
            new HelpHistoryPropertyNotifier<FlowDocument>();

        private readonly IDocumentLoader _documentLoader;

        public ObservableCollection<MenuTreeItem> MenuItems { get; private set; }

        public HelpView(MetroWindow mainWindow)
        {
            MetroWindow = mainWindow;
            InitializeComponent();
            Back.DataContext = _helpHistory;
            Forward.DataContext = _helpHistory;
            _documentLoader = new DocumentLoader(_helpHistory, this);
            var items  = new ObservableCollection<MenuTreeItem>();
            var settingsItem = new MenuTreeItem("Settings", new Uri("SettingsDocument.xaml", UriKind.Relative));

            var components = new MenuTreeItem("Components");
            components.Children.Add(new MenuTreeItem("Fretboard", new Uri(@"Component\Fretboard.xaml", UriKind.Relative)));
            components.Children.Add(new MenuTreeItem("Excercise Panel", new Uri(@"Component\ExcercisePanel.xaml", UriKind.Relative)));
            components.Children.Add(new MenuTreeItem("Intervals", new Uri(@"Component\Intervals.xaml", UriKind.Relative)));
            components.Children.Add(new MenuTreeItem("Notation Canvas", new Uri(@"Component\NotationCanvas.xaml", UriKind.Relative)));
            components.Children.Add(new MenuTreeItem("Notes Panel", new Uri(@"Component\Notes.xaml", UriKind.Relative)));
            components.Children.Add(new MenuTreeItem("Player", new Uri(@"Component\Fretboard.xaml", UriKind.Relative)));

            items.Add(settingsItem);
            items.Add(components);
             this.Menu.DataContext = this;
            Menu.ItemsSource = items;
        }

        public void PresentContent(FlowDocument flowDocument)
        {
            Viewer.Document = flowDocument;
        }

        private void HistoryBack_Click(object sender, RoutedEventArgs e)
        {
            Viewer.Document = _helpHistory.Undo();
        }

        private void HistoryForward_Click(object sender, RoutedEventArgs e)
        {
            Viewer.Document = _helpHistory.Redo();
        }

        public MetroWindow MetroWindow { get; set; }

        private void Menu_OnSelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            var item = (MenuTreeItem)e.NewValue;
            if(item.Children.Any())return;
            _documentLoader.LoadAndEnableNavigationForXaml(item.DocumentUri);
        }
    }


    public class MenuTreeItem
    {
        public MenuTreeItem(string header) : this(header,null)
        {
            Header = header;
        }

        public MenuTreeItem(string header, Uri documentUri)
        {
            Header = header;
            DocumentUri = documentUri;
            Children = new ObservableCollection<MenuTreeItem>();
        }

        public String Header { get; set; }
        public Uri DocumentUri { get; set; }
        public ObservableCollection<MenuTreeItem> Children { get; private set; }
    }


    public class HelpHistoryPropertyNotifier<T> : INotifyPropertyChanged
    {
        private readonly HelpHistory<T> _helpHistory = new HelpHistory<T>();

        public void Add(T t)
        {
            _helpHistory.Add(t);
            RaisePropertyChanged();
        }

        private void RaisePropertyChanged()
        {
            OnPropertyChanged("CanUndo");
            OnPropertyChanged("CanRedo");
        }

        public T Undo()
        {
            var document = _helpHistory.Undo();
            RaisePropertyChanged();
            return document;
        }

        public T Redo()
        {
            RaisePropertyChanged();
            var document = _helpHistory.Redo();
            RaisePropertyChanged();
            return document;
        }

        public bool CanUndo
        {
            get { return _helpHistory.CanUndo; }
        }

        public bool CanRedo
        {
            get { return _helpHistory.CanRedo; }
        }

        protected void OnPropertyChanged(string name)
        {
            var handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(name));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }

    public interface IHelpHistory<T>
    {
        bool CanUndo { get; }
        bool CanRedo { get; }
        void Add(T t);
        T Undo();
        T Redo();
    }

    public class HelpHistory<T> : IHelpHistory<T>
    {
        private readonly List<T> _history = new List<T>();
        private int _lastIndex = -1;

        public bool CanUndo
        {
            get { return _lastIndex > 0; }
        }

        public bool CanRedo
        {
            get { return _lastIndex < _history.Count - 1; }
        }

        public void Add(T t)
        {
            if (IsTheSamePageAlreadyAddedAtTheEnd(t))
            {
                return;
            }
            _history.Add(t);
            _lastIndex++;
        }

        private bool IsTheSamePageAlreadyAddedAtTheEnd(T t)
        {
            return _history.Any() && _history.Last().Equals(t);
        }

        public T Undo()
        {
            if (!CanUndo) throw new HelpHistoryException("Cannot Undo");
            return _history[--_lastIndex];
        }

        public T Redo()
        {
            if (!CanRedo) throw new HelpHistoryException("Cannot Redo");
            return _history[++_lastIndex];
        }


        public class HelpHistoryException : Exception
        {
            public HelpHistoryException(string message) : base(message)
            {
            }
        }
    }
}