using BassNotesMasterApi.Utils;

namespace BassNotesMasterApi.Components.Notation
{
    public interface IMusicNotationListener
    {
        void OnMouseClick(StringFretPair stringFretPair);
    }
}