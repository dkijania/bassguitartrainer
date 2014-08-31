using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Documents;
using System.Windows.Input;
using SimpleHelpSystem.DocumentLoader;
using SimpleHelpSystem.History;
using SimpleHelpSystem.UI.WPF.Gui;
using WpfExtensions;

namespace SimpleHelpSystem.UI.WPF
{
    public class HelpSystemViewModel : BindingDataContextBase,IHelpSystemViewModel
    {
        public IDocumentPresenter<FlowDocument> Presenter { get; private set; }
        public ICommand HistoryForwardCommand { get; private set; }
        public ICommand HistoryBackCommand { get; private set; }
        public ObservableCollection<HelpTreeItem> MenuItems { get; private set; }
        
        private readonly HelpHistoryPropertyNotifier<FlowDocument> _helpHistory =
            new HelpHistoryPropertyNotifier<FlowDocument>();

        private readonly AbstractDocumentLoader<FlowDocument> _abstractDocumentLoader;

        public HelpSystemViewModel(IDocumentPresenter<FlowDocument> presenter,
            HelpResourcesDescriptor helpResourcesDescriptor)
        {

            Presenter = presenter;
            _abstractDocumentLoader = new DocumentLoader<FlowDocument>(helpResourcesDescriptor, _helpHistory,Presenter);

            HistoryForwardCommand = new DelegateCommand(HistoryForward);
            HistoryBackCommand = new DelegateCommand(HistoryBack);
            MenuItems = new ObservableCollection<HelpTreeItem>(helpResourcesDescriptor.DocumentTreeRoot);

            BindableSelectedItemBehavior.OnSelectedTreeItemChangedEvent += OnMenuSelectedItemChanged;

            _helpHistory.PropertyChanged += HelpHistoryPropertyChanged;
        }

        public void HistoryBack()
        {
            var document = _helpHistory.Undo();
            Present(document);
        }

        public void HistoryForward()
        {
            var document = _helpHistory.Redo();
            Present(document);
        }

        public bool CanUndo
        {
            get { return _helpHistory.CanUndo; }
        }

        public bool CanRedo
        {
             get { return _helpHistory.CanRedo; }
        }


        public void OnMenuSelectedItemChanged(object newSelectedValue)
        {
            var item = (HelpTreeItem) newSelectedValue;
            if (item.Children.Any()) return;
            var document = _abstractDocumentLoader.LoadAndEnableNavigation(item.DocumentUri);
            Present(document);
        }

        private void Present(FlowDocument document)
        {
            Presenter.PresentContent(document);
        }

        void HelpHistoryPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            const string canUndo = "CanUndo";
            const string canRedo = "CanRedo";

            if (e.PropertyName.Equals(canUndo))
            {
                OnPropertyChanged(canUndo);
            }
            if (e.PropertyName.Equals(canRedo))
            {
                OnPropertyChanged(canRedo);
            }
        }
    }
}