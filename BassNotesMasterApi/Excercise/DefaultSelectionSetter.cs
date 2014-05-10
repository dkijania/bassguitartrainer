using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BassNotesMasterApi.Fretboard;
using BassNotesMasterApi.Fretboard.SelectionManager;

namespace BassNotesMasterApi.Excercise
{
    public class DefaultSelectionSetter : ISelectionSetter
    {
        public void InitSelection(FretboardManager fretboardManager)
        {
            fretboardManager.FretBoard.FretBoardGuiBuilder.IgnoreColoring = true;
            fretboardManager.FretBoard.FretBoardGuiBuilder.ForceClearView();
            fretboardManager.FretBoard.FretBoardGuiBuilder.DrawAllGraphicNoteRepresentation();
            fretboardManager.SelectionManager.CleanUp();
        }
    }
}
