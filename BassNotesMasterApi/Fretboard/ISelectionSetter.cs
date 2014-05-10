using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BassNotesMasterApi.Fretboard
{
    public interface ISelectionSetter
    {
        void InitSelection(FretboardManager fretBoard);
    }
}
