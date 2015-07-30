namespace DrumMachine.UI.WPF.Pattern
{
    public class DrumMachineTile
    {
        public int Row { get; protected set; }
        public int Column { get; protected set; }
        public event OnSelect OnSelectEvent;
        public event IgnoreMouseClick OnIgnoreMouseClick;
        
        public delegate void OnSelect(int row, int column, bool isSelected);
        public delegate bool IgnoreMouseClick();

        public DrumMachineTile(int row, int column)
        {
            Row = row;
            Column = column;
        }
        
        public virtual bool InvokeOnIgnoreMouseClick()
        {
            var handler = OnIgnoreMouseClick;
            return handler != null && handler();
        }

        public virtual void InvokeOnSelectEvent(int row, int column, bool isSelected)
        {
            var handler = OnSelectEvent;
            if (handler != null)
             handler(row,column,isSelected);
            
        }
    }
}
    