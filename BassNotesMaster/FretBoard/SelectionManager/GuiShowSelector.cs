using BassNotesMasterApi.Components.Fretboard;
using BassNotesMasterApi.Components.Fretboard.SelectionManager;
using BassNotesMasterApi.Utils;

namespace BassNotesMaster.FretBoard.SelectionManager
{
    public class GuiShowSelector : IGuiSelector
    {
        private readonly IFretBoardGuiBuilder _fretBoardGuiBuilder;

        public GuiShowSelector(IFretBoardGuiBuilder fretBoardGuiBuilder)
        {
            _fretBoardGuiBuilder = fretBoardGuiBuilder;
            _fretBoardGuiBuilder.AlwaysRedrawCollection.TransparencyRate = 1.0;
        }

        public void UnselectAllItems()
        {
            _fretBoardGuiBuilder.AlwaysRedrawCollection.Clear();
            _fretBoardGuiBuilder.ClearView();
        }

        public void SelectItems(params StringFretPair[] collectionOfStringFretPair)
        {
            _fretBoardGuiBuilder.AlwaysRedrawCollection.AddRange(collectionOfStringFretPair);
            _fretBoardGuiBuilder.DrawNotesIfNotExist(collectionOfStringFretPair);
        
        }

        public void UnselectItems(params StringFretPair[] collectionOfStringFretPair)
        {
        
        }

        public void SelectAllItems()
        {
            
        }
    }
}