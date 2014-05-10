using BassNotesMasterApi.Const;
using BassNotesMasterApi.Fretboard;

namespace BassNotesMasterApi.Excercise
{
    public class OnlyValidRootsSelectionSetter : ISelectionSetter
    {
        private readonly NotesToStringFretBoardMapping _boardMapping = new NotesToStringFretBoardMapping();
        private readonly NotesInfo _notesInfo = new NotesInfo();
        public void InitSelection(FretboardManager fretboardManager)
        {
            fretboardManager.FretBoard.FretBoardGuiBuilder.IgnoreColoring = true;
            fretboardManager.FretBoard.FretBoardGuiBuilder.ForceClearView();
            fretboardManager.FretBoard.FretBoardGuiBuilder.DrawNotes(_boardMapping.GetFullOctaveScalesRootPosition(_notesInfo.OrderWithAccidentals));
            fretboardManager.SelectionManager.CleanUp();
        }
    }
}
