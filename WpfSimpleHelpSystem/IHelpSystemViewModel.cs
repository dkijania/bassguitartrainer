using System.Collections.ObjectModel;
using System.Windows.Documents;
using System.Windows.Input;
using SimpleHelpSystem.DocumentLoader;
using SimpleHelpSystem.History;

namespace WpfSimpleHelpSystem
{
    public interface IHelpSystemViewModel
    {
        IDocumentPresenter<FlowDocument> Presenter { get; }
        ICommand HistoryForwardCommand { get; }
        ICommand HistoryBackCommand { get; }
        ObservableCollection<HelpTreeItem> MenuItems { get;}

        void HistoryBack();
        void HistoryForward();
        void OnMenuSelectedItemChanged(object newSelectedValue);
    }
}