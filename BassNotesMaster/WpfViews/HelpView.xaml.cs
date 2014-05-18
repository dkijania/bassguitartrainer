using System.Windows.Documents;
using MahApps.Metro.Controls;
using SimpleHelpSystem.DocumentLoader;
using SimpleHelpSystem.History;
using WpfSimpleHelpSystem;

namespace BassNotesMaster.WpfViews
{
    /// <summary>
    /// Interaction logic for HelpView.xaml
    /// </summary>
    public partial class HelpView : IViewControl, IDocumentPresenter<FlowDocument>
    {
        public HelpView(MetroWindow mainWindow)
        {
            MetroWindow = mainWindow;
            InitializeComponent();

            var viewmodel = new HelpSystemViewModel(this, GetHelpResourcesDescriptor());
            Back.DataContext = viewmodel;
            Forward.DataContext = viewmodel;
            Menu.DataContext = viewmodel;
       
        }
        
        public void PresentContent(FlowDocument flowDocument)
        {
            Viewer.Document = flowDocument;
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

        public MetroWindow MetroWindow { get; set; }

    }
}