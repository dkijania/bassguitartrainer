using BassNotesMasterApi.Utils;

namespace BassNotesMasterApi.Components.Fretboard
{
    public interface IFretboardListener
    {
        void OnMouseClick(StringFretPair stringFretPair, FretBoard fretBoard);
    }
    
}