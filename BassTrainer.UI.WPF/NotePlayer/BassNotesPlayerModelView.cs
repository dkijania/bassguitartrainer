using System.Windows.Input;
using BassTrainer.Core.Components.NotePlayer;
using WpfExtensions;

namespace BassTrainer.UI.WPF.NotePlayer
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