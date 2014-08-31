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

            var viewmodel = new HelpSystemViewModel(this, GetHelpResourcesDescriptor());
            BackButtonItem.DataContext = viewmodel;
            ForwardButtonItem.DataContext = viewmodel;
            HelpMenuItem.DataContext = viewmodel;
       
        }
        
        public void PresentContent(FlowDocument flowDocument)
        {
           DocViewer.Document = flowDocument;
        }

        private static HelpResourcesDescriptor GetHelpResourcesDescriptor()
        {
            var helpResourcesDescriptor = new HelpResourcesDescriptor("HelpSystemResources");
            var settingsItem = new HelpTreeItem("Settings", "SettingsDocument.xaml");
            var components = new HelpTreeItem("Components");
            components.Children.Add(new HelpTreeItem("Fretboard", @"Component\Fretboard.xaml"));
            components.Children.Add(new HelpTreeItem("Excercise Panel", @"Component\ExcercisePanel.xaml"));
            components.Children.Add(new HelpTreeItem("Intervals", @"Component\Intervals.xaml"));
            components.Children.Add(new HelpTreeItem("Notation Canvas", @"Component\NotationCanvas.xaml"));
            components.Children.Add(new HelpTreeItem("Notes Panel", @"Component\Notes.xaml"));
            components.Children.Add(new HelpTreeItem("Player", @"Component\Fretboard.xaml"));
            helpResourcesDescriptor.DocumentTreeRoot.Add(settingsItem);
            helpResourcesDescriptor.DocumentTreeRoot.Add(components);
            return helpResourcesDescriptor;
        }
    }
}