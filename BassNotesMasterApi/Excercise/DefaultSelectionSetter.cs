using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BassNotesMasterApi.Components.Fretboard;

namespace BassNotesMasterApi.Excercise
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
