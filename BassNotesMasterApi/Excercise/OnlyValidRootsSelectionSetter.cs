using BassNotesMasterApi.Components.Fretboard;
using BassNotesMasterApi.Const;

namespace BassNotesMasterApi.Excercise
{
    public class OnlyValidRootsSelectionSetter : ISelectionSetter
    {
        private readonly NotesToStringFretBoardMapping _boardMapping = NotesToStringFretBoardMapping.Instance;
        private readonly NotesInfo _notesInfo = new NotesInfo();
        public void InitSelection(FretboardManager fretboardManager)
        {
            fretboardManager.FretBoard.IgnoreColoring = true;
            fretboardManager.FretBoard.ForceClearView();
            fretboardManager.FretBoard.DrawNotes(_boardMapping.GetFullOctaveScalesRootPosition(_notesInfo.OrderWithAccidentals));
            fretboardManager.SelectionManager.CleanUp();
        }
    }
}
