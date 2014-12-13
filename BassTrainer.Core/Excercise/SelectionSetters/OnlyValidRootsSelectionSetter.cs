using BassTrainer.Core.Components.Fretboard;
using BassTrainer.Core.Const;

namespace BassTrainer.Core.Excercise.SelectionSetters
{
    public class OnlyValidRootsSelectionSetter : ISelectionSetter
    {
        private readonly NotesToStringFretBoardMapping _boardMapping = NotesToStringFretBoardMapping.Instance;
        private readonly NotesInfo _notesInfo = new NotesInfo();
        public void InitSelection(FretboardComponent fretboardComponent)
        {
            fretboardComponent.FretBoard.IgnoreColoring = true;
            fretboardComponent.FretBoard.ForceClearView();
            fretboardComponent.FretBoard.DrawNotes(_boardMapping.GetFullOctaveScalesRootPosition(_notesInfo.OrderWithAccidentals));
            fretboardComponent.SelectionManager.CleanUp();
        }
    }
}
