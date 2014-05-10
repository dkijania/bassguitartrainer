using BassNotesMasterApi.Utils;

namespace BassNotesMasterApi.Fretboard
{
    public interface IFretboardListener
    {
        void OnMouseClick(StringFretPair stringFretPair, FretBoard fretBoard);
    }
    
}