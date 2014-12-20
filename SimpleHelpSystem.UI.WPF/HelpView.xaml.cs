using System.Windows.Documents;
using SimpleHelpSystem.DocumentLoader;
using SimpleHelpSystem.History;

namespace SimpleHelpSystem.UI.WPF
{
    /// <summary>
    /// Interaction logic for HelpView.xaml
    /// </summary>
    public partial class HelpView : IDocumentPresenter<FlowDocument>
    {
        public HelpView()
        {
           InitializeComponent();

            var viewmodel = new HelpSystemViewModel(this);
            BackButtonItem.DataContext = viewmodel;
            ForwardButtonItem.DataContext = viewmodel;
            HelpMenuItem.DataContext = viewmodel;
        }
        
        public void PresentContent(FlowDocument flowDocument)
        {
           DocViewer.Document = flowDocument;
        }
    }
}