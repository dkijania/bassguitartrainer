using BassTrainer.Core.Const;
using BassTrainer.Core.Utils;

namespace BassTrainer.Core.Components.Fretboard
{
    public interface IFretboardListener
    {
        void OnMouseClick(StringFretPair stringFretPair, FretBoard fretBoard);
    }
    
}