using System.Windows.Input;
using BassNotesMasterApi.NotePlayer;
using WpfExtensions;

namespace BassNotesMaster.NotePlayer
{
    public class BassNotesPlayerModelView : BindingDataContextBase
    {
        public BassNotesPlayer Model { get; private set; }
        public ICommand MuteUnmuteCommand { get; private set; }
        public ICommand PlayCommand { get; private set; }
        
        public BassNotesPlayerModelView(BassNotesPlayer bassNotesPlayer)
        {
            Model = bassNotesPlayer;
            MuteUnmuteCommand = new DelegateCommand(Model.ChangeMuteState);
            PlayCommand = new DelegateCommand(Model.PlayAgain);
        }
    }
}