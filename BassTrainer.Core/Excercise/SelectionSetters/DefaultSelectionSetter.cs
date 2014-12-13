using BassTrainer.Core.Components.Fretboard;

namespace BassTrainer.Core.Excercise.SelectionSetters
{
    public class DefaultSelectionSetter : ISelectionSetter
    {
        public void InitSelection(FretboardComponent fretboardComponent)
        {
            fretboardComponent.FretBoard.IgnoreColoring = true;
            fretboardComponent.FretBoard.ForceClearView();
            fretboardComponent.FretBoard.DrawAllGraphicNoteRepresentation();
            fretboardComponent.SelectionManager.CleanUp();
        }
    }
}
