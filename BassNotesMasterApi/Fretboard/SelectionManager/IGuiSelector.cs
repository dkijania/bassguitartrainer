using BassNotesMasterApi.Utils;

namespace BassNotesMasterApi.Fretboard.SelectionManager
{
    public interface IGuiSelector
    {
        void UnselectAllItems();
        void SelectItems(params StringFretPair[] collectionOfStringFretPair);
        void UnselectItems(params StringFretPair[] collectionOfStringFretPair);

    }
}