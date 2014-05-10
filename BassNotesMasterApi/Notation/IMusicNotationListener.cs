using BassNotesMasterApi.Utils;

namespace BassNotesMasterApi.Notation
{
    public interface IMusicNotationListener
    {
        void OnMouseClick(StringFretPair stringFretPair);
    }
}