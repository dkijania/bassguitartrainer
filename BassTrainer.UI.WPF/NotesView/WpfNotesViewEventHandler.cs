using System.Windows;
using BassTrainer.Core.Components.NotesView;

namespace BassTrainer.UI.WPF.NotesView
{
    public class WpfNotesViewEventHandler : NotesViewEventHandler
    {
        public void OnClickEventHandler(object sender,RoutedEventArgs e)
        {
            OnClickEventHandler(sender);
        }
        
    }
}