namespace BassNotesMasterApi.Fretboard.SelectionManager
{
    public class StringFretSelectionManager
    {
       /* private readonly FretBoard _fretboard;

        public StringFretSelectionManager(FretBoard fretboard)
        {
            _fretboard = fretboard;
        }

        public IEnumerable<Border> SelectItems(string stringName, int startFret, int endFret)
        {
            return GetRangeOfSelectableItems(stringName, startFret, endFret);
        }

        private IEnumerable<Border> GetRangeOfSelectableItems(string stringName, int startFret, int endFret)
        {
            var selectableItemsList = new List<Border>();
            for (int i = Math.Min(startFret, endFret); i <= Math.Max(startFret, endFret); i++)
            {
                var stringFretPair = new StringFretPair(stringName, i);
                selectableItemsList.Add(
                    _fretboard.FretBoardGuiBuilder.GetBorderForCords(stringFretPair));
            }
            return selectableItemsList;
        }*/

    }
}