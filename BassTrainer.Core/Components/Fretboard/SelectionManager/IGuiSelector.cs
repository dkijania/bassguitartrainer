using BassTrainer.Core.Const;
using BassTrainer.Core.Utils;

namespace BassTrainer.Core.Components.Fretboard.SelectionManager
{
    public interface IGuiSelector
    {
        void UnselectAllItems();
        void SelectItems(params StringFretPair[] collectionOfStringFretPair);
        void UnselectItems(params StringFretPair[] collectionOfStringFretPair);

    }
}