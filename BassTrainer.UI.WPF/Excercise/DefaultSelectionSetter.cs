using BassNotesMasterApi.Fretboard;

namespace BassNotesMaster.Excercise
{
    public class DefaultSelectionSetter : ISelectionSetter
    {
        public void InitSelection(FretboardManager fretboardManager)
        {
            fretboardManager.FretBoard.IgnoreColoring = true;
            fretboardManager.FretBoard.ForceClearView();
            fretboardManager.FretBoard.DrawAllGraphicNoteRepresentation();
            fretboardManager.SelectionManager.CleanUp();
        }
    }
}
