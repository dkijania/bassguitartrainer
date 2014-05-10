using System.Windows;
using BassNotesMasterApi.NotesView;

namespace BassNotesMaster.NotesView
{
    public class WpfNotesViewEventHandler : NotesViewEventHandler
    {
        public void OnClickEventHandler(object sender,RoutedEventArgs e)
        {
            OnClickEventHandler(sender);
        }
        
    }
}