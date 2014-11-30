namespace DrumMachine.UI.WPF.Pattern
{
    public class DrumPatternPart
    {
        private readonly int _startIndex;
        private readonly int _length;
        private readonly bool _isSelected;
        private readonly int _rowId;
        private readonly int _bar;

        public DrumPatternPart(int rowId, int startIndex, int length, bool isSelected, int bar)
        {
            _rowId = rowId;
            _startIndex = startIndex;
            _length = length;
            _isSelected = isSelected;
            _bar = bar;
        }

        public int RowId
        {
            get { return _rowId; }
        }

        public int StartIndex
        {
            get { return _startIndex; }
        }

        public int Length
        {
            get { return _length; }
        }

        public bool IsSelected
        {
            get { return _isSelected; }
        }


        public int Bar
        {
            get { return _bar; }
        }
    }
}